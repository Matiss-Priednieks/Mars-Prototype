using Godot;
using System;


public class MissionStation : StaticBody
{
    RandomNumberGenerator rng = new RandomNumberGenerator();
    string[] MissionList = { "Research", "Resource", "Recovery" };

    public override void _Ready()
    {
        rng.Randomize();
        GetNode<Label3D>("Label3D").Text = MissionList[rng.RandiRange(0, 2)];
    }
}
