using Godot;
using System;

public class Coordinates : Node
{
    public static Coordinate PointToCoordinate(Vector3 pointOnUnitSphere)
    {
        float latitude = Mathf.Asin(pointOnUnitSphere.y);
        float a = pointOnUnitSphere.x;
        float b = -pointOnUnitSphere.z;

        float longitude = Mathf.Atan2(a, b);
        // GD.Print(pointOnUnitSphere.y);
        return new Coordinate(longitude, latitude);
    }

    public static Vector3 CoordinateToPoint(Coordinate coordinate, float radius)
    {
        float y = Mathf.Sin(coordinate.latitude);
        float r = Mathf.Cos(coordinate.latitude); // radius of 2d circle cut through sphere at 'y'
        float x = Mathf.Sin(coordinate.longitude) * r;
        float z = -Mathf.Cos(coordinate.longitude) * r;

        return new Vector3(x, y, z) * radius;
    }
    public struct Coordinate
    {

        public float longitude;
        public float latitude;

        public Coordinate(float longitude, float latitude)
        {
            this.longitude = longitude;
            this.latitude = latitude;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(longitude, latitude);
        }

        public Vector2 ToUV()
        {
            return new Vector2((longitude + (float)Math.PI) / (2 * (float)Math.PI), (latitude + (float)Math.PI / 2) / (float)Math.PI);
        }
    }
}
