# Projektüberblick

Dieses Projekt ist ein 3D-Labyrinth-Generator, der aus beliebigen Bilddateien automatisch ein spielbares Labyrinth entstehen lässt. Nutzer wählen einfach ein Bild aus, das System erkennt anhand festgelegter Farbcodierungen die Eigenschaften verschiedener Spielfelder (z. B. begehbar, Wand, Spawnpunkt) und baut daraus eine begehbare, dreidimensionale Spielwelt auf. Zielgruppe sind Spielende und Entwickler:innen, die unkompliziert eigene 3D-Level auf Basis grafischer Vorlagen erschaffen möchten.

---

## Technologien

- **Godot Engine 4.5 Development Snapshot 4** https://godotengine.org/article/dev-snapshot-godot-4-5-dev-4/
- **.NET 9.0 (C# 13)** https://dotnet.microsoft.com/en-us/download
- **Blender 4.4.3** https://www.blender.org/download/
  (Erstellung und Export der 3D-Meshes für Tiles und Level-Strukturen)
- **Paint.NET**  
  (Bearbeitung und Vorbereitung der Quellbilder als Level-Grundlage)

---

## Szenen-Struktur

Die zentrale Szene des Spiels ist in der Datei `game_scene.tscn` definiert und bildet die Grundlage für das gesamte Gameplay sowie die Menüführung. Die Struktur setzt sich aus mehreren Hauptknoten und Unterknoten zusammen, die verschiedene Verantwortlichkeiten abdecken. Hier ein Überblick der wichtigsten Bestandteile:

- **Application [Node]**
    - Einstiegspunkt der Szene, verknüpft mit der Hauptapplikations-Logik.
- **Core [Node]**
    - Beinhaltet alle Hauptmenüs und Controller-Elemente:
        - **MenuController**
            - Steuert das Hauptmenü, Setup-, Tutorial-, Options-, Credit- und Pause-Menü.
            - Jedes dieser Menüs ist als eigenes PackedScene-Modul eingebunden und bietet ein flexibles Navigationssystem.
        - **TimeController**
            - Steuert spielzeitbasierte Abläufe (z. B. Zeitlimits, Timer).

- **Game [Node3D]**
    - Wichtige Grundstruktur für alles, was im eigentlichen Level passiert:
        - **StaticBody3D**
            - Definiert die spielweltbegrenzende Physik-Umgebung (Kollisionsbereich als WorldBoundaryShape3D).
            - Enthält eine meist unsichtbare **MeshInstance3D** (Plane als Spielfeld-Basis).
        - **PlayerController3D [Node3D]**
            - Verantwortlich für die Steuerung des Spielers und der Kamera.
            - Beinhaltet den eigentlichen Spieler-Avatar (**Player3D**), welcher eine eigene PackedScene ist.
        - **LevelController3D**
            - Verwaltet den aktuellen Spielfeldausschnitt, den Levelaufbau und die Platzierung der Tiles sowie dynamischer Objekte.
        - **GameUI**
            - Die Benutzeroberfläche für das eigentliche Gameplay (z. B. Anzeigen für Leben, Items, Zeit usw.).

---

## Klassenüberblick: Core-Komponenten

### Erweiterungen

- **NodeExtensions**  
  Erweiterungsmethoden für Godot-Knoten, z. B. um Kinder eines bestimmten Typs einfach zu finden oder alle Kinder eines Knotens zu löschen.
- **IListExtensions**  
  Praktische Erweiterungen für Listen, etwa für Indexabfragen, Filtern, sichere Elementabfragen oder „Stack“- und „Queue“-Operationen.

### Abstrakte Komponenten

- **Function**  
  Abstrakte Basisklasse zur Definition von beliebigen Funktionsknoten, die über eine einheitliche `Execute()`-Methode ausgelöst werden.

- **InputActionAbility**  
  Abstrakte Klasse für Fähigkeiten, die auf bestimmte Eingabeaktionen (Input Actions) reagieren. Unterstützt Konfiguration, wann eine Aktion auslöst und bindet entsprechende Logik ein.

- **Component**
  Abstrakte Klasse, die eine Komponente eines Characters darstellt z.B Inventar, Lebenspunkte oder anderes.

### Menuführung

- **Menu**  
  Basisklasse für Menü-Panels. 
- **MenuController**  
  Zentrales Steuerungselement für alle Menüs. 
- **MenuNavButton**  
  Navigationsbutton innerhalb eines Menüs. Liefert das Zielmenü und regelt die Sichtbarkeit beim Drücken

### Tag-Nacht-Zyklus

- **TimeController**  
  Steuert die Spielzeit (z. B. Tageszeit) und synchronisiert ggf. die Umgebungsbeleuchtung. Ernöglicht Events bei Zeitänderungen und hält den aktuellen Tages- und Zeitstand bereit.

---

## Klassenüberblick: Game-Komponenten

### Spieler

- **PlayerController und Player3D**
  Verwaltet den aktuellen Spieler
- **Player3dAbility**
  Abstrakte Basisklasse für konkrete input-basierte Spieler-Fähigkeiten

### Levels
- **LevelController3D, LevelGenerator3D und Level3D**
  Das Zusammenspiel aus Verwaltung des aktuellen Levels, den Nodes im aktiven Level und einer Generator Klasse zum dynamischen Erstellen anhand von Daten aus einem Bild

### Items
- **Item3D, ItemData, ItemCollectArea3D, ItemBody3D**
  Die 3D Repräsentation eines Items, seine Daten, der Collider (Area3D) um es einzusammeln und der aktive Collider des Items

---

## Haupt-Features
- **Dateiauswahl:** Über einen FileDialog kann ein Bild ausgewählt werden.
- **Vorschau:** Nach der Auswahl wird das Bild als Levelvorschau angezeigt.
- **Levelaufbau:** Beim Starten wird das Bild analysiert und jeder Pixel interpretiert:
    - **Schwarz** (`RGB: 0,0,0`) → Kein Feld (Wand/Blockade)
    - **Weiß** (`RGB: 255,255,255`) → Begehbares Feld
    - **Grün** (`RGB: 0,255,0`) → Start-/Spawnpunkt
    - **Rot** (`RGB: 255,0,0`) → Zielpunkt
    - **Blau** (`RGB: 0,0,255`) → Item zum Einsammeln
- **3D Generierung:** Beim Starten des Spiels wird ein 3D-Labyrinth generiert.
- **Tiles und Assets:** Für jede Art (Beton oder Glas) gibt es 15 verschiedene Tile-Kombinationen. Mindestens 15 % Glas im Labyrinth).
- **Flexible Steuerung:** Vordefinierte Actions für Tastatur, Maus und Gamepad (alles anpassbar im Menü).
- **Szenen-Struktur:** Übersichtliche Node-Hierarchie mit getrennten Bereichen für Spiellogik, Spieler, Levelaufbau und UI.

---

## Erweiterte Features

- **Tag-Nacht-Zyklus:** Dynamische Atmosphäre per Shader, Tageszeit im HUD sichtbar.
- **Sammelbare Items:** Inventarsystem mit verschiedenen Items (z. B. Schlüssel, Power-Ups).
- **Interactables:** Hebel, Türen, Truhen und andere interaktive Levelobjekte.
- **Komfortable Menüs:** Hauptmenü, Pause, Optionen, Tutorial und Credits.
- **Komponentensystem:** Spielerfähigkeiten modular erweiterbar.
- **Eigene GUIs:** Für Inventar und Tageszeit.

---

### Spielersteuerung

Die Steuerung erfolgt über vordefinierte Input Actions, die sowohl Tastatur, Maus als auch Gamepad abdecken. Im Folgenden sind alle Hauptaktionen und deren Standardbelegungen aufgeführt:

| Aktion                              | Standard-Zuordnung                                                     |
|--------------------------------------|------------------------------------------------------------------------|
| **Spiel pausieren**                  | `game_pause` - Escape (oder Gamepad: Taste 5)                          |
| **Nach links bewegen**               | `player_move_left` - Taste A / Gamepad linker Stick                    |
| **Nach rechts bewegen**              | `player_move_right` - Taste D / Gamepad linker Stick                   |
| **Vorwärts bewegen**                 | `player_move_forward` - Taste W / Gamepad linker Stick                 |
| **Rückwärts bewegen**                | `player_move_backward` - Taste S / Gamepad linker Stick                |
| **Kamera nach links drehen**         | `player_look_left` - Maus bewege / Pfeil links / Gamepad rechter Stick |
| **Kamera nach rechts drehen**        | `player_look_right` - Maus bewege / Pfeil rechts / Gamepad rechter Stick             |
| **Kamera nach oben schauen**         | `player_look_up` - Maus bewege / Pfeil oben / Gamepad rechter Stick                  |
| **Kamera nach unten schauen**        | `player_look_down` - Maus bewege / Pfeil unten / Gamepad rechter Stick               |
| **Interagieren**                     | `player_interact` - Taste F / Maus: rechte Taste                       |
| **Inventar: nächste Slot**           | `player_inventory_slot_next` - Mausrad runter / Taste C                |
| **Inventar: vorheriger Slot**        | `player_inventory_slot_previous` - Mausrad hoch / Taste X              |
| **Item aufnehmen**                   | `player_item_collect` - Taste E                                        |
| **Item ablegen**                     | `player_item_drop` - Taste Q                                           |

---

## Bilder
### Godot Engine
![image](https://github.com/user-attachments/assets/c037f2b6-634b-424f-91a5-d0657fa0dba2)
![image](https://github.com/user-attachments/assets/39631ebe-39b6-4d3f-9388-904bd45fb695)
![image](https://github.com/user-attachments/assets/4dd5f6ec-a862-4047-b74a-c03752addefb)
![image](https://github.com/user-attachments/assets/ba3e737a-0f71-420b-9366-5d849c33e077)
![image](https://github.com/user-attachments/assets/d3979971-9e95-4f42-8737-433fdaa45c05)

### Blender  
![image](https://github.com/user-attachments/assets/fef0e628-6421-4b6d-ac76-9dfa3764a6e6)


