extends Node3D

class_name MovementTestNode

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func _unhandled_input(event: InputEvent) -> void:
	if(Input.is_action_pressed("forward")):
		print("UP")
	elif(Input.is_action_pressed("backward")):
		print("DOWN")
	elif(Input.is_action_pressed("left")):
		print("LEFT")
	elif(Input.is_action_pressed("right")):
		print("RIGHT")
