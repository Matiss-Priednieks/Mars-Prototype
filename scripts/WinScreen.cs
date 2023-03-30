using Godot;
using System;

public class WinScreen : Control
{
    public void _on_TextureButton_pressed()
    {
        GetTree().Quit();
    }
}
