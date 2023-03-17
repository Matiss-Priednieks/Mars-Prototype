using Godot;
using System;

public class BetterPlayer : KinematicBody
{
    Spatial PlayerModel;
    Vector3 LocalGravity;
    float MoveSpeed = 0.1f;
    float RotationSpeed = 15;
    bool ClickMoving = false;
    bool Selected = false;
    bool isMissionStarted = false;
    bool MissionClick = false;

    [Export(PropertyHint.Range, "1, 50")] int timeScale = 1;

    [Signal]
    public delegate void MissionComplete(string missionName, string missionID);
    [Signal]
    public delegate void MissionStarted(string missionName, string missionID);
    Vector3 targetLocation = Vector3.Zero;
    Vector3 targetNormal = Vector3.One;
    Vector3 LastStrongMoveDirection;
    Spatial PlanetMars;
    Material RoverMat;
    Label3D MissionLabel;
    Label3D MissionProgressLabel;
    AudioStreamPlayer3D RoverMovementSound;
    AudioStreamPlayer3D MissionCompleteSound;
    Timer MissionTimer;
    string tempMissionTitle;
    string tempMissionType;

    string CurrentMission;
    string MissionID;

    string MissionProgress;

    public override void _Ready()
    {
        this.Connect("MissionComplete", GetNode<Spatial>("../MissionGenerator"), "MissionCompleted");
        PlayerModel = GetNode<Spatial>("Rover");
        PlanetMars = GetParent().GetNode<Spatial>("Mars");
        LocalGravity = new Vector3(PlanetMars.GlobalTransform.origin - Transform.basis.y);
        RoverMat = GetNode<Spatial>("Rover").GetNode<MeshInstance>("Cylinder").GetActiveMaterial(0).NextPass;
        MissionLabel = GetNode<Label3D>("Mission");
        MissionProgressLabel = GetNode<Label3D>("MissionProgress");
        RoverMovementSound = GetNode<AudioStreamPlayer3D>("RoverMovement");
        MissionCompleteSound = GetNode<AudioStreamPlayer3D>("MissionCompletedSound");
        MissionTimer = GetNode<Timer>("MissionTimer");
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
            MoveAndSlide(MovementDirection * MoveSpeed * timeScale, normal);
            LookAt(MovementDirection, -GravityVector());
        }
        if (((destination.DistanceTo(Transform.origin) <= 0.5f && onMission) || destination.DistanceTo(Transform.origin) <= 0.1f) && ClickMoving)
        {
            GD.Print("Stopped");
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
        // GD.Print(timeScale);
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
        GD.Print("?");
        //signals are firing nonstop for some reason?
    }

    public void _on_MissionTimer_timeout()
    {
        EmitSignal("MissionComplete", CurrentMission, MissionID);
        MissionCompleteSound.Play();

        isMissionStarted = false;
        MissionClick = false;
        CurrentMission = "";
        MissionProgress = "";
    }
}
