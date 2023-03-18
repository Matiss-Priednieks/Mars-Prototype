using Godot;
using System;

public class MainCam : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Vector2 MouseDelta;
    Vector3 NextMousePos, PrevMousePos;
    bool MouseEntered = false;
    bool Rotating = false;
    public float rotationSpeed = 0.1f;
    bool RotatingLock, RotatingY, RotatingZ;
    Tween Snapback;

    public override void _Ready()
    {
        RotatingLock = false;
        RotatingY = false;
        RotatingZ = false;
        Snapback = GetNode<Tween>("MainCam/Snapback");
    }

    public override void _Process(float delta)
    {


        if (Input.IsActionJustPressed("XRot"))
        {
            RotatingLock = !RotatingLock;
        }


        if (Input.IsActionJustPressed("rotate") && MouseEntered)
        {
            PrevMousePos = new Vector3(GetViewport().GetMousePosition().x, GetViewport().GetMousePosition().y, 0);
            Rotating = true;
        }
        if (Input.IsActionJustReleased("rotate"))
        {
            Rotating = false;
            PrevMousePos = Vector3.Zero;
        }
        if (Input.IsActionJustPressed("ui_escape"))
        {
            Snapback.InterpolateProperty(this, "rotation_degrees", RotationDegrees, new Vector3(0, 0, 0), 0.5f, Tween.TransitionType.Elastic, Tween.EaseType.InOut);
            Snapback.Start();
        }

        if (Rotating)
        {
            NextMousePos = new Vector3(GetViewport().GetMousePosition().x, GetViewport().GetMousePosition().y, 0);
            Vector3 cameraRotation = RotationDegrees;
            NextMousePos.x += cameraRotation.y;
            NextMousePos.y += cameraRotation.x;
            NextMousePos.z += cameraRotation.z;
            PrevMousePos.x += cameraRotation.y;
            PrevMousePos.y += cameraRotation.x;
            PrevMousePos.z += cameraRotation.z;


            if (RotatingLock)
            {
                Vector3 rotation = (NextMousePos - PrevMousePos) * rotationSpeed * delta;
                // RotateX(-(NextMousePos.y - PrevMousePos.y) * rotationSpeed * delta);
                // RotateY((NextMousePos.x - PrevMousePos.x) * rotationSpeed * delta);
                // RotateZ(-(NextMousePos.y - PrevMousePos.y) * rotationSpeed * delta);
                GlobalTransform = GlobalTransform.Rotated(new Vector3(1, 0, 0), -rotation.x);
                GlobalTransform = GlobalTransform.Rotated(new Vector3(0, 1, 0), -rotation.y);
                GlobalTransform = GlobalTransform.Rotated(new Vector3(0, 0, 1), -rotation.z);
                PrevMousePos = NextMousePos;
            }
        }
        else
        {
            NextMousePos = Vector3.Zero;
        }

    }

    private void _on_StaticBody_mouse_entered()
    {
        MouseEntered = true;
    }
    private void _on_StaticBody_mouse_exited()
    {
        MouseEntered = false;
    }



}





