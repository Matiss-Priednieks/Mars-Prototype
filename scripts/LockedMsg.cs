using Godot;
using System;

public class LockedMsg : Panel
{
    public void _on_Timer_timeout()
    {
        Hide();
    }
}
