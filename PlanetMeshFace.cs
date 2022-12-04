using Godot;
using System;
public class PlanetMeshFace : MeshInstance
{
    [Export] Vector3 normal;
    [Export] int Resolution = 5;
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {

    }

    public void RegenerateMesh()
    {
        Godot.Collections.Array arrays = new Godot.Collections.Array();
        arrays.Resize((int)ArrayMesh.ArrayType.Max);
        Vector3[] vertexArray;
        Vector2[] uvArray;
        Vector3[] normalArray;
        int[] indexArray;

        int numberVertices = Resolution * Resolution;
        int numberIndices = (Resolution - 1) * (Resolution - 1) * 6;

        normalArray = new Vector3[numberVertices];
        uvArray = new Vector2[numberVertices];
        vertexArray = new Vector3[numberVertices];
        indexArray = new int[numberIndices];

        int triangleIndex = 0;
        Vector3 axisA = new Vector3(normal.y, normal.z, normal.x);
        Vector3 axisB = new Vector3(normal.Cross(axisA));
        for (int i = 0; i < Resolution; i++)
        {
            for (int j = 0; j < Resolution; j++)
            {
                int pos = i + j * Resolution;
                Vector2 percent = new Vector2(i, j) / (Resolution - 1);
                Vector3 pointOnUnitCube = new Vector3(normal + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB);
                Vector3 pointOnUnitSphere = pointOnUnitCube.Normalized();
                vertexArray[pos] = pointOnUnitSphere;

                if (i != Resolution - 1 && j != Resolution - 1)
                {
                    indexArray[triangleIndex + 2] = pos;
                    indexArray[triangleIndex + 1] = pos + Resolution + 1;
                    indexArray[triangleIndex] = pos + Resolution;

                    indexArray[triangleIndex + 5] = pos;
                    indexArray[triangleIndex + 4] = pos + 1;
                    indexArray[triangleIndex + 3] = pos + Resolution + 1;
                    triangleIndex += 6;
                }
            }
        }
        for (int a = 0; a < indexArray.Length; a += 3)
        {
            int b = a + 1;
            int c = a + 2;
            Vector3 ab = vertexArray[indexArray[b]] - vertexArray[indexArray[a]];
            Vector3 bc = vertexArray[indexArray[c]] - vertexArray[indexArray[b]];
            Vector3 ca = vertexArray[indexArray[a]] - vertexArray[indexArray[c]];

            Vector3 CrossABBC = ab.Cross(bc) * -1;
            Vector3 CrossBCCA = bc.Cross(ca) * -1;
            Vector3 CrossCAAB = ca.Cross(ab) * -1;
            normalArray[indexArray[a]] += CrossABBC + CrossBCCA + CrossCAAB;
            normalArray[indexArray[b]] += CrossABBC + CrossBCCA + CrossCAAB;
            normalArray[indexArray[c]] += CrossABBC + CrossBCCA + CrossCAAB;
        }
        for (int i = 0; i < normalArray.Length; i++)
        {
            normalArray[i] = normalArray[i].Normalized();
        }
        arrays[(int)Mesh.ArrayType.Vertex] = vertexArray;
        arrays[(int)Mesh.ArrayType.Normal] = normalArray;
        arrays[(int)Mesh.ArrayType.TexUv] = uvArray;
        arrays[(int)Mesh.ArrayType.Index] = indexArray;

        CallDeferred("_UpdateMesh", arrays);
    }

    public void _UpdateMesh(Godot.Collections.Array arrays)
    {
        ArrayMesh _mesh = new ArrayMesh();
        _mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
        Mesh = _mesh;
    }

}
