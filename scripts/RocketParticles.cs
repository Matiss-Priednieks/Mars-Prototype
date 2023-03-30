using Godot;
using System;

public class RocketParticles : Particles
{
    public override void _Process(float delta)
    {
        Translation += new Vector3(0, 5, 0);
    }
}
