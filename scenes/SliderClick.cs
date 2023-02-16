using Godot;
using System;

public class SliderClick : AudioStreamPlayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }
    public void _on_HSlider_value_changed(float value)
    {
        Play();
    }
    public void _on_AudioStreamPlayer_finished()
    {
        Stop();
    }
}
