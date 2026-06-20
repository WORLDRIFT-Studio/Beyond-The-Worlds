extends CharacterBody3D

## Skrypt kontrolujący pracę free camery.
##
## Odpowiada za swobodne poruszanie się kamery w przestrzeni 3D za pomocą klawiatury (WASD + SHIFT/SPACE) oraz obrót kamery za pomocą myszy.


#region Move Parameters
@export_category("Move parameters")
## Czułość obrotu kamery. Wyższe wartości oznaczają szybszy obrót. Zakres od 0.001 do 1, z krokiem 0.01.
@export_range(0.001, 1, 0.01, "prefer_slider") var sensitive: float = 0.001

## Prędkość poruszania się kamery. Wyższe wartości oznaczają szybsze poruszanie się. Zakres od 10 do 100, z krokiem 0.1.
@export_range(10, 100, 0.1, "prefer_slider") var speed: float = 5.0

##	Siła grawitacji działająca na kamerę. Wyższe wartości oznaczają silniejszą grawitację (szybsze opadanie). Zakres od 1 do 10, z krokiem 0.5.
@export_range(1, 10, 0.5, "prefer_slider") var gravity: float = 5.0
#endregion

#region Nodes
@onready var _camera: Camera3D = %Camera3D
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
		
	var dir: Vector3 = (_camera.transform.basis * Vector3(input_dir.x, move_up_down, input_dir.y)).normalized()
	
	if dir:
		velocity = velocity.move_toward(dir * speed, 200.0 * delta)
	else:
		velocity = velocity.move_toward(Vector3.ZERO, 200.0 * delta)
	
	move_and_slide()
	
func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		_camera.rotation.y -= (PI * event.relative.x * sensitive)
		_camera.rotation.x -= (PI * event.relative.y * sensitive)

		_camera.rotation.x = clampf(_camera.rotation.x, -PI/2, PI/2)
		
	
#endregion
