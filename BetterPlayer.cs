using Godot;
using System;

public class BetterPlayer : KinematicBody
{
    Spatial PlayerModel;
    Vector3 LocalGravity;
    float MoveSpeed = 70;
    float RotationSpeed = 15;
    bool ClickMoving = false;

    Vector3 LastStrongMoveDirection;
    Mars PlanetMars;
    public override void _Ready()
    {
        PlayerModel = GetNode<Spatial>("Model");
        PlanetMars = GetParent().GetNode<Mars>("Mars");
        LocalGravity = new Vector3(PlanetMars.GlobalTransform.origin - Transform.basis.y);
        ClickToMove();
    }

    public override void _PhysicsProcess(float delta)
    {
        // GD.Print(IsOnFloor());
        if (!IsOnFloor())
        {
            Translation += GravityVector();
        }
        var vel = Vector3.Zero;

        // if (Input.IsActionPressed("ui_up"))
        // {
        //     vel -= Transform.basis.y;

        // }

        // if (Input.IsActionPressed("ui_down"))
        // {
        //     vel += Transform.basis.y;

        // }

        // if (Input.IsActionPressed("ui_left"))
        // {
        //     vel += Transform.basis.x;

        // }

        // if (Input.IsActionPressed("ui_right"))
        // {
        //     vel -= Transform.basis.x;
        // }


        vel *= MoveSpeed * delta;

        vel = MoveAndSlide(vel, GravityVector() * -1, true, 4, Mathf.Deg2Rad(40), false);

        // GD.Print(vel);

        Transform = Transform.Orthonormalized();

        if (Transform.basis.y.Normalized().Cross(GravityVector()) != Vector3.Zero)
        {
            LookAt(PlanetMars.GlobalTransform.origin, Transform.basis.y);
        }
        else
        {
            LookAt(PlanetMars.GlobalTransform.origin, Transform.basis.x);

        }

    }

    public Vector3 GravityVector()
    {
        return (PlanetMars.Transform.origin - Transform.origin).Normalized();
    }
    private void _on_Area_input_event(Node camera, InputEvent new_event, Vector3 position, Vector3 normal, int shape_index)
    {
        if (new_event.IsActionReleased("click_to_move"))
        {
            GD.Print(camera + " " + position);
            ClickMoving = true;
        }
    }


    public void ClickToMove(Vector3 endLocation)
    {
        Vector3 diff = Transform.origin - endLocation;
        if (diff.Length() >= 0.1f)
        {
            MoveAndSlide(endLocation);
        }
    }
}
