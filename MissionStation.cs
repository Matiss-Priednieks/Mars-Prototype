using Godot;
using System;

public class MissionStation : StaticBody
{
    [Export] MissionResource MissionList;

    public override void _Ready()
    {
        GetNode<Label3D>("Label3D").Text = MissionList.Missions[0].MissionTitle;
    }
}
