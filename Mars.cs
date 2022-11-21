using Godot;
using System;

public class Mars : CSGSphere
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Vector2 mouseDelta;
    bool mouseEntered = false;
    public float rotationSpeed = 0.1f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Vector2 test;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //todo: mouse input for rotation
        if (Input.IsMouseButtonPressed(1) && mouseDelta.Length() > 0.1f)
        {
            if (mouseEntered)
            {
                RotationDegrees += new Vector3(mouseDelta.y * rotationSpeed, mouseDelta.x * rotationSpeed, 0);
                //todo: fix
            }
        }
        RotationDegrees += new Vector3(0, rotationSpeed, 0); // demo rotation


    }
    private void _on_Area_input_event(object camera, object @event, Vector3 position, Vector3 normal, int shape_idx)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            mouseDelta = mouseMotion.Relative;
        }
    }

    private void _on_Area_mouse_entered()
    {
        mouseEntered = true;
    }
    private void _on_Area_mouse_exited()
    {
        mouseEntered = false;
    }

    private void _on_Area_area_entered(object area)
    {

    }



}





