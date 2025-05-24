extends CharacterBody3D

@export var walk_speed: float = 4
@export var run_speed: float  = 8.0
@export var turn_speed: float = 180.0

@onready var anim_player: AnimationPlayer = $HeroAsset/AnimationPlayer

func _ready() -> void:
	add_to_group("player")

func _physics_process(delta: float) -> void:
	# Do not move, if the game is paused. 
	if PauseManager.is_paused():
		velocity = Vector3.ZERO
		move_and_slide()
		if anim_player.current_animation != "Idle":
			anim_player.play("Idle")
		return

	# Calculate movement based on inputs. 
	var input_vec = Vector3.ZERO
	if Input.is_action_pressed("move_forward"):
		input_vec -= transform.basis.z
	if Input.is_action_pressed("move_backward"):
		input_vec += transform.basis.z
	if Input.is_action_pressed("rotate_left"):
		rotation.y += deg_to_rad(turn_speed * delta)
	if Input.is_action_pressed("rotate_right"):
		rotation.y -= deg_to_rad(turn_speed * delta)
	input_vec = input_vec.normalized()

	# Speed can be switched.
	var speed: float
	if Input.is_action_pressed("toggle_walk"):
		speed = walk_speed
	else:
		speed = run_speed

	velocity = input_vec * speed
	move_and_slide()

	# Show animations
	if input_vec.length() > 0.01:
		var target_anim: String
		if Input.is_action_pressed("toggle_walk"):
			target_anim = "Walk"
		else:
			target_anim = "Run"
		if anim_player.current_animation != target_anim:
			anim_player.play(target_anim)
	else:
		if anim_player.current_animation != "Idle":
			anim_player.play("Idle")
