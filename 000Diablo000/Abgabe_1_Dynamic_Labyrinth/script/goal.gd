extends Area3D

var label: Label

func _ready():
	label = get_tree().root.get_node("game/Goal_notifier/CanvasLayer/Label")  # Pfad anpassen
	connect("body_entered", Callable(self, "_on_body_entered"))

func _on_body_entered(body):
	if body.name == "NewPlayer":
		if label:
			label.text = "🎯 Ziel erreicht!"
			label.visible = true
			await get_tree().create_timer(3.0).timeout
			label.visible = false
