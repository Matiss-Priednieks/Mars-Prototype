using Godot;
using System;
using System.Text;
using Godot.Collections;
class HTTPRequestDemo : CanvasLayer
{
    public override void _Ready()
    {
        GetNode("HTTPRequest").Connect("request_completed", this, "OnRequestCompleted");
        GetNode("Button").Connect("pressed", this, "OnButtonPressed");
    }

    public void OnButtonPressed()
    {
        HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");
        httpRequest.Request("https://api.nasa.gov/insight_weather/?api_key=OcEGj33d2t0Wu7OGgEQCUbPzoX5n0oohJgKwq7ud&feedtype=json&ver=1.0");
    }

    public void OnRequestCompleted(int result, int response_code, string[] headers, byte[] body)
    {
        JSONParseResult json = JSON.Parse(Encoding.UTF8.GetString(body));
        var weather = json.Result as Dictionary;

        GD.Print(weather["sol_keys"]);

    }
}