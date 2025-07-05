extends CharacterBody3D

const SPEED := 3.0
const JUMP_VELOCITY := 6.0
const GRAVITY := 29.4

var move_direction := Vector3.ZERO
var jump_requested := false
var current_action := "Idle"

@onready var anim_player: AnimationPlayer = $Y_Bot/AnimationPlayer
@onready var twist_pivot := $TwistPivot

func _physics_process(delta: float) -> void:
	# Gravitation anwenden
	if not is_on_floor():
		velocity.y -= GRAVITY * delta
	else:
		if jump_requested:
			velocity.y = JUMP_VELOCITY
			jump_requested = false
		else:
			velocity.y = -0.1

	# Bewegung übernehmen
	var horizontal_velocity := move_direction * SPEED
	velocity.x = horizontal_velocity.x
	velocity.z = horizontal_velocity.z

	move_and_slide()

	# Animation anpassen
	_update_animation()

func _update_animation() -> void:
	if not is_on_floor():
		_play_anim("locomotion-library/Jump")
	elif move_direction.length() > 0.1:
		_play_anim("locomotion-library/Run")  # Du kannst auch zwischen walk/run unterscheiden
	else:
		_play_anim("locomotion-library/Idle")

func _play_anim(name: String) -> void:
	if anim_player.current_animation != name:
		anim_player.play(name)

# Beispielaktionen (zum Test)
func walk_forward():
	move_direction = transform.basis.z

func walk_backward():
	move_direction = -transform.basis.z

func walk_left():
	move_direction = -transform.basis.x

func walk_right():
	move_direction = transform.basis.x

func stop():
	move_direction = Vector3.ZERO

func jump():
	if is_on_floor():
		jump_requested = true


func _on_http_request_request_completed(result: int, response_code: int, headers: PackedStringArray, body: PackedByteArray) -> void:
	pass # Replace with function body.
