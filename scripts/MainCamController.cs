using Godot;
using System;

public class MainCamController : Camera
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Export(PropertyHint.Range, "0.1, 10")] float ZoomAmount = 5;
    bool MenuOpen;
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        MenuOpen = GetNode<Panel>("../../GUI/Help").Visible;
        if (!MenuOpen)
        {
            if (Input.IsActionJustReleased("zoom_in"))
            {
                if (Translation.z > 40)
                {
                    Translate(new Vector3(0, 0, -ZoomAmount));
                }

            }
            if (Input.IsActionJustReleased("zoom_out"))
            {
                if (Translation.z < 150)
                {
                    Translate(new Vector3(0, 0, ZoomAmount));
                }
            }
        }
    }
}
