extends Node3D

## Skrypt kontrolujący pracę kamery orbitalnej 3D.
##
## Odpowiada za obrót kamery wokół punktu centralnego (głównego węzła) za pomocą myszy
## oraz za przybliżanie/oddalanie widoku (zoom) realizowane poprzez skalowanie węzła.

#region zmienne exportowe
@export_category("Orbital Camera Settings")

## Czułość obrotu kamery w osiach X i Y. Wyższy zakres oznacza szybszy obrót.
@export_range(0.1, 1.0, 0.1, "prefer_slider") var cam_sense: float = 0.2

## Czułość przybliżania/oddalania (zoom) za pomocą kółka myszy. Wyższe wartości oznaczają większy krok zoom.
@export_range(0.1, 1.0, 0.1, "prefer_slider") var zoom_sense: float = 0.1  
#endregion

#region referencje do węzłów
# Referencja do unikalnego węzła podrzędnego typu Node3D odpowiedzialnego za obrót w osi X.
@onready var _cam_gimbal: Node3D = %CamGimbal
#endregion

#region zmienne
# Wektor pomocniczy przechowujący przetworzoną wartość czułości zoomu dla operacji na Vector3.
var _zoom_packed: Vector3
#endregion

#region funkcje głowne
func _ready() -> void:
	_zoom_packed = Vector3(zoom_sense, zoom_sense, zoom_sense)


func _unhandled_input(event: InputEvent) -> void: 
	
	# Obsługa obrotu kamery wokół punktu (lewy przycisk myszy + ruch)
	if event is InputEventMouseMotion:
		if Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
			rotate_y(deg_to_rad(-event.relative.x * cam_sense))
			_cam_gimbal.rotate_x(deg_to_rad(-event.relative.y * cam_sense))

	# Obsługa przybliżania i oddalania widoku za pomocą kółka myszy
	if event is InputEventMouseButton:
		# Przybliżenie (zmniejszenie skali węzła)
		if Input.is_mouse_button_pressed(MOUSE_BUTTON_WHEEL_UP):
			scale -= _zoom_packed
			
		# Oddalenie (zwiększenie skali węzła)
		if Input.is_mouse_button_pressed(MOUSE_BUTTON_WHEEL_DOWN):
			scale += _zoom_packed
			
		scale = scale.clamp(Vector3(0.2, 0.2, 0.2), Vector3(1.0, 1.0, 1.0))
#endregion
