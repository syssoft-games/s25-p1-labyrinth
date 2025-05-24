extends Node3D

@onready var player = $PlayerBody
@onready var camera_rig = $CameraRig

func _ready():
	camera_rig.follow_target(player)
