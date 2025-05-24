extends Node3D

@onready var area: Area3D = $Area3D

var players_inside: int = 0
var closed_scale_y: float
const OPEN_SCALE_Y := 1.0
const CLOSE_SCALE_Y := 2.8

func _ready() -> void:
	closed_scale_y = scale.y
	area.monitoring = true
	area.monitorable = true
	area.body_entered.connect(_on_body_entered)
	area.body_exited.connect(_on_body_exited)

func _on_body_entered(body: Node) -> void:
	if body.is_in_group("player"):
		players_inside += 1
		if players_inside == 1:
			_scale_gate(OPEN_SCALE_Y)

func _on_body_exited(body: Node) -> void:
	if body.is_in_group("player"):
		players_inside = max(players_inside - 1, 0)
		if players_inside == 0:
			_scale_gate(CLOSE_SCALE_Y)

func _scale_gate(target_y_scale: float) -> void:
	var tween = create_tween()
	tween.tween_property(self, "scale:y", target_y_scale, 0.5)
