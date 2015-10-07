using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeometryMapGenerator : MonoBehaviour
{

    public Texture2D heightmap;
    public float scale;
    public float heightScale;
    public GameObject tile;
    public int radius;

    public GameObject focus;

    private int[,] geomap;
    private Vector3 prevLoc = Vector3.zero;
    private float xDif = 0;
    private float yDif = 0;

    private GameObject[,] tiles;

    void Start()
    {
        tiles = new GameObject[radius * 2, radius * 2];

        int width = (int)(heightmap.width * scale);
        int height = (int)(heightmap.height * scale);

        geomap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                geomap[x, y] = (int)(heightmap.GetPixel((int)(x / scale), (int)(y / scale)).r * heightScale) / 2;
            }
        }

        generateArea((int)focus.transform.position.x, (int)focus.transform.position.z);

        prevLoc = focus.transform.position;
    }

    void FixedUpdate()
    {
        if (focus.transform.position.x != prevLoc.x || focus.transform.position.z != prevLoc.z)
        {
            generateArea((int)focus.transform.position.x, (int)focus.transform.position.z);
        }

        prevLoc = focus.transform.position;
    }

    void generateArea(int x, int y)
    {
        int xLowerbounds = x - radius;
        int xUpperbounds = x + radius;
        int yLowerbounds = y - radius;
        int yUpperbounds = y + radius;

        if (xLowerbounds < 0) xLowerbounds = 0;
        if (xUpperbounds >= geomap.GetLength(0)) xUpperbounds = geomap.GetLength(0) - 1;
        if (yLowerbounds < 0) yLowerbounds = 0;
        if (yUpperbounds >= geomap.GetLength(1)) yUpperbounds = geomap.GetLength(1) - 1;

        smartClearTiles();
        //clearTiles();

        for (int i = xLowerbounds; i < xUpperbounds; i++)
        {
            for (int j = yLowerbounds; j < yUpperbounds; j++)
            {
                if (tiles[i - xLowerbounds, j - yLowerbounds] == null)
                {
                    GameObject temp = (GameObject)Instantiate(tile, new Vector3(i, (heightmap.GetPixel((int)(i / scale), (int)(j / scale)).r * heightScale) / 2, j), Quaternion.identity);
                    temp.GetComponent<Tile>().raise = true;
                    temp.GetComponent<Tile>().expand = true;
                    temp.GetComponent<Tile>().height = heightScale;
                    temp.transform.localScale = new Vector3(1, heightScale, 1);

                    tiles[i - xLowerbounds, j - yLowerbounds] = temp;
                }
            }
        }
    }

    void clearTiles()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (tiles[x, y] != null)
                {
                    tiles[x, y].GetComponent<Tile>().beginDestroy();
                    tiles[x, y] = null;
                }
            }
        }
    }


    void smartClearTiles()
    {
        xDif += focus.transform.position.x - prevLoc.x;
        yDif += focus.transform.position.z - prevLoc.z;

        int xInterval = 0;
        int yInterval = 0;

        if (xDif > 0)
        {
            xInterval = Mathf.FloorToInt(xDif);
        }
        else if (xDif < 0)
        {
            xInterval = Mathf.CeilToInt(xDif);
        }

        if (yDif > 0)
        {
            yInterval = Mathf.FloorToInt(yDif);
        }
        else if (xDif < 0)
        {
            yInterval = Mathf.CeilToInt(yDif);
        }

        xDif -= (float)xInterval;
        yDif -= (float)yInterval;

        if (xInterval == 0 && yInterval == 0)
        {
            return;
        }

        if (xInterval >= tiles.GetLength(0) || yInterval >= tiles.GetLength(1) || xInterval <= -tiles.GetLength(0) || yInterval <= -tiles.GetLength(1))
        {
            clearTiles();
            return;
        }

        Debug.Log(xInterval + ", " + yInterval);

        if (xInterval > 0)
        {
            for (int x = 0; x < xInterval; x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x, y] != null)
                    {
                        tiles[x, y].GetComponent<Tile>().beginDestroy();
                        tiles[x, y] = null;
                    }
                }
            }
        }
        else if (xInterval < 0)
        {
            for (int x = tiles.GetLength(0) + xInterval; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x, y] != null)
                    {
                        tiles[x, y].GetComponent<Tile>().beginDestroy();
                        tiles[x, y] = null;
                    }
                }
            }
        }

        if (yInterval > 0)
        {
            for (int y = 0; y < yInterval; y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    if (tiles[x, y] != null)
                    {
                        tiles[x, y].GetComponent<Tile>().beginDestroy();
                        tiles[x, y] = null;
                    }
                }
            }
        }
        else if (yInterval < 0)
        {
            for (int y = tiles.GetLength(1) + yInterval; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    if (tiles[x, y] != null)
                    {
                        tiles[x, y].GetComponent<Tile>().beginDestroy();
                        tiles[x, y] = null;
                    }
                }
            }
        }

        xScan(xInterval > 0, xInterval, yInterval);

        //for (int x = 0; x < tiles.GetLength(0); x++)
        //{
        //    for (int y = 0; y < tiles.GetLength(1); y++)
        //    {
        //        if ((x + xInterval < 0 || x + xInterval >= tiles.GetLength(0)) || (y + yInterval < 0 || y + yInterval >= tiles.GetLength(1)))
        //        {
        //            if (tiles[x,y] != null)
        //            {
        //                tiles[x, y].GetComponent<Tile>().beginDestroy();
        //            }
        //            tiles[x, y] = null;
        //        }
        //        else
        //        {
        //            if (tiles[x, y] != null && tiles[x + xInterval, y + yInterval] == null)
        //            {
        //                tiles[x, y].GetComponent<Tile>().beginDestroy();
        //            }
        //            tiles[x, y] = tiles[x + xInterval, y + yInterval];
        //        }
        //    }
        //}
    }

    private void xScan(bool positive, int xInterval, int yInterval)
    {
        if (positive)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                yScan(yInterval > 0, x, xInterval, yInterval);
            }
        }
        else
        {
            for (int x = tiles.GetLength(0) - 1; x >= 0; x--)
            {
                yScan(yInterval > 0, x, xInterval, yInterval);
            }
        }
    }

    private void yScan(bool positive, int x, int xInterval, int yInterval)
    {
        if (positive)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if ((tiles[x, y] != null && x - xInterval >= 0 && x - xInterval < tiles.GetLength(0) && y - yInterval >= 0 && y - yInterval < tiles.GetLength(1)) || (x - xInterval >= 0 && x - xInterval < tiles.GetLength(0) && y - yInterval >= 0 && y - yInterval < tiles.GetLength(1) && tiles[x - xInterval, y - yInterval] != null))
                {
                    GameObject temp = tiles[x - xInterval, y - yInterval];
                    tiles[x - xInterval, y - yInterval] = tiles[x, y];
                    tiles[x, y] = temp;
                }
            }
        }
        else
        {
            for (int y = tiles.GetLength(1) - 1; y >= 0; y--)
            {
                if ((tiles[x, y] != null && x - xInterval >= 0 && x - xInterval < tiles.GetLength(0) && y - yInterval >= 0 && y - yInterval < tiles.GetLength(1)) || (x - xInterval >= 0 && x - xInterval < tiles.GetLength(0) && y - yInterval >= 0 && y - yInterval < tiles.GetLength(1) && tiles[x - xInterval, y - yInterval] != null))
                {
                    GameObject temp = tiles[x - xInterval, y - yInterval];
                    tiles[x - xInterval, y - yInterval] = tiles[x, y];
                    tiles[x, y] = temp;
                }
            }
        }
    }

    //void smartClearTiles(int xLowerbounds, int xUpperbounds, int yLowerbounds, int yUpperbounds)
    //{
    //    for (int x = 0; x < tiles.GetLength(0); x++)
    //    {
    //        for (int y = 0; y < tiles.GetLength(1); y++)
    //        {
    //            if (tiles[x, y] != null && (!(tiles[x,y].transform.position.x >= xLowerbounds && tiles[x, y].transform.position.x <= xUpperbounds) || !(tiles[x, y].transform.position.z >= yLowerbounds && tiles[x, y].transform.position.z <= yUpperbounds)))
    //            {
    //                tiles[x, y].GetComponent<Tile>().beginDestroy();
    //                tiles[x, y] = null;
    //            }
    //        }
    //    }
    //}
}
