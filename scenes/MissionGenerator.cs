using Godot;
using System;

public class MissionGenerator : Spatial
{
    [Export] int NumOfTotalMissions = 12;
    RandomNumberGenerator rng = new RandomNumberGenerator();
    PackedScene MissionScene;
    System.Collections.Generic.List<MissionStation> MissionList;
    CanvasLayer UI;

    float TotalScrapMissions = 0;
    float TotalH2OMissions = 0;
    float TotalResearchMissions = 0;
    float TotalRecoveryMissions = 0;
    float[] MissionRatios = { 0.33f, 0.25f, 0.17f }; // mission spawn chances: 50% resources(25% per type), 33% research, 17% recovery. This is so mission amount can be changed dynamically.

    Vector3[] MissionLocations;
    Spatial PlanetMars;
    Vector2 ResourceMissions = Vector2.Zero; // x is H2O and y is Scrap

    public override void _Ready()
    {
        UI = GetParent().GetNode<CanvasLayer>("GUI");
        PlanetMars = GetParent().GetNode<Spatial>("Mars");
        MissionScene = (PackedScene)ResourceLoader.Load("res://scenes/MissionStation.tscn");
        string[] MissionTypeList = { "Research", "Resource", "Recovery" };
        string[] ResourceType = { "H2O", "SCRAP" };

        MissionLocations = new Vector3[NumOfTotalMissions];

        MissionList = new System.Collections.Generic.List<MissionStation>();

        // MissionList = new MissionStation[NumOfTotalMissions];

        TotalResearchMissions = (NumOfTotalMissions * MissionRatios[0]);
        TotalScrapMissions = (NumOfTotalMissions * MissionRatios[1]);
        TotalH2OMissions = (NumOfTotalMissions * MissionRatios[1]);
        TotalRecoveryMissions = (NumOfTotalMissions * MissionRatios[2]);


        for (int i = 0; i < NumOfTotalMissions; i++)
        {
            rng.Randomize();
            MissionLocations[i] = new Vector3(rng.RandiRange(-1000, 1000), rng.RandiRange(-1000, 1000), rng.RandiRange(-1000, 1000)).Normalized();

            if (i != 0)
            {
                if (MissionLocations[i].DistanceTo(MissionLocations[i - 1]) <= 0.2f)
                {
                    MissionLocations[i] = new Vector3(rng.RandiRange(-1000, 1000), rng.RandiRange(-1000, 1000), rng.RandiRange(-1000, 1000)).Normalized();
                }
            }
            MissionLocations[i] = MissionLocations[i] * 38.8f;


            var newMission = MissionScene.Instance<MissionStation>();



            newMission.Translation = MissionLocations[i];
            // MissionList[i] = newMission;
            MissionList.Add(newMission);
            AddChild(newMission);
            newMission.LookAt((GlobalTranslation - Translation).Normalized() * -1, Vector3.Down);
        }


        int researchQuota = 0;
        int h2oQuota = 0;
        int scrapQuota = 0;
        int recoveryQuota = 0;
        for (int i = 0; i < NumOfTotalMissions; i++)
        {
            if (researchQuota < TotalResearchMissions)
            {
                var isResource = false;
                MissionList[i].SetMission(MissionTypeList[0], "", isResource);
                researchQuota++;
            }
            else if (h2oQuota < TotalH2OMissions)
            {
                var isResource = true;
                MissionList[i].SetMission(MissionTypeList[1], ResourceType[0], isResource);
                h2oQuota++;
            }
            else if (scrapQuota < TotalScrapMissions)
            {
                var isResource = true;
                MissionList[i].SetMission(MissionTypeList[1], ResourceType[1], isResource);
                scrapQuota++;
            }
            else if (recoveryQuota < TotalRecoveryMissions)
            {
                var isResource = false;
                MissionList[i].SetMission(MissionTypeList[2], "", isResource);
                recoveryQuota++;
            }
        }
        PopulateMissionList();

    }
    public override void _PhysicsProcess(float delta)
    {

    }

    public void MissionCompleted(string missionName, string missionID)
    {
        for (int i = 0; i < MissionList.Count; i++)
        {
            var missionElements = (Label)UI.GetNode<VBoxContainer>("RightUI").GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("MissionsList").GetChild(i);
            GD.Print(missionElements.Text);
            if (missionID.Equals("M" + MissionList[i].Translation.Length() + MissionList[i].Rotation.Length()) && missionName.Contains(MissionList[i].GetMissionName()))
            {
                // GD.Print("removal successful");
                var removedMission = MissionList[i];
                if (missionElements.Text.Contains(MissionList[i].GetMissionName()))
                {
                    missionElements.QueueFree();
                }
                MissionList.RemoveAt(i);
                removedMission.QueueFree();
            }
        }
    }


    public void PopulateMissionList()
    {
        for (int i = 0; i < MissionList.Count; i++)
        {
            var missionLabel = (PackedScene)ResourceLoader.Load("res://scenes/MissionUIItem.tscn");
            var missionLabelInstance = missionLabel.Instance<Label>();
            missionLabelInstance.Text = MissionList[i].GetMissionName();
            missionLabelInstance.Align = Label.AlignEnum.Right;
            UI.GetNode<VBoxContainer>("RightUI").GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("MissionsList").AddChild(missionLabelInstance);
        }
    }


}
