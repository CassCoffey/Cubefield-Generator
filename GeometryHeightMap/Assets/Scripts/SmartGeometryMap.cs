using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartGeometryMap : MonoBehaviour {

    public Texture2D heightmap;
    public float scale;
    public float heightScale;
    public GameObject tile;

    public int chunkSize;

    public float maxDistance;
    public float minDistance;

    public GameObject focus;

    public float[,] values;
    public List<Chunk> chunks;

    void Start()
    {
        SmartTile.maxDistance = this.maxDistance;
        SmartTile.minDistance = this.minDistance;

        int width = (int)(heightmap.width * scale);
        int height = (int)(heightmap.height * scale);

        values = new float[width, height];
        chunks = new List<Chunk>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                values[x, y] = (heightmap.GetPixel((int)(x / scale), (int)(y / scale)).r * heightScale);
            }
        }
    }

    void FixedUpdate()
    {
        SmartTile.maxDistance = this.maxDistance;
        SmartTile.minDistance = this.minDistance;

        float range = maxDistance * 1.5f;

        float maxX = focus.transform.position.x + range;
        float minX = focus.transform.position.x - range;
        float maxY = focus.transform.position.z + range;
        float minY = focus.transform.position.z - range;

        if (maxX > values.GetLength(0)) maxX = values.GetLength(0);
        if (minX < 0) minX = 0;
        if (maxY > values.GetLength(1)) maxY = values.GetLength(1);
        if (minY < 0) minY = 0;

        //for (int x = (int)minX; x < maxX; x++)
        //{
        //    for (int y = (int)minY; y < maxY; y++)
        //    {
        //        if (chunks.con != null)
        //        {
        //
        //        }
        //    }
        //}
    }
}