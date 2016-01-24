using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk {

    public int size;
    public float heightScale;
    public GameObject prefab;

    public int chunkX;
    public int chunkY;

    public float[,] values;
    public SmartTile[,] tiles;

	public Chunk(int x, int y, int iSize, int iHeightsScale, GameObject iPrefab)
    {
        chunkX = x;
        chunkY = y;
        size = iSize;
        values = new float[size, size];
        tiles = new SmartTile[size, size];
    }

    public void AddValue(int x, int y, float value)
    {
        values[chunkX - x, chunkY - y] = value;
    }

    public void AddTile(int x, int y, SmartTile newTile)
    {
        tiles[x, y] = newTile;
    }

    public void LoadTiles()
    {
        GameObject temp = (GameObject)GameObject.Instantiate(prefab, new Vector3(), Quaternion.identity);
    }

    public void UpdateVisuals()
    {
        foreach (SmartTile tile in tiles)
        {
            tile.updateVisuals();
        }
    }
}
