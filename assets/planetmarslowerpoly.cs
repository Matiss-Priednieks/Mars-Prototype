using Godot;
using System;

public class planetmarslowerpoly : Spatial
{
    Vector2 mouseDelta;
    bool mouseEntered = false;

    private void _on_ClickArea_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            mouseDelta = mouseMotion.Relative;
        }
    }

}
