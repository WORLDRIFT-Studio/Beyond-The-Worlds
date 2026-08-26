extends Node
class_name WaveGenerator

@export var enemy_pool: Array[EnemyData] = []

func generate_wave(wave_index: int) -> WaveData:
	var budget: int = 50 + (wave_index * 30)
	var remaining_budget: int = budget
	
	# FIXED: Strongly typed array
	var segments: Array[WaveSegment] = []

	# FIXED: Strongly typed array
	var affordable_pool: Array[EnemyData] = []
	for enemy in enemy_pool:
		if enemy.cost <= budget:
			affordable_pool.append(enemy)

	# If no affordable enemies, return empty wave
	if affordable_pool.size() == 0:
		var wave_data: WaveData = WaveData.new()
		wave_data.segments = segments
		return wave_data

	# Continue generating segments until we can't afford any more enemies
	while remaining_budget > 0:
		
		# FIXED: Strongly typed array
		var currently_affordable: Array[EnemyData] = []
		for enemy in affordable_pool:
			if enemy.cost <= remaining_budget:
				currently_affordable.append(enemy)

		# Break if no enemies can be afforded with remaining budget
		if currently_affordable.size() == 0:
			break

		# Randomly pick an affordable enemy type
		var selected_enemy: EnemyData = currently_affordable[randi() % currently_affordable.size()]

		# Determine a random count for that enemy
		var max_count: int = remaining_budget / selected_enemy.cost
		var count: int = randi() % max_count + 1  # Random between 1 and max_count

		# Ensure count * cost doesn't exceed remaining_budget (shouldn't happen with above, but being safe)
		while count * selected_enemy.cost > remaining_budget:
			count -= 1

		# Create wave segment
		var segment: WaveSegment = WaveSegment.new()
		segment.enemy_scene = selected_enemy.enemy_scene
		segment.count = count
		# Set spawn interval based on count (more enemies = shorter interval)
		segment.spawn_interval = 0.5 + (randf() * 1.5)  # Random between 0.5 and 2.0

		segments.append(segment)
		remaining_budget -= count * selected_enemy.cost

	var wave_data: WaveData = WaveData.new()
	wave_data.segments = segments
	return wave_data
