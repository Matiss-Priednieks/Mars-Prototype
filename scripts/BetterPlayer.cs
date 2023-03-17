using Godot;
using System;

public class BetterPlayer : KinematicBody
{
    //signals and exported variables
    [Signal]
    public delegate void MissionComplete(string missionName, string missionID);
    [Signal]
    public delegate void MissionStarted(string missionName, string missionID);
    [Export(PropertyHint.Range, "1, 50")] int timeScale = 1;

    //object references
    Spatial PlayerModel;
    Spatial PlanetMars;
    Material RoverMat;
    Label3D MissionLabel;
    Label3D MissionProgressLabel;
    Label FuelLabel;
    AudioStreamPlayer3D RoverMovementSound;
    AudioStreamPlayer3D MissionCompleteSound;
    Timer MissionTimer;

    // floats and bools
    float MoveSpeed = 0.1f;
    bool ClickMoving = false;
    bool Selected = false;
    bool isMissionStarted = false;
    bool MissionClick = false;
    int H2O = 0;
    int Scrap = 0;
    int ResearchPoints = 0;

    double Fuel = 100;

    //vectors
    Vector3 LocalGravity;
    Vector3 targetLocation = Vector3.Zero;
    Vector3 targetNormal = Vector3.One;

    // strings
    string tempMissionTitle;
    string tempMissionType;
    string CurrentMission;
    string MissionID;
    string MissionProgress;

    public override void _Ready()
    {
        this.Connect("MissionComplete", GetNode<Spatial>("../MissionGenerator"), "MissionCompleted");

        //object references
        PlayerModel = GetNode<Spatial>("Rover");
        MissionLabel = GetNode<Label3D>("Mission");
        MissionTimer = GetNode<Timer>("MissionTimer");
        FuelLabel = GetNode<Label>("CanvasLayer/Fuel");
        PlanetMars = GetParent().GetNode<Spatial>("Mars");
        MissionProgressLabel = GetNode<Label3D>("MissionProgress");
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
        MissionProgressLabel.Text = MissionProgress;
        if (isMissionStarted)
        {
            MissionProgress = (100 - ((int)MissionTimer.TimeLeft * 20)).ToString() + "%";
        }
        else
        {
            MissionProgress = "";
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
            if (Fuel > 0) Fuel -= 0.01f;
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
        MissionCompleteSound.Play();

        isMissionStarted = false;
        MissionClick = false;
        GetMissionReward(CurrentMission);
        tempMissionTitle = "";
        tempMissionType = "";
        CurrentMission = "";
        MissionProgress = "";
        MissionID = "";
    }

    public void GetMissionReward(string missionName)
    {
        if (missionName.Contains("Resource"))
        {
            if (missionName.Contains("H2O"))
            {
                //give h2o
            }
            else
            {
                //give scrap
            }
        }
        else if (missionName.Equals("Research"))
        {
            //give research points
        }
        else if (missionName.Equals("Recovery"))
        {
            //give recovery points
        }
    }
}
