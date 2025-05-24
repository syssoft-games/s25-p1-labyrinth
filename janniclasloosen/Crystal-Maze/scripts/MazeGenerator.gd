# File: res://scripts/MazeGenerator.gd
extends Node3D
class_name MazeGenerator

@export var map_file_path: String = "res://maps/maze.txt"
@export var tile_size: Vector3 = Vector3(6, 0, 6)

# Start & Ziel
@export var start_scene: PackedScene = preload("res://assets/maze/Start.tscn")
@export var target_scene: PackedScene = preload("res://assets/maze/Target.tscn")

# Basis-Pfad für Ecken/Kreuzungen und als Variante
@export var base_path_scene: PackedScene = preload("res://assets/maze/paths/BasePath.tscn")
@export var base_path_weight: float = 1.0  # Gewicht für BasePath bei einfacher Zelle

# Walls + ihre Gewichte
@export var wall_scenes: Array[PackedScene] = []
@export var wall_weights: Array[float] = []

# Paths + ihre Gewichte (für spezielle Pfade)
@export var path_scenes: Array[PackedScene] = []
@export var path_weights: Array[float] = []

func _ready() -> void:
	randomize()
	print("MazeGenerator starts.")
	assert(wall_scenes.size() == wall_weights.size(), "Wall-Szenen und Gewichte müssen gleichlang sein")
	assert(path_scenes.size() == path_weights.size(), "Path-Szenen und Gewichte müssen gleichlang sein")
	generate_maze()
	print("MazeGenerator finished.")

func generate_maze() -> void:
	if not FileAccess.file_exists(map_file_path):
		push_error("Map-File nicht gefunden: %s" % map_file_path)
		return

	var file: FileAccess = FileAccess.open(map_file_path, FileAccess.ModeFlags.READ)
	if file == null:
		push_error("Konnte Datei nicht öffnen: %s" % map_file_path)
		return

	var lines: Array[String] = []
	while not file.eof_reached():
		lines.append(file.get_line())
	file.close()

	var total_instances := 0
	for row in range(lines.size()):
		var line := lines[row]
		for col in range(line.length()):
			var c := line[col]
			var instance: Node3D = null

			match c:
				"#":
					instance = _choose_weighted(wall_scenes, wall_weights).instantiate() as Node3D
				" ":
					# Nur in einfachen Zellen Variationen erlauben
					if _is_simple_cell(lines, row, col):
						# Szene- und Gewichtelisten kopieren und base_path anhängen
						var scenes := path_scenes.duplicate()
						var weights := path_weights.duplicate()
						scenes.append(base_path_scene)
						weights.append(base_path_weight)
						instance = _choose_weighted(scenes, weights).instantiate() as Node3D
					else:
						# Kreuzung oder Ecke → immer BasePath
						instance = base_path_scene.instantiate() as Node3D
					_orient_corridor(instance, lines, row, col)
				"S":
					instance = start_scene.instantiate() as Node3D
				"T":
					instance = target_scene.instantiate() as Node3D
				_:
					pass

			if instance:
				total_instances += 1
				instance.position = Vector3(col * tile_size.x, 0, row * tile_size.z)
				add_child(instance)

	print(lines.size(), "x" , lines[0].length(), " Felder, ", total_instances, " Instanzen")

func _is_simple_cell(lines: Array[String], row: int, col: int) -> bool:
	var dirs := [Vector2(0,-1), Vector2(0,1), Vector2(-1,0), Vector2(1,0)]
	var open_dirs := []
	for d in dirs:
		var r := row + int(d.y)
		var c := col + int(d.x)
		if r >= 0 and r < lines.size() and c >= 0 and c < lines[r].length():
			if lines[r][c] != "#":
				open_dirs.append(d)
	if open_dirs.size() == 1:
		return true
	if open_dirs.size() == 2 and (
		(open_dirs[0].x == 0 and open_dirs[1].x == 0) or
		(open_dirs[0].y == 0 and open_dirs[1].y == 0)
	):
		return true
	return false

func _orient_corridor(instance: Node3D, lines: Array[String], row: int, col: int) -> void:
	var dirs := [Vector2(0,-1), Vector2(0,1), Vector2(-1,0), Vector2(1,0)]
	var open_dirs := []
	for d in dirs:
		var r := row + int(d.y)
		var c := col + int(d.x)
		if r >= 0 and r < lines.size() and c >= 0 and c < lines[r].length() and lines[r][c] != "#":
			open_dirs.append(d)

	var target_rot := 0.0
	if open_dirs.size() == 1:
		match open_dirs[0]:
			Vector2(0,-1): target_rot = 0
			Vector2(0, 1): target_rot = PI
			Vector2(-1,0): target_rot = -PI/2
			Vector2(1, 0): target_rot = PI/2
	elif open_dirs.size() == 2:
		# Durchgang
		if open_dirs[0].x == 0 and open_dirs[1].x == 0:
			target_rot = 0
		else:
			target_rot = PI/2

	# zufällig 180° Variation
	if randf() < 0.5:
		target_rot += PI

	instance.rotation.y = target_rot

func _choose_weighted(scenes: Array[PackedScene], weights: Array[float]) -> PackedScene:
	var total := 0.0
	for w in weights:
		total += w
	if total <= 0.0:
		push_error("Summe der Gewichte muss > 0 sein")
		return scenes[0]
	var r := randf() * total
	var acc := 0.0
	for i in range(weights.size()):
		acc += weights[i]
		if r < acc:
			return scenes[i]
	return scenes[scenes.size() - 1]
