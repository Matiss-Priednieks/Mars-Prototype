using Godot;
using System;

public class MarsMesh : CSGSphere
{
    Vector2 mouseDelta;
    bool mouseEntered = false;
    public override void _Ready()
    {

    }

    private void _on_Area_area_entered(object area)
    {

    }

    private void _on_Area_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            mouseDelta = mouseMotion.Relative;
        }
    }

}
