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
    string tempMissionTitle;
    string tempMissionType;

    string CurrentMission;
    string MissionID;

    public override void _Ready()
    {
        this.Connect("MissionStarted", GetNode<Spatial>("../MissionGenerator"), "MissionStarted");
        this.Connect("MissionComplete", GetNode<Spatial>("../MissionGenerator"), "MissionCompleted");
        PlayerModel = GetNode<Spatial>("Rover");
        PlanetMars = GetParent().GetNode<Spatial>("Mars");
        LocalGravity = new Vector3(PlanetMars.GlobalTransform.origin - Transform.basis.y);
        RoverMat = GetNode<Spatial>("Rover").GetNode<MeshInstance>("Cylinder").GetActiveMaterial(0).NextPass;
        MissionLabel = GetNode<Label3D>("Label3D");
    }

    public override void _Process(float delta)
    {
        if (Selected) { RoverMat.Set("shader_param/grow", 0.02); } else { RoverMat.Set("shader_param/grow", 0); }
        MissionLabel.Text = CurrentMission;
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!IsOnFloor())
        {
            MoveAndCollide(GravityVector());
        }


        MissionChecker();
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
        if ((destination.DistanceTo(Transform.origin) <= 0.5f && MissionClick) || destination.DistanceTo(Transform.origin) <= 0.1f)
        {
            GD.Print("Stopped");
            ClickMoving = false;
            if (MissionClick)
            {
                isMissionStarted = true;
            }
        }
    }
    public void SetTimeScale(int value)
    {
        timeScale = value;
        GD.Print(timeScale);
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
            MissionClick = true;
            tempMissionTitle = selectedMission;
            tempMissionType = selectedMissionType;
            MissionID = missionID;
        }
    }


    public async void MissionChecker()
    {
        if (isMissionStarted)
        {
            if (tempMissionType != null)
            {
                CurrentMission = tempMissionTitle + ": " + tempMissionType;
            }
            else
            {
                CurrentMission = tempMissionTitle;
            }
            ClickMoving = false;
            // EmitSignal("MissionStarted", CurrentMission, MissionID);
            await ToSignal(GetTree().CreateTimer(5), "timeout");

            //make a timer node start counting here.
            EmitSignal("MissionComplete", CurrentMission, MissionID);




            isMissionStarted = false;
            MissionClick = false;
        }
        else
        {
            CurrentMission = "";

        }
    }
}
