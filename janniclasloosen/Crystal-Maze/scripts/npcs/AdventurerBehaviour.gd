extends Node3D

@export var detection_message: String = "Player nearby!"
@export var exit_message: String = "Player left the area."
@export var idle_animation: String = "CharacterArmature|Idle_Neutral"
@export var wave_animation: String = "CharacterArmature|Wave"

func _ready() -> void:
	var area = $Area3D
	area.body_entered.connect(_on_body_entered)
	area.body_exited.connect(_on_body_exited)
	play_animation(idle_animation)

func play_animation(animation_name: String) -> void:
	var animation_player = $Adventurer/AnimationPlayer
	
	if not animation_player.has_animation(animation_name):
		push_error("Animation not found: " + animation_name)
		return
	
	animation_player.stop()
	animation_player.seek(0, true)
	animation_player.play(animation_name)
	
	if animation_name == wave_animation:
		# Überprüfen, ob das Signal bereits verbunden ist
		if not animation_player.is_connected("animation_finished", Callable(self, "_on_wave_finished")):
			animation_player.animation_finished.connect(Callable(self, "_on_wave_finished"))

func _on_wave_finished(anim_name: String) -> void:
	if anim_name == wave_animation:
		play_animation(idle_animation)

func _on_body_entered(body: Node) -> void:
	if body.is_in_group("player"):
		play_animation(wave_animation)

func _on_body_exited(body: Node) -> void:
	if body.is_in_group("player"):
		play_animation(idle_animation)
