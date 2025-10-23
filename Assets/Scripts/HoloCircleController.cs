using UnityEngine;

// This script creates a holographic circle effect at a specified location.
// It's ideal for marking teleport points or other areas of interest.

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class HoloCircleController : MonoBehaviour
{
    // Public variables for customization in the Unity Inspector
    public float radius = 1f;
    public int segments = 64;
    public Color circleColor = new Color(0, 0.5f, 1, 0.7f); // Default blue color
    public float pulseSpeed = 1.5f;
    public float pulseMagnitude = 0.1f;

    private Mesh mesh;
    private Material material;

    void Start()
    {
        // Get the MeshRenderer component and its material
        material = GetComponent<MeshRenderer>().material;
        // Set the shader to the custom one we'll create.
        // Make sure you have a shader named "HologramCircle" in your project.
        material.shader = Shader.Find("Shader Graphs/HologramCircle");

        // Generate the circular mesh
        GenerateCircle();

        // Set initial material properties
        material.SetColor("_BaseColor", circleColor);
    }

    void Update()
    {
        // Create a simple pulsating effect for the circle's glow
        float pulse = 1.0f + Mathf.Sin(Time.time * pulseSpeed) * pulseMagnitude;
        // The "_Pulse" property will be used in our shader
        material.SetFloat("_Pulse", pulse);
    }

    // Creates the circular mesh for the holographic effect
    void GenerateCircle()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Pre-allocate arrays for vertices, triangles, and UVs
        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];
        Vector2[] uvs = new Vector2[segments + 1];

        // The center of the circle
        vertices[0] = Vector3.zero;
        uvs[0] = new Vector2(0.5f, 0.5f);

        // Generate vertices for the circle's edge
        for (int i = 0; i < segments; i++)
        {
            float angle = i * (360f / segments) * Mathf.Deg2Rad;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            uvs[i + 1] = new Vector2((vertices[i + 1].x / (2 * radius)) + 0.5f, (vertices[i + 1].z / (2 * radius)) + 0.5f);
        }

        // Generate triangles to form the circle's surface
        for (int i = 0; i < segments; i++)
        {
            int triangleIndex = i * 3;
            triangles[triangleIndex] = 0;
            triangles[triangleIndex + 1] = i + 1;
            triangles[triangleIndex + 2] = (i == segments - 1) ? 1 : i + 2;
        }

        // Assign the generated data to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }
}
