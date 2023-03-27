using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public class WeatherController : Spatial
{
    private Godot.Collections.Array objects;

    Label DayLabel, TemperatureLabel, PressureLabel, WindLabel;
    // MeshInstance AtmosphereShader;
    Material AtmosphereShader;
    float WindSpeed = 0;
    float LastWindSpeed;
    Tween SpeedTransition;


    public override void _Ready()
    {
        SpeedTransition = GetNode<Tween>("Tween");
        AtmosphereShader = (Material)ResourceLoader.Load("res://assets/cloudShader.tres");
        DayLabel = GetNode<Label>("../../GUI/ExtraInfo/MarginContainer/VBoxContainer/WeatherLabels/Day");
        TemperatureLabel = GetNode<Label>("../../GUI/ExtraInfo/MarginContainer/VBoxContainer/WeatherLabels/Temp");
        WindLabel = GetNode<Label>("../../GUI/ExtraInfo/MarginContainer/VBoxContainer/WeatherLabels/Wind");

        var file = new Godot.File();
        file.Open("res://assets/weather.json", Godot.File.ModeFlags.Read);
        var jsonString = file.GetAsText();
        var jsonResult = JSON.Parse(jsonString);
        objects = (Godot.Collections.Array)jsonResult.Result;
        file.Close();

        WindSpeed = (float)randomWeatherData()[3];
        AtmosphereShader.Set("shader_param/WindSpeed", WindSpeed);
        LastWindSpeed = WindSpeed;

        DayLabel.Text = "Sol: " + randomWeatherData()[0].ToString();
        TemperatureLabel.Text = "Temp: " + ((float)randomWeatherData()[1] + (float)randomWeatherData()[2]) / 2 + "C";
        WindLabel.Text = "WindSpd: " + randomWeatherData()[3].ToString() + "km/h";
    }

    public override void _Process(float delta)
    {

    }

    public List<object> randomWeatherData()
    {
        List<object> WeatherData = new List<object>();
        Random random = new Random();
        int randomObjectIndex = random.Next(0, objects.Count);
        Godot.Collections.Dictionary randomObject = (Godot.Collections.Dictionary)objects[randomObjectIndex];
        List<string> keys = new List<string>();
        foreach (var key in randomObject.Keys)
        {
            keys.Add(key.ToString());
        }
        var id = keys[0];
        var sol = keys[1];
        var min_temp = keys[2];
        var max_temp = keys[3];
        var windspd = keys[5]; //change this to reflect what key I want (sol, temps etc...)

        var randomSol = randomObject[sol]; // picks random sol (day)
        var randomMin = randomObject[min_temp]; // picks random min temperature
        var randomMax = randomObject[max_temp]; // picks random max temperature
        var randomWind = randomObject[windspd]; // picks random windspeed

        WeatherData.Add(randomSol);
        WeatherData.Add(randomMin);
        WeatherData.Add(randomMax);
        WeatherData.Add(randomWind);
        WindSpeed = (float)randomWind;

        return WeatherData;
    }

    public void _on_Timer_timeout()
    {
        SpeedTransition.InterpolateProperty(AtmosphereShader, "shader_param/WindSpeed", LastWindSpeed, WindSpeed, 0.1f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        GD.Print(LastWindSpeed, WindSpeed);
        LastWindSpeed = WindSpeed;
        randomWeatherData();
        DayLabel.Text = "Sol: " + randomWeatherData()[0].ToString();
        TemperatureLabel.Text = "Temp: " + ((float)randomWeatherData()[1] + (float)randomWeatherData()[2]) / 2 + "C";
        WindLabel.Text = "WindSpd: " + randomWeatherData()[3].ToString() + "km/h";
        SpeedTransition.Start();
    }
}
