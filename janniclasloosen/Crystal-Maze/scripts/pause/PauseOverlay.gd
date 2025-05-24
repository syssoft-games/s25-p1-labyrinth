# PauseOverlay.gd
extends Control

@onready var label := $Background/CenterContainer/Label

func _ready() -> void:
	hide()
	PauseManager.connect("pause_toggled", Callable(self, "_on_pause_toggled"))

func _on_pause_toggled(paused: bool) -> void:
	if paused:
		label.text = PauseManager.get_message()
		show()
	else:
		hide()
