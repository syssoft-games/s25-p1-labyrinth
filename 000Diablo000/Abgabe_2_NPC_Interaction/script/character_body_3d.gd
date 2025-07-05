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
var jumping := false  # Muss außerhalb der Funktion, aber nur EINMAL definiert sein!

@onready var twist_pivot := $TwistPivot
@onready var pitch_pivot := $TwistPivot/PitchPivot
@onready var anim_player := $TwistPivot/Player_Charakter_animated/XBot/AnimationPlayer

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _physics_process(delta: float) -> void:
	if is_chat_focused():
		return
	# Input sammeln
	var input_dir := Vector3(
		Input.get_axis("left", "right"),
		0,
		Input.get_axis("forward", "backward")
	).normalized()

	var horizontal_speed = Vector2(velocity.x, velocity.z).length()

	# Animationen
	if is_on_floor():
		if jumping:
			jumping = false
		if horizontal_speed > 0.1:
			var forward := Input.get_action_strength("forward")
			var backward := Input.get_action_strength("backward")
			var left := Input.get_action_strength("left")
			var right := Input.get_action_strength("right")

			if left > 0.1 and forward == 0 and backward == 0:
				anim_player.play("Strafe run right")  # <-- Passe den Namen ggf. an
			elif right > 0.1 and forward == 0 and backward == 0:
				anim_player.play("Strafe run right_001")
			elif Input.is_action_pressed("run"):
				anim_player.play("Run")
			else:
				anim_player.play("walk")
		else:
			anim_player.play("Idle")
	else:
		if not jumping:
			jumping = true
			anim_player.play("Jump")

	# Maus-Modus umschalten
	if Input.is_action_just_pressed("ui_cancel"):
		if Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
			Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
			get_tree().paused = false
		elif Input.mouse_mode == Input.MOUSE_MODE_VISIBLE:
			Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
			get_tree().paused = true

	# Geschwindigkeit berechnen
	var speed := move_speed
	if Input.is_action_pressed("run"):
		speed *= run_multiplier

	var direction = twist_pivot.basis * input_dir
	direction.y = 0  # Nur horizontale Bewegung

	var target_velocity = direction * speed
	velocity.x = move_toward(velocity.x, target_velocity.x, speed * 5 * delta)
	velocity.z = move_toward(velocity.z, target_velocity.z, speed * 5 * delta)

	# Gravitation & Springen
	if not is_on_floor():
		velocity.y -= gravity * delta
	else:
		if Input.is_action_just_pressed("jump"):
			velocity.y = jump_velocity
		else:
			velocity.y = -0.1
	if infinit_jump:
		if Input.is_action_just_pressed("jump"):
			velocity.y = jump_velocity

	move_and_slide()

	# Kamera drehen
	twist_pivot.rotate_y(twist_input)
	pitch_pivot.rotate_x(pitch_input)
	pitch_pivot.rotation.x = clamp(pitch_pivot.rotation.x, deg_to_rad(-90), deg_to_rad(90))
	twist_input = 0.0
	pitch_input = 0.0

func _unhandled_input(event: InputEvent) -> void:
	if is_chat_focused():
		return

	if event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		twist_input = -event.relative.x * mouse_sensitivity
		pitch_input = -event.relative.y * mouse_sensitivity


func is_chat_focused() -> bool:
	var chat_input = get_node_or_null("/root/Game/CanvasLayer/ChatInput")
	return chat_input != null and chat_input.visible and chat_input.has_focus()
