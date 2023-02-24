using Godot;
using System;

public class MissionItem : Resource
{
    public float TimeComplexity = 10.0f;
    public string MissionTitle = "DefaultTitle";
    [Export(PropertyHint.Enum, "Research, Resources")] public string MissionType;
    RandomNumberGenerator rng = new RandomNumberGenerator();

    public void SetTimeComplexity(float time)
    {
        TimeComplexity = time;
    }

    public void RandomTimeComplexity(float min, float max)
    {
        if (min <= 0)
        {
            min = 0.1f;
            GD.Print("MissionItem.cs: Min cannot be less than or equal to 0, defaulting to 0.1");
        }
        if (max <= 0)
        {
            max = 0.1f;
            GD.Print("MissionItem.cs: Max cannot be less than or equal to 0, defaulting to 0.1");
        }
        TimeComplexity = rng.RandfRange(min, max);
    }

    public void SetMissionTitle(string title)
    {
        MissionTitle = title;
    }
}
