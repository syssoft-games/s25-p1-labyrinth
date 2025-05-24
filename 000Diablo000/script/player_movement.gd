extends RigidBody3D

@export var move_speed : float = 2.0
@export var jump_velocity : float = 6.0
@export var gravity : float = 29.4
@export var max_velocity: float = 12.0

# Mouse Controls
var mouse_sensitivity := 0.001
var twist_input := 0.0
var pitch_input := 0.0

@onready var twist_pivot := $TwistPivot
@onready var pitch_pivot := $TwistPivot/PitchPivot

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var input := Vector3.ZERO
	input.x = Input.get_axis("left", "right")
	input.z = Input.get_axis("forward", "backward")
	
	var input_direction = twist_pivot.basis * input.normalized()
	var horizontal_velocity = linear_velocity
	horizontal_velocity.y = 0  # Y ausklammern

	if input.length() > 0.01 and horizontal_velocity.length() < max_velocity or Input.is_action_pressed("run") and input.length() > 0.01 and horizontal_velocity.length() < max_velocity * 2:
		apply_central_force(input_direction * 1200.0 * delta)
	
	if Input.is_action_just_pressed("jump") and is_on_floor():
		apply_central_impulse(Vector3.UP * jump_velocity)
		apply_central_force(Vector3.DOWN * gravity)
	
	if Input.is_action_just_pressed("ui_cancel"):
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		
	twist_pivot.rotate_y(twist_input)
	pitch_pivot.rotate_x(pitch_input)
	pitch_pivot.rotation.x = clamp(pitch_pivot.rotation.x,
		deg_to_rad(-90),
		deg_to_rad(90)
	)
	twist_input = 0.0
	pitch_input = 0.0

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		if Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
			twist_input = - event.relative.x * mouse_sensitivity
			pitch_input = - event.relative.y * mouse_sensitivity
			
func is_on_floor() -> bool:
	return linear_velocity.y < 0.05 and linear_velocity.y > -0.05
	
