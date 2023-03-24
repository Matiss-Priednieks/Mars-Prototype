using Godot;
using System;

public class Settings : VBoxContainer
{
    bool Fullscreen = false;
    bool QHD = false;
    Button ResButton;
    Button FsButton;

    OptionButton ResOptions, WinOptions;

    Vector2[] ResArray;
    bool[] WinModeArray;


    Godot.Collections.Dictionary<string, Vector2> Resolutions = new Godot.Collections.Dictionary<string, Vector2>(){
            {"3840x2160", new Vector2(3840, 2160)},
            {"2560x1440", new Vector2(2560, 1440)},
            {"1920x1080", new Vector2(1920, 1080)},
            {"1280×720", new Vector2(1280, 720)}};

    Godot.Collections.Dictionary<string, bool> WindowMode = new Godot.Collections.Dictionary<string, bool>(){
        {"Fullscreen", true},
        {"Windowed", false}
    };

    public override void _Ready()
    {
        ResArray = new Vector2[4];
        ResOptions = GetNode<OptionButton>("Resolution");

        foreach (var item in Resolutions)
        {
            ResOptions.AddItem(item.Key);
        }
        Resolutions.Values.CopyTo(ResArray, 0);

        WinModeArray = new bool[2];
        WinOptions = GetNode<OptionButton>("WindowMode");
        foreach (var item in WindowMode)
        {
            WinOptions.AddItem(item.Key);
        }
        WindowMode.Values.CopyTo(WinModeArray, 0);
    }


    public void _on_Resolution_item_selected(int index)
    {
        OS.WindowSize = ResArray[index];
        GetTree().SetScreenStretch(SceneTree.StretchMode.Viewport, SceneTree.StretchAspect.Keep, ResArray[index]);
    }
    public void _on_Fullscreen_item_selected(int index)
    {
        OS.WindowFullscreen = WinModeArray[index];
    }
}
