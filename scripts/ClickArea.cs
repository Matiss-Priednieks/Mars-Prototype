using Godot;
using System;

public class ClickArea : Area
{
    public void _on_ClickArea_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
    {
        if (@event is InputEventMouseMotion mouseMotion && !Input.IsActionPressed("click_to_move"))
        {

        }
    }
}
