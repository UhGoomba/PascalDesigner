using Godot;
using System;

public partial class SelectionFinder : Node3D
{
	private const float RayLength = 1000.0f;
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Left)
		{
			var camera = GetParent<Camera3D>();
			var spaceState = GetWorld3D().DirectSpaceState;
			var from = camera.ProjectRayOrigin(eventMouseButton.Position);
			var to = from + camera.ProjectRayNormal(eventMouseButton.Position) * RayLength;
			var query = PhysicsRayQueryParameters3D.Create(from, to);
			var result = spaceState.IntersectRay(query);
			
			if (result.Count > 0)
			{
				TextDisplaySelectionCollision hit = (TextDisplaySelectionCollision)result["collider"];
				
				hit.Select(Input.IsActionPressed("Multi Select"));
			}
		}
	}
}
