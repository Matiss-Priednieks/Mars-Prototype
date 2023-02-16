using Godot;
using System;

public class BetterPlayer : KinematicBody
{
    Spatial PlayerModel;
    Vector3 LocalGravity;
    float MoveSpeed = 0.1f;
    float RotationSpeed = 15;
    bool ClickMoving = false;

    [Export(PropertyHint.Range, "1, 50")] int timeScale = 1;

    Vector3 targetLocation = Vector3.Zero;
    Vector3 targetNormal = Vector3.One;
    Vector3 LastStrongMoveDirection;
    Spatial PlanetMars;
    public override void _Ready()
    {
        PlayerModel = GetNode<Spatial>("Rover");
        PlanetMars = GetParent().GetNode<Spatial>("planetmarslowerpoly");
        LocalGravity = new Vector3(PlanetMars.GlobalTransform.origin - Transform.basis.y);
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
    private void _on_ClickArea_input_event(Node camera, InputEvent new_event, Vector3 position, Vector3 normal, int shape_index)
    {
        if (new_event.IsActionReleased("click_to_move"))
        {
            targetNormal = normal;
            targetLocation = position;
            ClickMoving = true;
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
}
