extends Node2D
class_name EnemySpawner

# Reference to your wave generator
@export var wave_generator: WaveGenerator

# Spawn configuration
@export var spawn_position: Vector2 = Vector2.ZERO
@export var spawn_area_size: Vector2 = Vector2.ONE  # Random offset area around spawn_position
@export var spawn_enabled: bool = true

# Internal state tracking
var current_wave: WaveData = null
var current_segment_index: int = 0
var enemies_spawned_in_segment: int = 0
var time_since_last_spawn: float = 0.0
var is_spawning_active: bool = false
var waves_completed: int = 0

# Signals
signal wave_started(wave_index)
signal wave_completed(wave_index)
signal all_waves_completed

func _ready() -> void:
	# Optional: Auto-start first wave when spawner is added to scene
	# Uncomment the next line if you want waves to start automatically
	start_wave(0)
	pass

func start_wave(wave_index: int) -> void:
	if not spawn_enabled:
		print("Spawner is disabled. Enable 'spawn_enabled' in inspector to start waves.")
		return

	if wave_generator == null:
		push_error("EnemySpawner: wave_generator is not assigned! Please assign it in the inspector.")
		return

	print("Starting wave %d" % wave_index)
	emit_signal("wave_started", wave_index)

	current_wave = wave_generator.generate_wave(wave_index)
	current_segment_index = 0
	enemies_spawned_in_segment = 0
	time_since_last_spawn = 0.0
	is_spawning_active = true

	if current_wave.segments.size() == 0:
		print("Wave %d is empty!" % wave_index)
		is_spawning_active = false
		wave_completed()

func stop_wave() -> void:
	is_spawning_active = false
	current_wave = null
	print("Wave spawning stopped.")

func _process(delta: float) -> void:
	if not is_spawning_active or not spawn_enabled or current_wave == null:
		return

	time_since_last_spawn += delta

	# Check if we've finished all segments in current wave
	if current_segment_index >= current_wave.segments.size():
		is_spawning_active = false
		waves_completed += 1
		emit_signal("wave_completed", waves_completed - 1)  # zero-based index
		return

	var current_segment: WaveSegment = current_wave.segments[current_segment_index]

	# Check if we've spawned all enemies in current segment
	if enemies_spawned_in_segment >= current_segment.count:
		# Move to next segment
		current_segment_index += 1
		enemies_spawned_in_segment = 0
		time_since_last_spawn = 0.0
		return

	# Check if it's time to spawn next enemy
	if time_since_last_spawn >= current_segment.spawn_interval:
		var success: bool = spawn_enemy(current_segment.enemy_scene)
		if success:
			enemies_spawned_in_segment += 1
			time_since_last_spawn = 0.0

func spawn_enemy(enemy_scene: PackedScene) -> bool:
	if enemy_scene == null:
		push_error("EnemySpawner: Attempted to spawn null enemy scene!")
		return false

	#	if not IS_INSTANCE_VALID(enemy_scene):
	#		push_error("EnemySpawner: Invalid enemy scene reference!")
	#		return false

	var enemy_instance: Node = enemy_scene.instantiate()

	if enemy_instance == null:
		push_error("EnemySpawner: Failed to instantiate enemy scene!")
		return false

	# Calculate spawn position with random offset
	var spawn_offset: Vector2 = Vector2(
		randf() * spawn_area_size.x - spawn_area_size.x / 2,
		randf() * spawn_area_size.y - spawn_area_size.y / 2
	)
	var final_position: Vector2 = spawn_position + spawn_offset

	# Set position (works for both Node2D and Node3D)
	if enemy_instance is Node2D:
		enemy_instance.position = final_position
	elif enemy_instance is Node3D:
		enemy_instance.global_transform.origin = Vector3(final_position.x, 0, final_position.y)
	else:
		# Fallback for other Node types
		enemy_instance.position = final_position

	# Add to scene tree (as sibling to spawner)
	var parent_node: Node = get_parent()
	if parent_node == null:
		push_error("EnemySpawner: No parent node to add enemy to!")
		enemy_instance.queue_free()
		return false

	parent_node.add_child(enemy_instance)

	# Optional: Connect to enemy's death/destroyed signal if it exists
	# This allows tracking when all enemies in wave are defeated
	if enemy_instance.has_method("die"):
		enemy_instance.connect("die", Callable(self, "_on_enemy_died").bind(enemy_instance))
	elif enemy_instance.has_signal("destroyed"):
		enemy_instance.connect("destroyed", Callable(self, "_on_enemy_destroyed").bind(enemy_instance))

	print("Spawned %s at %s" % [enemy_scene.get_name(), final_position])
	return true

# Optional helper methods for wave completion tracking
func _on_enemy_died(enemy: Node) -> void:
	# You could implement enemy counting here if needed
	pass

func _on_enemy_destroyed(enemy: Node) -> void:
	# You could implement enemy counting here if needed
	pass

# Getter methods for external monitoring
func get_waves_completed() -> int:
	return waves_completed

func get_is_spawning() -> bool:
	return is_spawning_active

func get_current_wave_index() -> int:
	return waves_completed  # Returns index of last completed wave, or current if still spawning

func get_remaining_enemies_in_wave() -> int:
	if not is_spawning_active or current_wave == null:
		return 0

	var remaining: int = 0

	# Add remaining enemies in current segment
	remaining += (current_wave.segments[current_segment_index].count - enemies_spawned_in_segment)

	# Add all enemies in remaining segments
	for i in range(current_segment_index + 1, current_wave.segments.size()):
		remaining += current_wave.segments[i].count

	return remaining
