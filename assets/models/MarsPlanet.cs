using Godot;
using System;

public class MarsPlanet : Spatial
{
    Vector2 mouseDelta;
    bool mouseEntered = false;

    private void _on_StaticBody_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
    {
        if (@event is InputEventMouseMotion mouseMotion && !Input.IsActionPressed("click_to_move"))
        {
            mouseDelta = mouseMotion.Relative;
        }
    }


}
