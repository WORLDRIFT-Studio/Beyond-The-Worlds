extends Node2D
class_name Soldier

@export var speed: float = 70.0
@export var health: int = 20

var velocity: Vector2 = Vector2.ZERO

func _ready() -> void:
	# Add a simple sprite for visibility
	var sprite = Sprite2D.new()
	#sprite.texture = preload("res://icon.png") # Uses Godot's default icon
	sprite.scale = Vector2(0.5, 0.5)
	sprite.modulate = Color(0.2, 0.6, 1.0)  # Blue tint for soldier
	add_child(sprite)

	# Add collision shape
	var collision_shape = CollisionShape2D.new()
	collision_shape.shape = RectangleShape2D.new()
	collision_shape.shape.extents = Vector2(20, 20)
	add_child(collision_shape)

func _process(delta: float) -> void:
	# Diagonal movement for testing
	position.x += speed * 0.5 * delta
	position.y += speed * delta

	# Wrap around screen
	if position.x > 1200:
		position.x = -100
	if position.y > 800:
		position.y = -100

func die() -> void:
	queue_free()
