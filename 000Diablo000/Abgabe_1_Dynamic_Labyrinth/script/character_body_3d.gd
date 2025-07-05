extends CharacterBody3D

@export var move_speed: float = 5.0
@export var run_multiplier: float = 2.0
@export var jump_velocity: float = 6.0
@export var gravity: float = 29.4
@export var max_velocity: float = 12.0
@export var infinit_jump: bool = true

var mouse_sensitivity := 0.005
var twist_input := 0.0
var pitch_input := 0.0

@onready var twist_pivot := $TwistPivot
@onready var pitch_pivot := $TwistPivot/PitchPivot

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _physics_process(delta: float) -> void:
	# Input sammeln
	var input_dir := Vector3(
		Input.get_axis("left", "right"),
		0,
		Input.get_axis("forward", "backward")
	).normalized()
	
	if Input.is_action_just_pressed("ui_cancel"):
		if Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
			Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
			get_tree().paused = false
		elif Input.mouse_mode == Input.MOUSE_MODE_VISIBLE:
			Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
			get_tree().paused = true

	var speed := move_speed
	if Input.is_action_pressed("run"):
		speed *= run_multiplier

	var direction = twist_pivot.basis * input_dir
	direction.y = 0  # Nur horizontale Bewegung

	# Zielgeschwindigkeit berechnen
	var target_velocity = direction * speed
	velocity.x = move_toward(velocity.x, target_velocity.x, speed * 5 * delta)
	velocity.z = move_toward(velocity.z, target_velocity.z, speed * 5 * delta)

	# Gravitation anwenden
	if not is_on_floor():
		velocity.y -= gravity * delta
	else:
		if Input.is_action_just_pressed("jump"):
			velocity.y = jump_velocity
		else:
			velocity.y = -0.1  # sanftes Andocken am Boden
	if infinit_jump:
		if Input.is_action_just_pressed("jump"):
			velocity.y = jump_velocity

	# Bewegung anwenden
	move_and_slide()

	# Kamera drehen
	twist_pivot.rotate_y(twist_input)
	pitch_pivot.rotate_x(pitch_input)
	pitch_pivot.rotation.x = clamp(pitch_pivot.rotation.x, deg_to_rad(-90), deg_to_rad(90))
	twist_input = 0.0
	pitch_input = 0.0

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		twist_input = -event.relative.x * mouse_sensitivity
		pitch_input = -event.relative.y * mouse_sensitivity
