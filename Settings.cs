using Godot;
using System;

public class Settings : VBoxContainer
{
    bool Fullscreen = false;
    bool QHD = false;
    Button ResButton;
    Button FsButton;
    public override void _Ready()
    {
        ResButton = GetNode<Button>("MarginContainer/Resolution");
        FsButton = GetNode<Button>("MarginContainer2/Fullscreen");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!QHD)
        {
            OS.WindowSize = new Vector2(1920, 1080);
            ResButton.Text = "1920x1080";
        }
        else
        {
            OS.WindowSize = new Vector2(2560, 1440);
            ResButton.Text = "2560x1440";
        }
        if (!Fullscreen)
        {
            OS.WindowFullscreen = true;
            FsButton.Text = "Fullscreen";
        }
        else
        {
            OS.WindowFullscreen = false;
            FsButton.Text = "Windowed";
        }
    }
    public void _on_Fullscreen_pressed()
    {
        Fullscreen = !Fullscreen;
    }
    public void _on_Resolution_pressed()
    {
        QHD = !QHD;
    }
}
