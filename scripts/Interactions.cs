using Godot;
using System;

public class Interactions : Node
{

    [Signal] public delegate void InteractionConnector(InputEvent inputEvent, Vector3 position, string selectedMission, string selectedMissionType, string missionID);
    bool SignalConnected = false;
    string MainGameScene = "GameScene";

    public void InteractionHandler(InputEvent inputEvent, Vector3 position, string missionList, string missionType, Vector3 normal, string missionID)
    {
        EmitSignal("InteractionConnector", inputEvent, position, missionList, missionType, normal, missionID);
    }

    public override void _Process(float delta)
    {
        if (!SignalConnected && GetTree().Root.GetChild(1).Name == MainGameScene)
        {
            this.Connect("InteractionConnector", GetNode<KinematicBody>("../GameScene/Player"), "AttemptMission");
            SignalConnected = true;
        }
    }
}