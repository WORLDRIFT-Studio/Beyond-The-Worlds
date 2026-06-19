extends Node3D

@export_category("Orbital Camera")
@export_range(0.1, 1, 0.1, "prefer_slider") var cam_sense: float = 0.2
@export_range(0.1, 1, 0.1, "prefer_slider") var zoom_sense: float = 0.1  


@onready var camera: Camera3D = %Camera3D
@onready var cam_gimbal: Node3D = %CamGimbal


func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		if Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
			rotate_y(deg_to_rad(-event.relative.x * cam_sense))
			cam_gimbal.rotate_x(deg_to_rad(-event.relative.y * cam_sense))
