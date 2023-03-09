using Godot;
using System;

public class MainMenu : Spatial
{
    public void _on_Button_pressed()
    {
        GetTree().ChangeScene("scenes/Planet.tscn");
    }
}
