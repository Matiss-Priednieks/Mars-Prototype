using Godot;
using System;

public class PlayerCam : Camera
{
    public override void _Process(float delta)
    {
        LookAt(GetParent<KinematicBody>().Translation, Vector3.Up);
    }
}
