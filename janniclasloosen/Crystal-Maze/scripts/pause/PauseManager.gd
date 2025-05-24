# PauseManager.gd
extends Node

signal pause_toggled(paused: bool)

var _pause_active: bool = false
var _pause_message: String = ""

func _ready() -> void:
	set_process(true)

func _process(delta: float) -> void:
	if Input.is_action_just_pressed("pause"):
		if _pause_active:
			disable_pause()
		else:
			enable_pause("Game Paused")

func enable_pause(msg: String) -> void:
	_pause_message = msg
	_pause_active = true
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	emit_signal("pause_toggled", true)

func disable_pause() -> void:
	print("Pause disabled")
	_pause_active = false
	_pause_message = ""
	emit_signal("pause_toggled", false)

func is_paused() -> bool:
	return _pause_active

func get_message() -> String:
	return _pause_message
