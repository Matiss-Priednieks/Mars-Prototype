using Godot;
using System;

public class MissionGenerator : Spatial
{
    RandomNumberGenerator rng = new RandomNumberGenerator();
    [Export] int NumOfTotalMissions = 12;
    float TotalResourceMissions = 0;
    float TotalResearchMissions = 0;
    float TotalRecoveryMissions = 0;
    float[] MissionRatios = { 0.5f, 0.33f, 0.17f }; // mission spawn chances: 50% resources, 33% research, 17% recovery. This is so mission amount can be changed dynamically.

    Vector3[] MissionLocations;

    PackedScene MissionScene;
    Spatial PlanetMars;


    public override void _Ready()
    {
        PlanetMars = GetParent().GetNode<Spatial>("Mars");
        MissionScene = (PackedScene)ResourceLoader.Load("res://scenes/MissionStation.tscn");
        MissionLocations = new Vector3[NumOfTotalMissions];
        TotalResourceMissions = (NumOfTotalMissions * MissionRatios[0]);
        TotalResearchMissions = (NumOfTotalMissions * MissionRatios[1]);
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
            MissionLocations[i] = MissionLocations[i] * 39;


            var newMission = MissionScene.Instance<Spatial>();
            newMission.Translation = MissionLocations[i];
            AddChild(newMission);
            newMission.LookAt((GlobalTranslation - Translation).Normalized() * -1, Vector3.Down);
        }

    }
    public override void _PhysicsProcess(float delta)
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
