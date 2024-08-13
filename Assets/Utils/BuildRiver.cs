using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
[ExecuteInEditMode()]
public class BuildRiver : MonoBehaviour
{
    //private SplineContainer _splineContainer;
    //private MeshFilter _meshFilter;

    [SerializeField]
    private int _splineIndex;

    [Range(1.0f, 10.0f)]
    public float width;

    [Range(3.0f, 100.0f)]
    public float resolution;
    private List<Vector3> pathPositions;
    private List<Vector3> pathTangents;
    private float3 upVector;

    private GameObject objWater;
    private GameObject objGround;

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }


    public void BuildRiverMesh()
    {
        _getVerts();
        _buildWaterMesh();
        _buildGroundMesh();
    }

    private void _getVerts()
    {
        SplineContainer splineContainer = GetComponent<SplineContainer>();
        float3 position;
        float3 tangent;
        pathPositions = new List<Vector3>();
        pathTangents = new List<Vector3>();

        float step = 1.0f / resolution;

        for (int i = 0; i < resolution; i++) {
            float t = step * i;
            splineContainer.Evaluate(_splineIndex, t, out position, out tangent, out upVector);
            pathPositions.Add(position);
            pathTangents.Add(tangent);
        }
    }

    private void _buildWaterMesh()
    {
        Mesh meshWater = new Mesh();
        List<Vector3> vertsWater = new List<Vector3>();
        List<int> trisWater = new List<int>();
        int offset = 0;

        int length = pathPositions.Count;

        for (int i = 0; i < length; i++) {
            Vector3 left = Vector3.Cross(pathTangents[i], upVector).normalized;

            // WATER: Flat surface of width @ zero altitude
            Vector3 wp1 = pathPositions[i] + (left * width);
            Vector3 wp2 = pathPositions[i] + (-left * width);
            Vector3 wp3;
            Vector3 wp4;

            if (i < (length - 1)) {
                Vector3 nextLeft = Vector3.Cross(pathTangents[i + 1], upVector).normalized;
                wp3 = pathPositions[i + 1] + (nextLeft * width);
                wp4 = pathPositions[i + 1] + (-nextLeft * width);
            } else {
                Vector3 originLeft = Vector3.Cross(pathTangents[0], upVector).normalized;
                wp3 = pathPositions[0] + (originLeft * width);
                wp4 = pathPositions[0] + (-originLeft * width);
            }

            offset = 4 * i;

            int t1 = offset + 0;
            int t2 = offset + 2;
            int t3 = offset + 1;

            int t4 = offset + 2;
            int t5 = offset + 3;
            int t6 = offset + 1;

            vertsWater.AddRange(new List<Vector3> { wp1, wp2, wp3, wp4 });
            trisWater.AddRange(new List<int> { t1, t2, t3, t4, t5, t6 });
        }

        meshWater.SetVertices(vertsWater);
        meshWater.SetTriangles(trisWater, 0);

        DestroyImmediate(objWater);
        objWater = new GameObject(name + "Water");
        objWater.transform.SetParent(transform);

        MeshFilter meshWaterFilter = objWater.AddComponent<MeshFilter>();
        meshWaterFilter.mesh = meshWater;

        Material matWater = Resources.Load<Material>("Materials/GraphWater");
        objWater.AddComponent<MeshRenderer>().material = matWater;
    }

    private void _buildGroundMesh()
    {
        Mesh meshGround = new Mesh();
        List<Vector3> vertsGround = new List<Vector3>();
        List<int> trisGround = new List<int>();
        int offset = 0;

        int length = pathPositions.Count;

        for (int i = 0; i < length; i++) {
            Vector3 left = Vector3.Cross(pathTangents[i], upVector).normalized;

            // GROUND: Terrain mesh
            Vector3 depth = new Vector3(0, -10, 0);
            Vector3 gp1 = pathPositions[i] + (left * width) + depth;
            Vector3 gp2 = pathPositions[i] + (-left * width) + depth;
            Vector3 gp3;
            Vector3 gp4;

            if (i < (length - 1)) {
                Vector3 nextLeft = Vector3.Cross(pathTangents[i + 1], upVector).normalized;
                gp3 = pathPositions[i + 1] + (nextLeft * width) + depth;
                gp4 = pathPositions[i + 1] + (-nextLeft * width) + depth;
            } else {
                Vector3 originLeft = Vector3.Cross(pathTangents[0], upVector).normalized;
                gp3 = pathPositions[0] + (originLeft * width) + depth;
                gp4 = pathPositions[0] + (-originLeft * width) + depth;
            }

            offset = 4 * i;

            int t1 = offset + 0;
            int t2 = offset + 2;
            int t3 = offset + 1;

            int t4 = offset + 2;
            int t5 = offset + 3;
            int t6 = offset + 1;

            vertsGround.AddRange(new List<Vector3> { gp1, gp2, gp3, gp4 });
            trisGround.AddRange(new List<int> { t1, t2, t3, t4, t5, t6 });
        }

        meshGround.SetVertices(vertsGround);
        meshGround.SetTriangles(trisGround, 0);

        DestroyImmediate(objGround);
        objGround = new GameObject(name + "Ground");
        objGround.transform.SetParent(transform);

        MeshFilter meshGroundFilter = objGround.AddComponent<MeshFilter>();
        meshGroundFilter.mesh = meshGround;

        Material matGround = Resources.Load<Material>("Materials/Grass");
        objGround.AddComponent<MeshRenderer>().material = matGround;
    }

    private void _buildQuad()
    {
        //MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //meshRenderer.sharedMaterial = new Material(Shader.Find("Water"));

        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        vertices.AddRange(new List<Vector3>
        {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, 0, width),
            new Vector3(width, 0, width)
        });
        mesh.SetVertices(vertices);

        List<int> tris = new List<int>();
        tris.AddRange(new List<int>
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        });
        mesh.SetTriangles(tris, 0);
/*
        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;
        */
/*
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;
        */

        //_meshFilter.mesh = mesh;
    }
}
