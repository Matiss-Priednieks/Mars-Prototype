using Godot;
using System;

public class ProgressTracker : Node
{
    Vector3[] PlayerPosition = new Vector3[2];

    double Fuel;
    int H2O, Scrap, ResearchPoints, Recovery;
    bool ClickMoving, Selected, isMissionStarted, MissionClick, ResearchComplete = false;

    int timeScale;
}
