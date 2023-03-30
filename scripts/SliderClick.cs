using Godot;
using System;

public class SliderClick : AudioStreamPlayer
{
    public void _on_HSlider_value_changed(float value)
    {
        Play();
    }
    public void _on_AudioStreamPlayer_finished()
    {
        Stop();
    }
}
