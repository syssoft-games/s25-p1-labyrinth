extends CanvasLayer

@export var chat_input_path: NodePath
@export var chat_log_path: NodePath
@export var ybot_ai_path: NodePath

var chat_input: LineEdit
var chat_log: RichTextLabel
var ybot_ai: Node = null
var ybot_ai_ready := false

func _ready():
	chat_input = get_node(chat_input_path)
	chat_log = get_node(chat_log_path)
	chat_input.visible = false
	chat_input.clear()

	await get_tree().process_frame
	await get_tree().process_frame

	await find_ybot_ai_later()


func find_ybot_ai_later():
	for i in 10:
		await get_tree().process_frame

		var npc = get_tree().get_root().find_child("NPC", true, false)
		if npc and npc.has_node("Y_Bot"):
			ybot_ai = npc.get_node("Y_Bot")
			if ybot_ai and ybot_ai.has_method("send_command_to_ai"):
				print("YBotAI gefunden zur Laufzeit:", ybot_ai.name)
				ybot_ai_ready = true
				return
			else:
				push_error("YBotAI hat keine Methode send_command_to_ai")
				return
		else:
			print("Suche nach YBotAI... (Versuch %d)" % (i + 1))

	push_error("YBotAI konnte nicht gefunden werden.")

func _input(event):
	if event is InputEventKey and event.pressed and not event.echo and event.keycode == KEY_ENTER:
		if chat_input.visible and chat_input.has_focus():
			var text = chat_input.text.strip_edges()
			if text != "":
				print("Spieler sagt:", text)
				chat_log.append_text("[Spieler]: " + text + "\n")

				if ybot_ai_ready:
					var history := build_chat_history()
					print("📡 Sende an YBotAI:", text)
					ybot_ai.send_command_to_ai(text, history)
				else:
					push_error("YBotAI nicht bereit – Eingabe konnte nicht gesendet werden.")

			chat_input.clear()
			chat_input.visible = false
			chat_input.release_focus()
			Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
			get_viewport().set_input_as_handled()
		else:
			chat_input.visible = true
			await get_tree().process_frame
			chat_input.grab_focus()
			Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
			get_viewport().set_input_as_handled()

func build_chat_history() -> String:
	var lines := chat_log.text.split("\n")
	var trimmed := lines.slice(max(lines.size() - 20, 0), lines.size())
	return "\n".join(trimmed)

func add_message_to_log(author: String, message: String):
	chat_log.append_text("[%s]: %s\n" % [author, message])
