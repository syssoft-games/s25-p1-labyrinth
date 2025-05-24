# TargetStatus.gd
extends Node3D

@export var message: String = "The End"

var _area: Area3D

func _ready() -> void:
	_area = $Area3D
	_area.body_entered.connect(_on_body_entered)

func _on_body_entered(body: Node) -> void:
	if body.is_in_group("player"):
		PauseManager.enable_pause(message)
