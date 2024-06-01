using UnityEngine;

public class MeshToTerrain : MonoBehaviour
{
    public Terrain terrain;
    public MeshFilter meshFilter;
    public Vector3 terrainSize = new Vector3(100, 20, 100);

    void Start()
    {
        if (terrain == null || meshFilter == null)
        {
            Debug.LogError("Terrain or MeshFilter not assigned.");
            return;
        }

        Mesh mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            Debug.LogError("No mesh found in the MeshFilter.");
            return;
        }

        Vector3[] vertices = mesh.vertices;

        // Calculate bounds to normalize vertices
        Vector3 minBounds = mesh.bounds.min;
        Vector3 maxBounds = mesh.bounds.max;
        Vector3 sizeBounds = maxBounds - minBounds;

        int resolution = terrain.terrainData.heightmapResolution;
        float[,] heightMap = new float[resolution, resolution];

        // Create a counter to track the number of times a cell in the heightMap is accessed
        int[,] heightMapCount = new int[resolution, resolution];

        for (int i = 0; i < vertices.Length; i++)
        {
            // Normalize vertex positions to range [0, 1]
            float normalizedX = (vertices[i].x - minBounds.x) / sizeBounds.x;
            float normalizedY = (vertices[i].y - minBounds.y) / sizeBounds.y;
            float normalizedZ = (vertices[i].z - minBounds.z) / sizeBounds.z;

            int x = Mathf.Clamp(Mathf.RoundToInt(normalizedX * (resolution - 1)), 0, resolution - 1);
            int z = Mathf.Clamp(Mathf.RoundToInt(normalizedZ * (resolution - 1)), 0, resolution - 1);

            heightMap[z, x] += normalizedY;
            heightMapCount[z, x]++;
        }

        // Average the height values
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                if (heightMapCount[y, x] > 0)
                {
                    heightMap[y, x] /= heightMapCount[y, x];
                }
            }
        }

        // Apply heights to terrain
        terrain.terrainData.heightmapResolution = resolution;
        terrain.terrainData.size = terrainSize;
        terrain.terrainData.SetHeights(0, 0, heightMap);

        Debug.Log("Terrain generation completed.");
    }
}
