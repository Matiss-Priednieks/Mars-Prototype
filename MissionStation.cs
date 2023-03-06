using Godot;
using System;


public class MissionStation : StaticBody
{
    RandomNumberGenerator rng = new RandomNumberGenerator();
    [Signal] public delegate void Interacted(InputEvent inputEvent, Vector3 position, string missionList, string missionType, Vector3 Normal);
    string[] MissionList = { "Research", "Resource", "Recovery" };
    string[] ResourceType = { "H2O", "SCRAP" };
    string SelectedMission;
    string SelectedMissionType;
    bool IsResource = false;


    public override void _Ready()
    {
        this.Connect("Interacted", GetNode<Node>("/root/Interactions"), "InteractionHandler");

        rng.Randomize();
        SelectedMission = MissionList[rng.RandiRange(0, 2)];

        if (SelectedMission == "Resource")
        {
            SelectedMissionType = ResourceType[rng.RandiRange(0, 1)];
            IsResource = true;
        }
        else
        {
            IsResource = false;
        }


        GetNode<Label3D>("Label3D").Text = SelectedMission;

        GetNode<Sprite3D>("Sprite3D").Texture = MissionTexGen(SelectedMission, SelectedMissionType, IsResource);
    }


    public void _on_MissionStation_input_event(Node camera, InputEvent inputEvent, Vector3 position, Vector3 normal, int shape_idx)
    {
        EmitSignal("Interacted", inputEvent, position, SelectedMission, SelectedMissionType, normal);
    }

    public Texture MissionTexGen(string missionTitle, string missionType, bool isResource)
    {
        Texture tex;
        if (IsResource)
        {
            tex = GD.Load<Texture>("assets/" + missionTitle + "_" + missionType + "_Icon" + ".png");
        }
        else
        {
            tex = GD.Load<Texture>("assets/" + missionTitle + "_Icon" + ".png");
        }

        return tex;
    }

    public PackedScene MissionModelGen(string missionTitle, string missionType, bool isResource)
    {
        PackedScene model;
        if (IsResource)
        {
            model = ResourceLoader.Load<PackedScene>("assets/" + missionTitle + "_" + missionType + "_Model" + ".png");
        }
        else
        {
            model = ResourceLoader.Load<PackedScene>("assets/" + missionTitle + "_Model" + ".png");
        }

        return model;
    }
}
