using Godot;
using System;
using System.Net;
using System.IO;
public class PlanetScript : Spatial
{
    Camera PlayerCam;
    public override void _Ready()
    {
        PlayerCam = GetNode<Camera>("Player/PlayerCamera");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustReleased("switch_view"))
        {
            PlayerCam.Current = !PlayerCam.Current;
        }
    }

}
