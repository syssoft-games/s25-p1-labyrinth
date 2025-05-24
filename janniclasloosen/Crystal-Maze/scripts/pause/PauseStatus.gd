# PauseStatus.gd
# (optional stub; Input wird jetzt zentral im PauseManager gehandhabt)
extends Node

@export var pause_reason: String = "Game Paused"

func pause() -> void:
	PauseManager.enable_pause(pause_reason)

func resume() -> void:
	PauseManager.disable_pause()
