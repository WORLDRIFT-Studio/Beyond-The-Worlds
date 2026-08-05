using Godot;

namespace BeyondTheWorlds.levels.level_test_2;
public partial class Tiles : Node3D
{
	[Export] private PackedScene TileScene { get; set; }
	[Export] private int _widthArena = 10;
	[Export] private int _lengthArena = 10;
	[Export] private float _spacing = 1.0f;
	
	public override void _Ready()
	{
		if (TileScene == null) {return;}
		
		float rowSpacing = _spacing * (Mathf.Sqrt(3.0f) / 2.0f);
		
		for (int x = 0; x < _widthArena; x++)
		{
			for (int z = 0; z < _lengthArena; z++)
			{
				Node3D tile = TileScene.Instantiate<Node3D>();
				AddChild(tile);
				
				float posX = x * _spacing;

				if (z % 2 != 0)
				{
					posX += _spacing / 2.0f;
				}

				float posZ = z * rowSpacing;
				
				tile.Position = new Vector3(posX, 0, posZ);
			}
		}
	}
}
