extends OmniLight3D

@export var min_intensity: float = 1.0
@export var max_intensity: float = 3.0
@export var flicker_speed: float = 5.0
@export var color_variation: float = 0.2
@export var flicker_smoothness: float = 0.9

var flicker_time: float = 0.0
var base_color: Color

func _ready() -> void:
	base_color = light_color
	set_process(true)

func _process(delta: float) -> void:
	# Update the flicker timer
	flicker_time += delta * flicker_speed
	
	# Use a Perlin noise for smoother, more organic flicker
	var intensity_factor = (sin(flicker_time) + 1.0) / 2.0
	var new_intensity = lerp(min_intensity, max_intensity, intensity_factor)
	light_energy = new_intensity
	
	# Smoothly adjust the color based on the intensity
	var color_offset = (randf() - 0.5) * color_variation
	var new_color = base_color
	new_color.r = clamp(new_color.r + color_offset, 0.8, 1.0)
	new_color.g = clamp(new_color.g + color_offset * 0.6, 0.4, 0.6)
	new_color.b = clamp(new_color.b + color_offset * 0.3, 0.2, 0.4)
	light_color = light_color.lerp(new_color, flicker_smoothness * delta)
