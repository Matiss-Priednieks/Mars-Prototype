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

    [Export(PropertyHint.Range, "1, 50")] int timeScale = 1;

    Vector3 targetLocation = Vector3.Zero;
    Vector3 targetNormal = Vector3.One;
    Vector3 LastStrongMoveDirection;
    Spatial PlanetMars;
    Material RoverMat;
    public override void _Ready()
    {
        PlayerModel = GetNode<Spatial>("Rover");
        PlanetMars = GetParent().GetNode<Spatial>("planetmarslowerpoly");
        LocalGravity = new Vector3(PlanetMars.GlobalTransform.origin - Transform.basis.y);
        RoverMat = GetNode<Spatial>("Rover").GetNode<MeshInstance>("Cylinder").GetActiveMaterial(0).NextPass;
    }

    public override void _Process(float delta)
    {
        if (Selected) { RoverMat.Set("shader_param/grow", 0.02); } else { RoverMat.Set("shader_param/grow", 0); }
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!IsOnFloor())
        {
            MoveAndCollide(GravityVector());
        }

        if (Transform.basis.y.Normalized().Cross(GravityVector()) != Vector3.Zero)
        {
            LookAt(PlanetMars.GlobalTransform.origin, Transform.basis.y - GravityVector());
        }
        else
        {
            LookAt(PlanetMars.GlobalTransform.origin, Transform.basis.x - GravityVector());
        }

        ClickToMove(targetLocation, targetNormal);

    }

    public Vector3 GravityVector()
    {
        return (PlanetMars.Transform.origin - Transform.origin).Normalized();
    }
    private void _on_StaticBody_input_event(Node camera, InputEvent new_event, Vector3 position, Vector3 normal, int shape_index)
    {
        if (new_event.IsActionReleased("click_to_move") && Selected)
        {
            targetNormal = normal;
            targetLocation = position;
            ClickMoving = true;
        }
        if (new_event.IsActionReleased("mousepress") && Selected)
        {
            Selected = false;
        }
    }

    public void ClickToMove(Vector3 endLocation, Vector3 normal)
    {
        Vector3 diff = endLocation - Transform.origin;
        diff = diff.Normalized();
        if (diff.Length() >= 0.15f && ClickMoving)
        {
            MoveAndSlide(diff * MoveSpeed * timeScale, normal);
            LookAt(diff, normal);
            // GD.Print(diff);
        }
        else if (diff.Length() <= 0.12f)
        {
            ClickMoving = false;
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
}
