using Godot;
using System;

public class ClickArea : Area
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public void _on_ClickArea_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
    {
        if (@event is InputEventMouseMotion mouseMotion && !Input.IsActionPressed("click_to_move"))
        {

        }
    }
}
