extends Node2D
class_name Boss

@export var speed: float = 30.0
@export var health: int = 100

var velocity: Vector2 = Vector2.ZERO

func _ready() -> void:
	# Add a simple sprite for visibility
	var sprite = Sprite2D.new()
	#sprite.texture = preload("res://icon.png")  # Uses Godot's default icon
	sprite.scale = Vector2(1.0, 1.0)  # Larger for boss
	sprite.modulate = Color(1.0, 0.2, 0.2)  # Red tint for boss
	add_child(sprite)

	# Add collision shape
	var collision_shape = CollisionShape2D.new()
	collision_shape.shape = RectangleShape2D.new()
	collision_shape.shape.extents = Vector2(32, 32)
	add_child(collision_shape)

func _process(delta: float) -> void:
	# Slow, predictable movement for testing
	position.y += speed * delta * 0.5

	# Wrap around screen
	if position.y > 800:
		position.y = -100

func die() -> void:
	queue_free()
