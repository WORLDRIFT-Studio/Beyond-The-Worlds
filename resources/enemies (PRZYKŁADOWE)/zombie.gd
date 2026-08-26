extends Node2D
class_name Zombie

@export var speed: float = 50.0
@export var health: int = 10

var velocity: Vector2 = Vector2.ZERO

func _ready() -> void:
	# Optional: Add a simple sprite for visibility
	var sprite = Sprite2D.new()
	#sprite.texture = preload("res://icon.png")  # Uses Godot's default icon
	sprite.scale = Vector2(0.5, 0.5)
	add_child(sprite)

	# Add collision shape for basic physics
	var collision_shape = CollisionShape2D.new()
	collision_shape.shape = RectangleShape2D.new()
	collision_shape.shape.extents = Vector2(16, 16)
	add_child(collision_shape)

func _process(delta: float) -> void:
	# Simple downward movement for testing
	position.y += speed * delta

	# Wrap around screen
	if position.y > 800:
		position.y = -100

func die() -> void:
	# Called when enemy should be removed
	queue_free()

	# Emit custom signal if needed
	# emit_signal("died")
