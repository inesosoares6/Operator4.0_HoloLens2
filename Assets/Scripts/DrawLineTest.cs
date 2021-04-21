using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineTest : MonoBehaviour
{
    private LineRenderer lineRenderer;
    //private Vector3[] positions;
    private List<Vector3> positions = new List<Vector3>();

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //positions = new Vector3[3] { new Vector3(0, 0, 0), new Vector3(-1, 1, 0), new Vector3(1, 1, 0) };
        positions.Add(new Vector3(0, 0, 0));
        positions.Add(new Vector3(-1, 1, 0));
        positions.Add(new Vector3(1, 1, 0));
        //DrawTriangle(positions.ToArray(), 0.02f, 0.02f);
    }

    void Update()
    {
        positions.Add(new Vector3(2, 2, 0));
        positions.Add(new Vector3(0, 1, 0));
        DrawTriangle(positions.ToArray(), 0.02f, 0.02f);
    }

    void DrawTriangle(Vector3[] vertexPositions, float startWidth, float endWidth)
    {
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.loop = true;
        lineRenderer.positionCount = 5;
        lineRenderer.SetPositions(vertexPositions);
    }
}
