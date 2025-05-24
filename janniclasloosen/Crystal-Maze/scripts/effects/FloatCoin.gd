extends Node3D

# Exportierte Variablen im Inspector
@export var max_height: float = 50.0  # wie weit er über den Startpunkt hinaus steigt
@export var speed:      float = 2.0   # Wie schnell gebobbt wird (Periodendauer)

# Interne Variablen
var _origin: Vector3      # Start-Position als Unterkante
var _time_acc: float = 0  # akkumulierte Laufzeit

func _ready() -> void:
	_origin = global_transform.origin

func _process(delta: float) -> void:
	# Laufzeit akkumulieren
	_time_acc += delta

	# Sinus-Welle zwischen –1 und +1
	var sine_wave = sin(_time_acc * speed)
	# Normierung auf [0,1]
	var t = (sine_wave + 1.0) * 0.5
	# Offset von 0 bis max_height
	var offset_y = lerp(0.0, max_height, t)

	# Neue Position setzen
	global_transform.origin = _origin + Vector3(0, offset_y, 0)
