using Godot;
using System;

public class Interactions : Node
{

    [Signal] public delegate void InteractionConnector(InputEvent inputEvent, Vector3 position, string selectedMission, string selectedMissionType);
    bool SignalConnected = false;
    string MainGameScene = "GameScene";

    public override void _Ready()
    {

    }

    public void InteractionHandler(InputEvent inputEvent, Vector3 position, string missionList, string missionType, Vector3 normal)
    {
        EmitSignal("InteractionConnector", inputEvent, position, missionList, missionType, normal);
    }
    public override void _Process(float delta)
    {

        if (!SignalConnected && GetTree().Root.GetChild(2).Name == MainGameScene)
        {
            GD.Print("Test");
            this.Connect("InteractionConnector", GetNode<KinematicBody>("../GameScene/Player"), "AttemptMission");
            SignalConnected = true;
        }
    }
}