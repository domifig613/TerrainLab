using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
using Random = System.Random;

public class TerrainGame : MonoBehaviour
{
    private const int SizeOfTile = 8;
    private Terrain _terrain;
    private TerrainData _terrainData;
    private int _xRes;
    private int _yRes;
    private Vector2Int[,] _tilesStartPoints;
    private bool[,] _isTilesGoUp;
    private float[,] _tilesSpeeds;

    void Start()
    {
        _terrain = GetComponent<Terrain>();
        _terrainData = _terrain.terrainData;
        _xRes = _terrainData.heightmapResolution;
        _yRes = _terrainData.heightmapResolution;

        PrepareDataForAnimation();
    }

    private void PrepareDataForAnimation()
    {
        _tilesStartPoints = new Vector2Int[_xRes / SizeOfTile, _yRes / SizeOfTile];
        _isTilesGoUp = new bool[_xRes / SizeOfTile, _yRes / SizeOfTile];
        _tilesSpeeds = new float[_xRes / SizeOfTile, _yRes / SizeOfTile];
        
        for (int i = 0; i < _tilesStartPoints.GetLength(0); i++)
        {
            for (int j = 0; j < _tilesStartPoints.GetLength(1); j++)
            {
                _tilesStartPoints[i, j] = new Vector2Int(SizeOfTile * i, SizeOfTile * j);
                _isTilesGoUp[i, j] = Convert.ToBoolean(UnityEngine.Random.Range(0,2));
                _tilesSpeeds[i,j] = UnityEngine.Random.Range(1f, 5f) / 10000f;
            }
        }
    }

    private void MoveTile(float[,] heights, bool isGoUp, float speed)
    {
        var currentHeight = heights[0, 0];
        var nextHeight = isGoUp ? currentHeight + speed : currentHeight - speed;
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                heights[i, j] = nextHeight;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < _tilesStartPoints.GetLength(0); i++)
        {
            for (int j = 0; j < _tilesStartPoints.GetLength(1); j++)
            {
                float[,] terrainHeights = _terrainData.GetHeights(_tilesStartPoints[i,j].x, _tilesStartPoints[i,j].y, SizeOfTile, SizeOfTile);
                MoveTile(terrainHeights, _isTilesGoUp[i,j], _tilesSpeeds[i,j]);
                UpdateTileDirection((x: i, y: j), terrainHeights[0,0]);
                _terrainData.SetHeights(_tilesStartPoints[i,j].x, _tilesStartPoints[i,j].y, terrainHeights);
            }
        }
    }

    private void UpdateTileDirection((int x, int y) index, float height)
    {
        if (height > 0.4f)
        {
            _isTilesGoUp[index.x, index.y] = false;
        }
        else if(height < 0.05f)
        {
            _isTilesGoUp[index.x, index.y] = true;
            
            if (UnityEngine.Random.Range(0, 4) == 1)
            {
                _tilesSpeeds[index.x,index.y] = UnityEngine.Random.Range(1f, 5f) / 10000f;
            }
        }

    }
}