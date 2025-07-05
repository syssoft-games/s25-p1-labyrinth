extends Node3D

@export var icon_node_path: NodePath
@export var animation_player_path: NodePath
@export var http_node_path: NodePath
@export var npc_path: NodePath

var http: HTTPRequest
var anim_player: AnimationPlayer
var thinking_icon: Node = null
var npc: Node = null  # Initialisierung erst in _ready()
var current_command: String = ""
var current_history: String = ""

func _ready():
	print("🔧 YBotAI ready")
	
	http = get_node(http_node_path)
	if http == null:
		push_error("HTTPRequest-Node wurde NICHT gefunden! Pfad: %s" % http_node_path)
	else:
		print("HTTPRequest-Node gefunden: %s" % http.name)

	anim_player = get_node(animation_player_path)

	if icon_node_path != NodePath():
		thinking_icon = get_node(icon_node_path)
		thinking_icon.visible = false

	if npc_path != NodePath():
		npc = get_node(npc_path)

	# Sicherstellen, dass Signal verbunden ist
	if not http.is_connected("request_completed", Callable(self, "_on_HTTPRequest_request_completed")):
		http.connect("request_completed", Callable(self, "_on_HTTPRequest_request_completed"))

func send_command_to_ai(command: String, history: String = "") -> void:
	print("send_command_to_ai wurde wirklich aufgerufen")
	print("Befehl: %s" % command)

	current_command = command
	current_history = history

	if http == null:
		push_error("Kein HTTPRequest-Node vorhanden. Abbruch.")
		return

	if thinking_icon:
		thinking_icon.visible = true

	var json_data = {
		"state": "idle",
		"player_input": command,
		"history": history
	}
	var body = JSON.stringify(json_data)
	var url = "http://host.docker.internal:8000/npc/respond"

	print("Sende POST an: ", url)
	print("Body: ", body)

	var error = http.request(
		url,
		["Content-Type: application/json"],
		HTTPClient.METHOD_POST,
		body
	)

	if error != OK:
		push_error("Fehler beim Senden: %s" % error)
	else:
		print("Anfrage erfolgreich abgesendet.")

func _on_HTTPRequest_request_completed(result: int, response_code: int, headers: PackedStringArray, body: PackedByteArray) -> void:
	print("Anfrage abgeschlossen")
	print("Statuscode: ", response_code)
	print("Antwort (RAW): ", body.get_string_from_utf8())

	if thinking_icon:
		thinking_icon.visible = false

	if response_code != 200:
		push_error("Fehler vom Server: %d" % response_code)
		return

	var parsed = JSON.parse_string(body.get_string_from_utf8())
	if parsed == null:
		push_error("JSON ungültig.")
		return

	handle_ai_response(parsed)

func handle_ai_response(response: Dictionary):
	if response.has("reaction") and npc != null:
		match response["reaction"]:
			"Idle", "Stop":
				npc.stop()
			"Forward":
				npc.walk_forward()
			"Backward":
				npc.walk_backward()
			"Left":
				npc.walk_left()
			"Right":
				npc.walk_right()
			"Jump":
				npc.jump()
			_:
				npc.stop()

	if response.has("say"):
		var npc_text = response["say"]
		print("NPC sagt: %s" % npc_text)

		var canvas_layer = get_tree().get_root().get_node("Game/CanvasLayer")
		if canvas_layer.has_method("add_message_to_log"):
			canvas_layer.add_message_to_log("NPC", npc_text)
