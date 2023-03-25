using Godot;
using System;

public class Rocket : Spatial
{
    Vector3 Direction;
    Spatial Planet;
    PackedScene WinScreen;
    int Speed = 10;
    public override void _Ready()
    {
        Planet = GetNode<Spatial>("../Mars");
        WinScreen = (PackedScene)ResourceLoader.Load("res://scenes/WinScreen.tscn");
        Direction = (GlobalTranslation - Planet.Translation).Normalized();

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        Translation += Direction * Speed * delta;
        LookAt(Transform.origin + (Direction * Speed * delta), Vector3.Up);
    }

    public void _on_Timer_timeout()
    {
        GetTree().ChangeSceneTo(WinScreen);
    }
}
