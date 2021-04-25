using UnityEngine;
using System.Collections;
using System;

//! Sample terrain animator/generator
public class RandomTerrain : MonoBehaviour
{
    private Terrain _terrain;
    private TerrainData _terrainData;
    private int _xRes;
    private int _yRes;
    float[,] _terrainHeights;
    float[,] _originalTerrainSectionHeight;
    public int numberOfPasses = 5;

    public int radiusOfAnimation = 50;
    
    void Start()
    {
        _terrain = GetComponent<Terrain>();
        _terrainData = _terrain.terrainData;
// Get terrain dimensions in tiles (X tiles x Y tiles)
        _xRes = _terrainData.heightmapResolution;
        _yRes = _terrainData.heightmapResolution;
// Set heightmap
        RandomizeTerrain();
    }
    
    void Update()
    {
        Anim();
    }

// Set the terrain using noise pattern
    private void RandomizeTerrain()
    {
// Extract entire heightmap (expensive!)
        _terrainHeights = _terrainData.GetHeights(0, 0, _xRes, _yRes);
// STUDENT'S CODE //
// ...
        float[] scale = new float[5];
        for (int k = 0; k < numberOfPasses; k++)
        {
            scale[k] = UnityEngine.Random.Range(4.0f, 12.0f);
        }

        for (int i = 0; i < _xRes; i++)
        {
            for (int j = 0; j < _yRes; j++)
            {
                float xCoeff = (float) i / _xRes;
                float yCoeff = (float) j / _yRes;
                _terrainHeights[i, j] = 0;
                for (int k = 0; k < numberOfPasses; k++)
                {
                    _terrainHeights[i, j] +=
                        Mathf.PerlinNoise(xCoeff * scale[k], yCoeff * scale[k]);
                }

                _terrainHeights[i, j] /= numberOfPasses;
            }
        }

//
// Set terrain heights (_terrHeights[coordX, coordY] = heightValue) in a loop
//
// You can sample perlin's noise (Mathf.PerlinNoise (xCoeff, yCoeff)) using
//coefficients
// between 0.0f and 1.0f
//
// You can combine 2-3 layers of noise with different resolutions and amplitudes for
//a better effect
// ...
// END OF STUDENT'S CODE //
// Set entire heightmap (expensive!)
        _terrainData.SetHeights(0, 0, _terrainHeights);
        _originalTerrainSectionHeight = _terrainData.GetHeights(147, 168, radiusOfAnimation * 2,
            radiusOfAnimation * 2);
    }

// Animate part of the terrain
    private void Anim()
    {
// STUDENT'S CODE //
// ...
// Extract PART of the terrain e.g. 40x40 tiles (select corner (x, y) and extracted
//patch size)
// GetHeights(5, 5, 10, 10) will extract 10x10 tiles at position (5, 5)
//
// Animate it using Time.time and trigonometric functions
//
// 3d generalizaton of sinc(x) function can be used to create the teardrop effect
//(sinc(x) = sin(x) / x)
//
// It is reasonable to store animated part of the terrain in temporary variable e.g.
//in RandomizeTerrain()
// function. Later, in AnimTerrain() this temporary area can be combined with
//calculated Z (height) value.
// Make sure you make a deep copy instead of shallow one (Clone(), assign operator).
//
// Set PART of the terrain (use extraction parameters)
//
// END OF STUDENT'S CODE //
        _terrainHeights = _terrainData.GetHeights(147, 168, radiusOfAnimation * 2,
            radiusOfAnimation * 2);
        
        Vector2 middle = new Vector2(radiusOfAnimation, radiusOfAnimation);
        for (int i = 0; i < radiusOfAnimation * 2; i++)
        {
            for (int j = 0; j < radiusOfAnimation * 2; j++)
            {
                Vector2 point = new Vector2(i, j);
                double distance = Vector2.Distance(point, middle);
                double difference = (radiusOfAnimation - distance) /
                                    radiusOfAnimation;
                if (difference < 0) difference = 0;
                _terrainHeights[i, j] = (float) (_originalTerrainSectionHeight[i, j] *
                                              (Math.Sin(Time.time + distance / 10) / 2f) * difference) +
                                     _originalTerrainSectionHeight[i, j];
            }
        }

        _terrainData.SetHeights(147, 168, _terrainHeights);
    }
}