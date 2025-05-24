extends Node3D
class_name CameraRig

# Camera is attached to a player
var player: Node3D

# Adjust camera position
@export var offset: Vector3             = Vector3(0, 0, 6)
@export var min_distance: float         = 2.0
@export var max_distance: float         = 12.0
@export var height: float               = 2.5

# Adjust camera interactions
@export var sensitivity: float          = 0.002
@export var manual_zoom_intensity: float = 1.0
@export var auto_zoom_intensity: float   = 10.0
@export var min_pitch: float            = deg_to_rad(0)
@export var max_pitch: float            = deg_to_rad(80)
@export var rot_speed: float            = 10.0
@export var follow_speed: float         = 10.0

# Collision settings (Layer mask)
@export var collision_mask: int         = 1

# Current y- and x-axis angles
var yaw: float                          = 0.0
var pitch: float                        = 0.0

# Internal: current zoom distance
var current_distance: float

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	current_distance = offset.z

func _process(delta: float) -> void:
	if not player:
		return

	# 1) Compute view direction from yaw/pitch
	var dir: Vector3 = Vector3(
		sin(yaw) * cos(pitch),
		sin(pitch),
		cos(yaw) * cos(pitch)
	).normalized()

	# 2) Build raycast parameters
	var ray_origin: Vector3 = player.global_position + Vector3.UP * height
	var ray_target: Vector3 = ray_origin + dir * offset.z

	var params := PhysicsRayQueryParameters3D.create(ray_origin, ray_target, collision_mask)
	params.exclude = [ player.get_rid() ]

	var space = get_world_3d().direct_space_state
	var result: Dictionary = space.intersect_ray(params)

	# 3) Calculate target distance
	var target_distance: float = offset.z
	if len(result) > 0:
		# Jetzt wissen wir: result hat wirklich einen Eintrag "position"
		var hit_point: Vector3 = result["position"]
		var hit_dist: float = ray_origin.distance_to(hit_point) - 0.2
		target_distance = clamp(hit_dist, min_distance, offset.z)

	# 4) Smoothly interpolate current_distance toward target_distance
	current_distance = lerp(current_distance, target_distance, clamp(delta * auto_zoom_intensity, 0, 1))

	# 5) Calculate camera’s target position
	var cam_target: Vector3 = player.global_position + dir * current_distance + Vector3.UP * height

	# 6) Smooth follow (position)
	var new_pos: Vector3 = global_position.move_toward(cam_target, follow_speed * delta)

	# 7) Smooth rotation (slerp)
	var look_target: Vector3 = player.global_position + Vector3.UP * 1.5
	var forward: Vector3 = (look_target - new_pos).normalized()
	var desired_basis: Basis = Basis().looking_at(forward, Vector3.UP)
	var new_basis: Basis = global_transform.basis.slerp(desired_basis, rot_speed * delta)

	# 8) Apply transform
	global_transform = Transform3D(new_basis, new_pos)

func _unhandled_input(event: InputEvent) -> void:
	if PauseManager.is_paused():
		return

	# Mouse look
	if event is InputEventMouseMotion:
		yaw   -= event.relative.x * sensitivity
		pitch  = clamp(pitch + event.relative.y * sensitivity, min_pitch, max_pitch)

	# Mouse-wheel zoom override
	elif event is InputEventMouseButton and event.pressed:
		if event.button_index == MOUSE_BUTTON_WHEEL_UP:
			offset.z = max(min_distance, offset.z - manual_zoom_intensity)
		elif event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
			offset.z = min(max_distance, offset.z + manual_zoom_intensity)
		# Direkt syncen, damit das automatische Zoom-Verhalten unmittelbar darauf aufbaut
		current_distance = offset.z

func follow_target(target: Node3D) -> void:
	player = target
	var to_player: Vector3 = (player.global_position - global_position).normalized()
	yaw   = atan2(to_player.x, to_player.z)
	pitch = asin(to_player.y)
