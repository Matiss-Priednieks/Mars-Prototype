using Godot;
using System;

public class PauseMenu : Panel
{
    VBoxContainer Settings, Menu;
    public override void _Ready()
    {
        Menu = GetNode<VBoxContainer>("Menu");
        Settings = GetNode<VBoxContainer>("Settings");
    }

    public void _on_Settings_pressed()
    {
        Menu.Hide();
        Settings.Show();
    }

    public void _on_BackButton_pressed()
    {
        Settings.Hide();
        Menu.Show();
    }
}
