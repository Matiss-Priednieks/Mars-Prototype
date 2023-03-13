using Godot;
using System;

public class LabelContainer : Spatial
{
    Camera camera;
    public override void _Ready()
    {
        camera = GetParent().GetParent().GetParent().GetNode<Spatial>("CameraPivot").GetNode<Camera>("MainCam");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        // LookAt(camera.Translation, Vector3.Up);
    }
}
