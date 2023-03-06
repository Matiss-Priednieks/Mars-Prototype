using Godot;
using System;

public class Interactions : Node
{

    [Signal] public delegate void InteractionConnector(InputEvent inputEvent, Vector3 position, string selectedMission, string selectedMissionType);

    public override void _Ready()
    {
        this.Connect("InteractionConnector", GetNode<KinematicBody>("../Spatial/Player"), "AttemptMission");
    }

    public void InteractionHandler(InputEvent inputEvent, Vector3 position, string missionList, string missionType, Vector3 normal)
    {
        EmitSignal("InteractionConnector", inputEvent, position, missionList, missionType, normal);
    }

}