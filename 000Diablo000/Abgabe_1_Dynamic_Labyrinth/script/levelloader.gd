extends Node3D

@export var level_image: Texture2D
@export var tile_size: float = 4.0
@export var debug_colors: bool = true

@export var player_scene: PackedScene
@export var ground_scene: PackedScene
@export var wall_scene: PackedScene
@export var transparent_scene: PackedScene
@export var reflecting_scene: PackedScene
@export var absorbing_scene: PackedScene
@export var start_scene: PackedScene
@export var finish_scene: PackedScene

const COLOR_TOLERANCE := 0.05
var player_spawn_position: Vector3
var seen_colors := {}

var color_map := {
	Color.WHITE: func(pos):
		_spawn(ground_scene, pos),

	Color.BLACK: func(pos):
		_spawn(wall_scene, pos),

	Color(0.1333, 0.6941, 0.298): func(pos): # Grün – Startpunkt
		_spawn(ground_scene, pos)
		_spawn(start_scene, pos)
		player_spawn_position = pos,

	Color(0.9294, 0.1098, 0.1412): func(pos): # Rot – Zielpunkt
		_spawn(ground_scene, pos)
		_spawn(finish_scene, pos),

	Color(0.498, 0.498, 0.498): func(pos): # Grau – transparente Wand
		_spawn(ground_scene, pos)
		_spawn(transparent_scene, pos),

	Color(0.0, 0.6353, 0.9098): func(pos): # Cyan – reflektierende Wand
		_spawn(ground_scene, pos)
		_spawn(reflecting_scene, pos),

	Color(1.0, 0.949, 0.0): func(pos): # Gelb – absorbierende Wand
		_spawn(ground_scene, pos)
		_spawn(absorbing_scene, pos),
}

func _ready():
	if not level_image:
		print("⚠️ Kein Level-Bild gesetzt!")
		return

	var image := level_image.get_image()
	if image is Image:
		image.convert(Image.FORMAT_RGB8)

	for y in range(image.get_height()):
		for x in range(image.get_width()):
			var color := image.get_pixel(x, y)
			var world_pos := Vector3(x * tile_size, 0, y * tile_size)

			var matched := false
			for ref_color in color_map.keys():
				if is_color_close(color, ref_color):
					color_map[ref_color].call(world_pos)
					matched = true
					break

			if debug_colors and not matched and not seen_colors.has(color):
				seen_colors[color] = true
				print("🟡 Nicht zugewiesene Farbe:", color, " bei (", x, ",", y, ")")

	# ✅ Player instanziieren und positionieren
	call_deferred("_spawn_player")

func _spawn_player():
	if player_scene and player_spawn_position != null:
		var player = player_scene.instantiate()
		add_child(player)
		player.global_position = player_spawn_position + Vector3(0, 1, 0)
		print("✅ Spieler gespawnt bei:", player.global_position)
	else:
		print("⚠️ Kein Player oder keine Spawn-Position gefunden!")

func is_color_close(a: Color, b: Color, tolerance := COLOR_TOLERANCE) -> bool:
	return abs(a.r - b.r) < tolerance \
		and abs(a.g - b.g) < tolerance \
		and abs(a.b - b.b) < tolerance

func _spawn(scene: PackedScene, position: Vector3) -> void:
	if scene:
		var instance = scene.instantiate()
		add_child(instance)
		instance.global_position = position
