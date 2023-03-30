using Godot;
using System;
using System.Net;
using System.IO;
public class PlanetScript : Spatial
{
    Camera PlayerCam, MainCam;
    public override void _Ready()
    {
        PlayerCam = GetNode<Camera>("Player/PlayerCamera");
        MainCam = GetNode<Camera>("CameraPivot/MainCam");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustReleased("switch_view"))
        {
            PlayerCam.Current = !PlayerCam.Current;
        }
    }

}
