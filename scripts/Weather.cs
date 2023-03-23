using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public class Weather : Node
{
    private Godot.Collections.Array objects;

    public override void _Ready()
    {
        var file = new Godot.File();
        file.Open("res://assets/weather.json", Godot.File.ModeFlags.Read);
        var jsonString = file.GetAsText();
        var jsonResult = JSON.Parse(jsonString);
        objects = (Godot.Collections.Array)jsonResult.Result;
        file.Close();

        Random random = new Random();
        int randomObjectIndex = random.Next(0, objects.Count);
        Godot.Collections.Dictionary randomObject = (Godot.Collections.Dictionary)objects[randomObjectIndex];
        List<string> keys = new List<string>();
        foreach (var key in randomObject.Keys)
        {
            keys.Add(key.ToString());
            GD.Print(key);
        }
        var randomKey = keys[random.Next(0, keys.Count)]; //change this to reflect what key I want (sol, pressure etc...)
        var randomValue = randomObject[randomKey]; // this will pick a random value from that key.


    }
}
