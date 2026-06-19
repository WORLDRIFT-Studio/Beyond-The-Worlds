extends CharacterBody3D

#region Move Parameters
@export_category("Move parameters")
@export_range(0.001, 1, 0.01, "prefer_slider") var sensitive: float = 0.001
@export_range(10, 100, 0.1, "prefer_slider") var speed: float = 5.0
@export_range(1, 10, 0.5, "prefer_slider") var gravity: float = 5.0
#endregion

#region Nodes
@onready var camera: Camera3D = %Camera3D
#endregion

#region Logic
func _ready() -> void:
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func _physics_process(delta: float) -> void:
	var input_dir: Vector2 = Input.get_vector("move_left", "move_right", "move_forward", "move_backward")
	var move_up_down: float = 0.0
	
	if Input.is_action_pressed("move_up"):
		move_up_down += gravity
	if Input.is_action_pressed("move_down"):
		move_up_down -= gravity
		
	var dir: Vector3 = (camera.transform.basis * Vector3(input_dir.x, move_up_down, input_dir.y)).normalized()
	
	if dir:
		velocity = velocity.move_toward(dir * speed, 200.0 * delta)
	else:
		velocity = velocity.move_toward(Vector3.ZERO, 200.0 * delta)
	
	move_and_slide()
	
func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		camera.rotation.y -= (PI * event.relative.x * sensitive)
		camera.rotation.x -= (PI * event.relative.y * sensitive)

		camera.rotation.x = clampf(camera.rotation.x, -PI/2, PI/2)
		
	
#endregion
