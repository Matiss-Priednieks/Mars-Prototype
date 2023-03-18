using Godot;
using System;


public class MissionStation : StaticBody
{
    RandomNumberGenerator rng = new RandomNumberGenerator();
    [Signal] public delegate void Interacted(InputEvent inputEvent, Vector3 position, string missionList, string missionType, Vector3 Normal, string MissionID);

    string SelectedMission;
    string SelectedMissionType;

    public override void _Ready()
    {
        this.Connect("Interacted", GetNode<Node>("/root/Interactions"), "InteractionHandler");

        rng.Randomize();

    }


    public void _on_MissionStation_input_event(Node camera, InputEvent inputEvent, Vector3 position, Vector3 normal, int shape_idx)
    {
        var missionID = "M" + Translation.Length() + Rotation.Length();
        EmitSignal("Interacted", inputEvent, position, SelectedMission, SelectedMissionType, normal, missionID);
    }

    public Texture MissionTexGen(string missionTitle, string missionType, bool isResource)
    {
        Texture tex;
        if (isResource)
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
        if (isResource)
        {
            model = ResourceLoader.Load<PackedScene>("assets/" + missionTitle + "_" + missionType + "_Model" + ".png");
        }
        else
        {
            model = ResourceLoader.Load<PackedScene>("assets/" + missionTitle + "_Model" + ".png");
        }
        return model;
    }


    public void SetMission(string missionType, string missionResourceType, bool isResource)
    {
        SelectedMission = missionType;
        SelectedMissionType = missionResourceType;
        GetNode<Label3D>("LabelContainer/Label3D").Text = SelectedMission;

        GetNode<Sprite3D>("LabelContainer/Sprite3D").Texture = MissionTexGen(SelectedMission, SelectedMissionType, isResource);
    }

    public string GetMissionType()
    {
        return SelectedMissionType;
    }
    public string GetMission()
    {
        return SelectedMission;
    }
    public string GetMissionName()
    {
        if (SelectedMission == "Resource")
        {
            return SelectedMission + ": " + SelectedMissionType;
        }
        else
        {
            return SelectedMission;
        }
    }
}
