using Godot;
using System;

public class MissionProgressBar : ProgressBar
{
    public override void _Process(float delta)
    {
        if (Value == 0)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
}
