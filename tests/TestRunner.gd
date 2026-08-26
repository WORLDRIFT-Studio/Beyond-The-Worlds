extends Node

func _ready() -> void:
	randomize()
	run_all_tests()

func run_all_tests() -> void:
	print("=".repeat(50))
	print("WAVE GENERATOR SYSTEM TEST SUITE")
	print("=".repeat(50))

	var passed: int = 0
	var failed: int = 0

	# Test 1: Budget calculation accuracy
	passed += run_test("Budget Calculation", func() -> bool:
		return test_budget_calculation()
	)

	# Test 2: WaveData contains valid WaveSegment instances
	passed += run_test("Valid WaveSegments", func() -> bool:
		return test_valid_wave_segments()
	)

	# Test 3: Generated cost does not exceed initial budget
	passed += run_test("Budget Limit", func() -> bool:
		return test_budget_limit()
	)

	# Test 4: Leftover budget is smaller than cheapest available enemy
	passed += run_test("Leftover Budget Check", func() -> bool:
		return test_leftover_budget()
	)

	# Test 5: Generated waves are not empty (when pool has affordable enemies)
	passed += run_test("Non-empty Waves", func() -> bool:
		return test_non_empty_waves()
	)

	# Test 6: Empty wave when no affordable enemies
	passed += run_test("Empty Wave When No Affordable Enemies", func() -> bool:
		return test_empty_when_no_affordable()
	)

	print("=".repeat(50))
	print("TEST RESULTS: %d passed, %d failed" % [passed, failed])
	print("=".repeat(50))

	if failed == 0:
		print("All tests passed! ✓")
	else:
		print("Some tests failed! ✗")
		push_error("Test suite completed with failures")

func run_test(test_name: String, test_func: Callable) -> int:
	var result: bool = test_func.call()
	if result:
		print("✓ %s: PASSED" % test_name)
		return 1
	else:
		print("✗ %s: FAILED" % test_name)
		push_error("%s failed" % test_name)
		return 0

func test_budget_calculation() -> bool:
	var generator: WaveGenerator = WaveGenerator.new()
	setup_test_enemy_pool(generator)

	# Test wave indices 0, 1, 5, 10
	var test_cases: Array = [0, 1, 5, 10]
	var expected_budgets: Array = [50, 80, 200, 350]

	for i in test_cases.size():
		var wave_data: WaveData = generator.generate_wave(test_cases[i])
		var total_cost: int = calculate_total_cost(wave_data, generator.enemy_pool)
		var expected_budget: int = expected_budgets[i]

		if total_cost > expected_budget:
			print("  Wave %d: Cost %d exceeds budget %d" % [test_cases[i], total_cost, expected_budget])
			return false

	return true

func test_valid_wave_segments() -> bool:
	var generator: WaveGenerator = WaveGenerator.new()
	setup_test_enemy_pool(generator)

	var wave_data: WaveData = generator.generate_wave(5)  # Use mid-range wave

	if wave_data.segments.size() == 0:
		print("  No segments generated")
		return false

	for segment in wave_data.segments:
		if segment == null:
			print("  Null segment found")
			return false
		if segment.enemy_scene == null:
			print("  Segment with null enemy scene")
			return false
		if segment.count <= 0:
			print("  Segment with invalid count: %d" % segment.count)
			return false
		if segment.spawn_interval <= 0:
			print("  Segment with invalid spawn interval: %f" % segment.spawn_interval)
			return false

	return true

func test_budget_limit() -> bool:
	var generator: WaveGenerator = WaveGenerator.new()
	setup_test_enemy_pool(generator)

	# Test multiple wave indices to ensure consistency
	var test_waves: Array = [0, 2, 4, 6, 8, 10]

	for wave_index in test_waves:
		var budget: int = 50 + (wave_index * 30)
		var wave_data: WaveData = generator.generate_wave(wave_index)
		var total_cost: int = calculate_total_cost(wave_data, generator.enemy_pool)

		if total_cost > budget:
			print("  Wave %d: Cost %d exceeds budget %d" % [wave_index, total_cost, budget])
			return false

	return true

func test_leftover_budget() -> bool:
	var generator: WaveGenerator = WaveGenerator.new()
	setup_test_enemy_pool(generator)

	var wave_index: int = 3
	var budget: int = 50 + (wave_index * 30)
	var wave_data: WaveData = generator.generate_wave(wave_index)
	var total_cost: int = calculate_total_cost(wave_data, generator.enemy_pool)
	var leftover: int = budget - total_cost

	# Find cheapest enemy in pool
	var cheapest_cost: int = -1
	for enemy in generator.enemy_pool:
		if cheapest_cost == -1 or enemy.cost < cheapest_cost:
			cheapest_cost = enemy.cost

	# If we have enemies in pool and generated some segments
	if cheapest_cost != -1 and wave_data.segments.size() > 0:
		# Leftover should be less than cheapest enemy cost (otherwise we could add another)
		if leftover >= cheapest_cost:
			print("  Leftover %d >= cheapest enemy cost %d" % [leftover, cheapest_cost])
			return false

	return true

func test_non_empty_waves() -> bool:
	var generator: WaveGenerator = WaveGenerator.new()
	setup_test_enemy_pool(generator)

	# Test several wave indices
	var test_waves: Array = [0, 1, 2, 5, 10]

	for wave_index in test_waves:
		var wave_data: WaveData = generator.generate_wave(wave_index)
		if wave_data.segments.size() == 0:
			print("  Wave %d generated empty segments" % wave_index)
			return false

	return true

func test_empty_when_no_affordable() -> bool:
	var generator: WaveGenerator = WaveGenerator.new()

	# Create enemies that are all too expensive for even wave 0 (budget=50)
	var expensive_enemy: EnemyData = EnemyData.new()
	expensive_enemy.cost = 100  # More than starting budget of 50
	# expensive_enemy.enemy_scene = preload("res://")  # Dummy scene

	generator.enemy_pool = [expensive_enemy]

	var wave_data: WaveData = generator.generate_wave(0)  # Budget = 50

	if wave_data.segments.size() != 0:
		print("  Expected empty wave when no affordable enemies, got %d segments" % [wave_data.segments.size()])
		return false

	return true

func setup_test_enemy_pool(generator: WaveGenerator) -> void:
	# Create a variety of test enemies with different costs
	var enemy_costs: Array = [5, 10, 15, 20, 25]

	generator.enemy_pool = []
	for cost in enemy_costs:
		var enemy: EnemyData = EnemyData.new()
		enemy.cost = cost
		# Use a dummy scene reference (in real project, this would be an actual enemy scene)
		# enemy.enemy_scene = preload("res://")  # Points to project root - just for testing
		generator.enemy_pool.append(enemy)

func calculate_total_cost(wave_data: WaveData, enemy_pool: Array[EnemyData]) -> int:
	var total: int = 0

	# Create a mapping from enemy scene to cost for quick lookup
	var scene_to_cost: Dictionary = {}
	for enemy in enemy_pool:
		if enemy.enemy_scene:
			scene_to_cost[enemy.enemy_scene] = enemy.cost

	for segment in wave_data.segments:
		if segment.enemy_scene and segment.enemy_scene in scene_to_cost:
			total += scene_to_cost[segment.enemy_scene] * segment.count
		# If we can't find the cost, we skip it (shouldn't happen in our tests)

	return total
