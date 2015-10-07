using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartGeometryMap : MonoBehaviour {

    public Texture2D heightmap;
    public float scale;
    public float heightScale;
    public GameObject tile;

    public float maxDistance;
    public float minDistance;

    public GameObject focus;

    public SmartTile[,] tiles;

    void Start()
    {
        SmartTile.maxDistance = this.maxDistance;
        SmartTile.minDistance = this.minDistance;

        int width = (int)(heightmap.width * scale);
        int height = (int)(heightmap.height * scale);

        tiles = new SmartTile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject temp = (GameObject)Instantiate(tile, new Vector3(x, (heightmap.GetPixel((int)(x / scale), (int)(y / scale)).r * heightScale) / 2, y), Quaternion.identity);
                temp.GetComponent<SmartTile>().height = heightScale;
                temp.GetComponent<SmartTile>().focus = focus;
                tiles[x,y] = temp.GetComponent<SmartTile>();
            }
        }
    }

    void FixedUpdate()
    {
        SmartTile.maxDistance = this.maxDistance;
        SmartTile.minDistance = this.minDistance;

        int range = (int)(maxDistance * 1.5f);

        int maxX = (int)focus.transform.position.x + range;
        int minX = (int)focus.transform.position.x - range;
        int maxY = (int)focus.transform.position.z + range;
        int minY = (int)focus.transform.position.z - range;

        if (maxX > tiles.GetLength(0)) maxX = tiles.GetLength(0);
        if (minX < 0) minX = 0;
        if (maxY > tiles.GetLength(1)) maxY = tiles.GetLength(1);
        if (minY < 0) minY = 0;

        //minX = 0;
        //maxX = tiles.GetLength(0);
        //minY = 0;
        //maxY = tiles.GetLength(0);

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                tiles[x,y].updateVisuals();
            }
        }
    }
}
