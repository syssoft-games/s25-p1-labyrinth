
# 3D-Labyrinth in Godot Engine 4.4.1

## 1. Einleitung

Im Rahmen dieser Aufgabe wurde ein begehbares 3D-Labyrinth erstellt, das mithilfe von Ray-Tracing-artiger Beleuchtung visualisiert wird. Die Umsetzung erfolgte mit der Godot Engine in der Version 4.4.1 (stable). Die Struktur des Labyrinths basiert auf einem zweidimensionalen PNG-Plan, dessen Farbwerte automatisch in unterschiedliche Objekte übersetzt werden. Das Ziel dieses Berichts ist es, die zugrunde liegende Softwarestruktur und ihre Interaktion mit der Engine zu dokumentieren.

## 2. Engine-Überblick: Godot 4.4.1

Godot ist eine open-source Game Engine, die sowohl 2D- als auch 3D-Spiele unterstützt.  
**Wichtige Eigenschaften der Engine:**  
- **Programmiersprachen**: GDScript (Python-ähnlich), C#, C++ (optional)  
- **Node-basiertes System**: Jede Szene ist eine Baumstruktur aus Nodes  
- **Cross-Platform**: Export auf Web, Windows, Linux, macOS, Android, iOS  
- **Editor-integriert**: Echtzeit-Vorschau, Shader-Editor, Visual Script, Debugger  
- **Rendering**: Unterstützung für Vulkan (seit Godot 4), SSAO, SSIL, SDFGI  

Trotz umfangreicher Grafikoptionen bietet Godot aktuell noch **kein vollwertiges Ray Tracing**. Stattdessen wurden alternative Techniken wie **SDFGI (Signed Distance Field Global Illumination)** sowie **SSAO** und **SSIL** verwendet, um eine ähnliche visuelle Qualität zu erreichen.

## 3. Softwarestruktur

Die Szenenstruktur basiert auf einer Hauptszene namens `Game`, die alle relevanten Elemente des Labyrinths enthält. Die Szene wurde durch ein dynamisches Level-Parsing aus einem PNG-Bild aufgebaut. Die Farben im Bild bestimmen, welcher Blocktyp erstellt wird.

### 3.1 Aufbau der Szene `Game`

Die Szene `Game` enthält folgende Elemente:
```
Game (Node3D)
├── LevelLoader (Node)           → Lädt die PNG-Karte und instanziiert die passenden Blöcke dynamisch
├── Ground (StaticBody3D)
│   ├── MeshInstance3D           → Flächendarstellung des Bodens
│   └── CollisionShape3D         → Kollision für den Boden
├── WorldEnvironment             → Legt globale Beleuchtung und Umgebungseffekte fest (z. B. SDFGI, Nebel)
├── Wall (Instanzierte Szene)
│   ├── MeshInstance3D
│   ├── CollisionShape3D
│   ├── OmniLight3D              → Lokale Lichtquelle
│   ├── MeshInstance3D           → Weitere Mesh-Komponente (z. B. Struktur)
│   └── street_lamp_02_4k        → Subszene für Dekoration/Licht
│       └── OmniLight3D
├── Sun                          → Hauptlichtquelle (z. B. directional light)
├── Moon                         → Alternative Lichtquelle (z. B. für Nachtmodus)
├── start                        → Spielerstartpunkt
├── Transparent_Wall
│   ├── MeshInstance3D
│   ├── CollisionShape3D
│   └── OmniLight3D              → Leuchtet durch transparente Wand
├── Absorbing_Wall
│   ├── MeshInstance3D
│   └── CollisionShape3D         → Schluckt Licht (Material-Shader vermutlich mit niedriger Albedo)
├── Reflecting_Wall
│   ├── MeshInstance3D
│   ├── CollisionShape3D
│   └── ReflectionProbe          → Simuliert Spiegelungen in Echtzeit
└── Goal_notifier (Node)         → Logik zur Zielerreichung (z. B. Signalverbindung)
```
Hinweis zur Architektur:

- Alle Elemente wie Wall, Transparent_Wall, Reflecting_Wall usw. sind als eigenständige Szenen (.tscn) gespeichert. Das macht das Level modular und wiederverwendbar.
- Viele der Blöcke enthalten zusätzlich Lichter (OmniLight3D) zur Simulation von Lichtquellen innerhalb des Labyrinths.
- Der LevelLoader übernimmt die dynamische Erzeugung der Szene basierend auf einem 2D-Bitmap-Plan (PNG-Datei).

### 3.2 Spielerstruktur (`NewPlayer`)

Die Steuerung des Spielers erfolgt über ein First-Person-Controller-Modell mit Drehachsen für Kamera und Licht. Godot liefert eine CharacterBody3D Node inkl. Template, um die Steuerung des Players zu erleichtern:

```
NewPlayer (CharacterBody3D)
├── MeshInstance3D       → Visuelles Modell des Spielers (z. B. für Körper oder Rüstung)
├── CollisionShape3D     → Kollisionsform für Physik-Interaktion
├── TwistPivot (Node3D)  → Drehung um Y-Achse (horizontal), Basis für Maussteuerung
    └── PitchPivot (Node3D) → Drehung um X-Achse (vertikal), Kamera- und Lichtsteuerung
        ├── Camera3D     → Spielerperspektive (First-Person)
        └── SpotLight3D  → "Taschenlampe", an die Kamera gekoppelt
```

### 3.3 Dynamischer Levelaufbau

Die Logik des `LevelLoader` interpretiert ein PNG-Bild und erstellt dynamisch Instanzen entsprechender Blöcke basierend auf Farbwerten.  
Beispiel:
- `#000000` → Wand  
- `#00FF00` → Startpunkt  
- `#FF0000` → Ziel  
- `#FFFFFF` → Plattform

Die Objekte werden dabei als Szeneninstanzen geladen und positioniert. Diese Entkopplung ermöglicht ein schnelles Testen neuer Layouts durch einfaches Austauschen des Levelbilds.

## 3.4 Überblick über zentrale Skripte

### `new_player.gd` – Spielersteuerung (First Person)

Dieses Skript steuert den **beweglichen Spieler-Charakter** auf Basis von `CharacterBody3D`.

- Ermöglicht **WASD-Steuerung**, Springen, Rennen (mit Multiplikator) und optionalen Flugmodus (`infinit_jump`)
- Kamera-Rotation durch zwei verschachtelte Pivots:
  - `TwistPivot`: horizontale Drehung (Y-Achse)
  - `PitchPivot`: vertikale Drehung (X-Achse)
- Mausbewegung beeinflusst die Blickrichtung (`_unhandled_input`)
- Umschaltfunktion für Maus-Capture und Pause mit der `ESC`-Taste
- Exportierte Variablen erlauben einfache Anpassung im Editor:
  - z. B. `move_speed`, `jump_velocity`, `gravity`, `mouse_sensitivity`

---

### `goal.gd` – Zielerkennung (Work in Progress)

Einfaches Skript, das an ein `Area3D` gebunden ist, um zu erkennen, **wann der Spieler das Ziel erreicht**.

- Erkennt Kollision mit `NewPlayer` über `body_entered`
- Anzeige einer Textnachricht über ein `Label` im UI-Baum (Goal_notifier)
- Nachricht bleibt 3 Sekunden sichtbar, dann wird sie automatisch ausgeblendet

---

### `levelloader.gd` – Dynamischer Levelgenerator aus PNG

Liest eine **PNG-Grafik** ein und erstellt aus Farbpixeln passende Szeneninstanzen.

- Exportierte Parameter:
  - `level_image`: PNG-Levelplan
  - `tile_size`: Abstand der generierten Objekte
  - Verschiedene Szenen (`wall_scene`, `start_scene`, `reflecting_scene`, etc.)
- Unterstützt mehrere Farben:
  - Schwarz → Wand
  - Weiß → Boden
  - Grün → Startpunkt (setzt auch `player_spawn_position`)
  - Rot → Zielpunkt
  - Grau → transparente Wand
  - Cyan → reflektierende Wand
  - Gelb → absorbierende Wand
- Farbvergleich erfolgt mit Toleranz (`is_color_close`)
- Optionales Debugging: Ausgabe unbekannter Farben zur Laufzeit
- Spieler wird nach dem Laden an `player_spawn_position` gespawnt

---

## 4. Fazit

Die Umsetzung von echtem Ray Tracing war in Godot nicht praktikabel, da keine stabile, öffentlich nutzbare Implementation verfügbar ist. Der Versuch, ältere Shader-Demos nachzubauen, erwies sich als zu aufwändig. Stattdessen wurden SDFGI, SSAO und SSIL verwendet, um ein möglichst realistisches Lichtgefühl zu erzeugen.

Trotz dieser Einschränkung hat sich die Godot Engine als äußerst leistungsfähig erwiesen. Die Kombination aus modularer Node-Struktur, einfacher Skriptanbindung und aktiver Community macht sie zu einer hervorragenden Wahl – auch für 3D-Projekte. Dank Vulkan-Unterstützung ist es nur eine Frage der Zeit, bis auch echtes Ray Tracing vollständig integriert ist. Ich werde Godot definitiv weiterhin nutzen.

Ich habe einige weitere Materials und Tests im Sourcecode welche ich für mehr Abwechslung und neuen Mechaniken einbauen wollte. Allerdings konnte ich diese nicht bis zur Abgabe vervollständigen. Es sollten noch kleine Wände, Plattformen, ein Zielsegment mit notification (existiert, aber ohne Benachrichtigung), richtige Spiegel, Blender-Model import (Im Game enthalten, aber Textur ein wenig falsch), welcher die Umgebung, aber ohne Spieler, gespiegelt hat (Einstellungen verloren und kein Backup - nur ein Screenshot existiert noch).

## Demo – 3D-Labyrinth

### Level Loader
![Level Loader](media/00_level_loader.gif)

### Materialsystem & Shader
![Materialsystem](media/01_materials.gif)

### Lichteffekte
![Lichteffekte](media/02_lights.gif)

### Reflektionen mit Custom Shader
![Reflektion](media/03_reflection.gif)

### Engine & Materialien in der Übersicht
![Engine Materialien](media/04_engine_materials.gif)
