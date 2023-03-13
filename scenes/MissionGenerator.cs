using Godot;
using System;

public class MissionGenerator : Spatial
{
    RandomNumberGenerator rng = new RandomNumberGenerator();
    [Export] int NumOfTotalMissions = 12;
    float TotalScrapMissions = 0;
    float TotalH2OMissions = 0;
    float TotalResearchMissions = 0;
    float TotalRecoveryMissions = 0;
    float[] MissionRatios = { 0.33f, 0.25f, 0.17f }; // mission spawn chances: 50% resources(25% per type), 33% research, 17% recovery. This is so mission amount can be changed dynamically.

    Vector3[] MissionLocations;

    MissionStation[] MissionList;

    PackedScene MissionScene;
    Spatial PlanetMars;
    Vector2 ResourceMissions = Vector2.Zero; // x is H2O and y is Scrap

    public override void _Ready()
    {
        PlanetMars = GetParent().GetNode<Spatial>("Mars");
        MissionScene = (PackedScene)ResourceLoader.Load("res://scenes/MissionStation.tscn");
        MissionLocations = new Vector3[NumOfTotalMissions];
        MissionList = new MissionStation[NumOfTotalMissions];
        TotalResearchMissions = (NumOfTotalMissions * MissionRatios[0]);
        TotalScrapMissions = (NumOfTotalMissions * MissionRatios[1]);
        TotalH2OMissions = (NumOfTotalMissions * MissionRatios[1]);
        TotalRecoveryMissions = (NumOfTotalMissions * MissionRatios[2]);
        // GD.Print(TotalRecoveryMissions + TotalResearchMissions + TotalResourceMissions);


        for (int i = 0; i < NumOfTotalMissions; i++)
        {
            rng.Randomize();
            MissionLocations[i] = new Vector3(rng.RandiRange(-1000, 1000), rng.RandiRange(-1000, 1000), rng.RandiRange(-1000, 1000)).Normalized();

            GD.Print(MissionLocations[i]);
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
            MissionList[i] = newMission;
            AddChild(newMission);
            newMission.LookAt((GlobalTranslation - Translation).Normalized() * -1, Vector3.Down);
        }

    }
    public override void _PhysicsProcess(float delta)
    {

    }

    public void MissionCompleted(string missionName, string missionID)
    {
        GD.Print(missionID + "\n");
        for (int i = 0; i < MissionList.Length; i++)
        {
            GD.Print("M" + MissionList[i].Translation.Length());
            if (missionID.Equals("M" + MissionList[i].Translation.Length()))
            {
                MissionList[i].QueueFree();
                MissionList[i] = null;

                //TODO: rewrite this system. Currently broken and likely needs a full rewrite to work properly.
            }
        }
    }
    public void MissionStarted(string MissionName, string missionID)
    {

    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
