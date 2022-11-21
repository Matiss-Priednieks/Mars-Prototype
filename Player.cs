using Godot;
using System;

public class Player : RigidBody
{
    Spatial PlayerModel;
    Vector3 LocalGravity;
    float MoveSpeed = 70;
    float RotationSpeed = 15;
    Vector3 LastStrongMoveDirection;
    public override void _Ready()
    {
        PlayerModel = GetNode<Spatial>("Model");
    }

    public override void _IntegrateForces(PhysicsDirectBodyState state)
    {
        LocalGravity = state.TotalGravity.Normalized();
        var MoveDirection = GetModelOrientedInput();
        if (MoveDirection.Length() > 0.2f)
        {
            LastStrongMoveDirection = MoveDirection.Normalized();
        }
        OrientCharToDirection(LastStrongMoveDirection, state.Step);

        AddCentralForce(GetModelOrientedInput() * MoveSpeed);
    }

    public Vector3 GetModelOrientedInput()
    {
        var InputLeftRight = Godot.Input.GetActionStrength("ui_right") - Godot.Input.GetActionStrength("ui_left");
        var InputForwardBackward = Godot.Input.GetActionStrength("ui_up") - Godot.Input.GetActionStrength("ui_down");
        var RawInput = new Vector2(InputLeftRight, InputForwardBackward);

        var Input = Vector3.Zero;

        Input.x = RawInput.x * Mathf.Sqrt((float)(1.0 - RawInput.y * RawInput.y / 2.0));
        Input.z = RawInput.y * Mathf.Sqrt((float)(1.0 - RawInput.x * RawInput.x / 2.0));

        Input = Transform.basis.Xform(Input);
        return Input;
    }

    public void OrientCharToDirection(Vector3 Direction, float delta)
    {
        var LeftAxis = -LocalGravity.Cross(Direction);
        var RotationBasis = new Basis(LeftAxis, -LocalGravity, Direction).Orthonormalized();
        var RotationQuat = PlayerModel.Transform;
        var newRotationBasis = new Quat(RotationBasis);

        RotationQuat.basis = new Basis(PlayerModel.Transform.basis.RotationQuat().Slerp(newRotationBasis.Normalized(), delta * RotationSpeed));
        PlayerModel.Transform = RotationQuat;

    }

    public bool IsOnFloor(PhysicsDirectBodyState state)
    {
        for (int i = 0; i < state.GetContactCount(); i++)
        {
            var ContactNormal = state.GetContactLocalNormal(i);
            if (ContactNormal.Dot(-LocalGravity) > 0.5f)
            {
                return true;
            }
        }
        return false;
    }
}
