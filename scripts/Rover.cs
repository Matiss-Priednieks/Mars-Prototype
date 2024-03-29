using Godot;
using System;

public class Rover : KinematicBody
{
    //signals and exported variables
    [Signal]
    public delegate void MissionComplete(string missionName, string missionID);
    [Signal]
    public delegate void MissionStarted(string missionName, string missionID);
    [Signal] public delegate void MissionRewards(string rewardType, int rewardAmount);
    [Export(PropertyHint.Range, "1, 50")] int timeScale = 1;


    //object references
    Spatial PlayerModel, PlanetMars;
    Material RoverMat;
    Label3D MissionLabel;
    PackedScene Rocket;
    Panel LockedMsg;

    Label FuelLabel, TemperatureLabel, PressureLabel, WindLabel;
    ProgressBar MissionProgressBar;
    AudioStreamPlayer3D RoverMovementSound, MissionCompleteSound;
    Timer MissionTimer;

    // floats and bools
    float MoveSpeed = 0.1f;
    bool ClickMoving, Selected, isMissionStarted, MissionClick, ResearchComplete = false;

    int H2O, Scrap, ResearchPoints, Recovery = 0;

    double Fuel;

    //vectors
    Vector3 LocalGravity, targetLocation = Vector3.Zero;
    Vector3 targetNormal = Vector3.One;

    // strings
    string tempMissionTitle, tempMissionType, CurrentMission, MissionID;

    int MissionProgress;

    public override void _Ready()
    {
        LockedMsg = GetNode<Panel>("../GUI/LockedMsg");
        Rocket = (PackedScene)ResourceLoader.Load("res://scenes/Rocket.tscn");
        this.Connect("MissionComplete", GetNode<Spatial>("../MissionGenerator"), "MissionCompleted");
        this.Connect("MissionRewards", GetNode<CanvasLayer>("../GUI"), "UpdateResources");

        //object references

        PlayerModel = GetNode<Spatial>("Rover");
        MissionLabel = GetNode<Label3D>("Mission");
        MissionTimer = GetNode<Timer>("MissionTimer");
        FuelLabel = GetNode<Label>("../GUI/ExtraInfo/MarginContainer/VBoxContainer/Fuel");
        MissionProgressBar = GetNode<ProgressBar>("../GUI/ExtraInfo/MarginContainer/VBoxContainer/MissionProgressBar");
        PlanetMars = GetParent().GetNode<Spatial>("Mars");

        RoverMovementSound = GetNode<AudioStreamPlayer3D>("RoverMovement");
        MissionCompleteSound = GetNode<AudioStreamPlayer3D>("MissionCompletedSound");
        RoverMat = GetNode<Spatial>("Rover").GetNode<MeshInstance>("Cylinder").GetActiveMaterial(0).NextPass;

        //other
        LocalGravity = new Vector3(PlanetMars.GlobalTransform.origin - Transform.basis.y);
    }

    public override void _Process(float delta)
    {
        if (Selected) { RoverMat.Set("shader_param/grow", 0.02); } else { RoverMat.Set("shader_param/grow", 0); }
        MissionLabel.Text = CurrentMission;

        MissionProgressBar.Value = MissionProgress;
        if (isMissionStarted)
        {
            MissionProgress = (100 - ((int)MissionTimer.TimeLeft * 20));
        }
        else
        {
            MissionProgress = 0;
        }
        Fuel = Math.Round(Fuel, 2);
        FuelLabel.Text = "Fuel: " + Math.Round(Fuel, 0).ToString();
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!IsOnFloor())
        {
            MoveAndCollide(GravityVector());
        }

        ClickToMove(targetLocation, targetNormal, MissionClick);

    }

    public Vector3 GravityVector()
    {
        return (PlanetMars.Transform.origin - Transform.origin).Normalized();
    }
    private void _on_StaticBody_input_event(Node camera, InputEvent new_event, Vector3 position, Vector3 normal, int shape_index)
    {
        if (new_event.IsActionReleased("click_to_move") && Selected && !isMissionStarted)
        {
            targetNormal = normal;
            targetLocation = position;
            ClickMoving = true;
            MissionClick = false;
            RoverMovementSound.Play();
        }
        if (new_event.IsActionReleased("mousepress") && Selected)
        {
            Selected = false;
        }
    }

    public void ClickToMove(Vector3 destination, Vector3 normal, bool onMission)
    {
        Vector3 MovementDirection;

        MovementDirection = destination - Transform.origin;
        MovementDirection = MovementDirection.Normalized();

        if (ClickMoving)
        {
            if (Fuel <= 10)
            {
                MoveAndSlide(MovementDirection * (MoveSpeed * 0.5f) * timeScale, normal);
            }
            else
            {
                MoveAndSlide(MovementDirection * MoveSpeed * timeScale, normal);
            }
            LookAt(MovementDirection, -GravityVector());

        }
        if (((destination.DistanceTo(Transform.origin) <= 0.5f && onMission) || destination.DistanceTo(Transform.origin) <= 0.1f) && ClickMoving)
        {
            ClickMoving = false;
            RoverMovementSound.Stop();
            if (onMission)
            {
                isMissionStarted = true;
                MissionChecker();
                onMission = false;
            }
        }
    }
    public void SetTimeScale(int value)
    {
        timeScale = value;
    }


    private void _on_Player_input_event(Node camera, InputEvent new_event, Vector3 position, Vector3 normal, int shape_index)
    {

        if (new_event.IsActionReleased("mousepress"))
        {
            Selected = true;
        }
    }

    public void AttemptMission(InputEvent inputEvent, Vector3 position, string selectedMission, string selectedMissionType, Vector3 normal, string missionID)
    {

        if (inputEvent.IsActionReleased("click_to_move") && Selected && !isMissionStarted)
        {
            if (selectedMission == "Recovery" && !ResearchComplete)
            {
                LockedMsg.Show();
                LockedMsg.GetNode<Timer>("Timer").Start();
                LockedMsg.GetNode<Label>("Panel/FuelError").Hide();
                LockedMsg.GetNode<Label>("Panel/ResearchError").Show();
                LockedMsg.GetNode<AudioStreamPlayer>("Error").Play();

            }
            else if (selectedMission == "Recovery" && Fuel < 20)
            {
                LockedMsg.Show();
                LockedMsg.GetNode<Timer>("Timer").Start();
                LockedMsg.GetNode<Label>("Panel/ResearchError").Hide();
                LockedMsg.GetNode<Label>("Panel/FuelError").Show();
                LockedMsg.GetNode<AudioStreamPlayer>("Error").Play();
            }
            else
            {
                targetNormal = normal;
                targetLocation = position;
                ClickMoving = true;
                RoverMovementSound.Play();
                MissionClick = true;
                tempMissionTitle = selectedMission;
                tempMissionType = selectedMissionType;
                MissionID = missionID;
            }
        }
    }


    public void MissionChecker()
    {
        if (isMissionStarted)
        {
            EmitSignal("MissionStarted", CurrentMission, MissionID);
            if (tempMissionType != null)
            {
                CurrentMission = tempMissionTitle + ": " + tempMissionType;
            }
            else
            {
                CurrentMission = tempMissionTitle;
            }
            ClickMoving = false;
            RoverMovementSound.Stop();
        }
    }
    public void _on_Player_MissionStarted(string CurrentMission, string MissionID)
    {
        MissionTimer.Start();
    }

    public void _on_MissionTimer_timeout()
    {
        EmitSignal("MissionComplete", CurrentMission, MissionID);
        if (CurrentMission == "Recovery")
        {
            Fuel -= 20;
        }
        MissionCompleteSound.Play();

        isMissionStarted = false;
        MissionClick = false;
        GetMissionReward(CurrentMission);
        tempMissionTitle = "";
        tempMissionType = "";
        CurrentMission = "";
        MissionProgress = 0;
        MissionID = "";
    }

    public void GetMissionReward(string missionName)
    {
        if (missionName.Contains("Resource"))
        {
            if (missionName.Contains("H2O"))
            {
                H2O++;
                EmitSignal("MissionRewards", "H2O", H2O);
            }
            else
            {
                Scrap++;
                EmitSignal("MissionRewards", "Scrap", Scrap);
            }
        }
        else if (missionName.Contains("Research"))
        {
            ResearchPoints++;
            EmitSignal("MissionRewards", "Research", ResearchPoints);
        }
        else if (missionName.Contains("Recovery"))
        {
            Recovery++;
            EmitSignal("MissionRewards", "Recovery", Recovery);
        }
    }

    public void SetResearchComplete()
    {
        ResearchComplete = true;
        ResearchPoints = 0;
        EmitSignal("MissionRewards", "Research", ResearchPoints);

    }
    public void AddFuel()
    {
        Fuel += 20;
        H2O--;
        EmitSignal("MissionRewards", "H2O", H2O);
    }

    public void RecoveryMission()
    {
        var RocketInstance = Rocket.Instance<Spatial>();
        var tempOrigin = Transform;
        RocketInstance.Transform = tempOrigin;
        GetParent().AddChild(RocketInstance);
    }
}
