using UnityEngine;
using System.Collections;

public class ResetTerrain : MonoBehaviour
{
    [SerializeField] private Texture2D texture;
    private Terrain _terrain;
    private TerrainData _tData;
    private int _xRes;
    private int _yRes;
    private float[,] _heights;

    void Start()
    {
        _terrain = transform.GetComponent<Terrain>();
        _tData = _terrain.terrainData;
        _xRes = _tData.heightmapResolution;
        _yRes = _tData.heightmapResolution;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 25), "Wrinkle"))
        {
            RandomizePoints(0.1f);
        }

        if (GUI.Button(new Rect(10, 40, 100, 25), "Reset"))
        {
            ResetPoints();
        }

        if (GUI.Button(new Rect(10, 70, 100, 25), "SetTextureHeight"))
        {
            SetHeightFromTexture();
        }
    }

    void RandomizePoints(float strength)
    {
        _heights = _tData.GetHeights(0, 0, _xRes, _yRes);

        for (int y = 0; y < _yRes; y++)
        {
            for (int x = 0; x < _xRes; x++)
            {
                _heights[x, y] = Random.Range(0.0f, strength) * 0.5f;
            }
        }

        _tData.SetHeights(0, 0, _heights);
    }

    void ResetPoints()
    {
        var heights = _tData.GetHeights(0, 0, _xRes, _yRes);
        for (int y = 0; y < _yRes; y++)
        {
            for (int x = 0; x < _xRes; x++)
            {
                heights[x, y] = 0;
            }
        }

        _tData.SetHeights(0, 0, heights);
    }

    void SetHeightFromTexture()
    {
        Texture2D textureCopy = new Texture2D(texture.width, texture.height);
        textureCopy.SetPixels(texture.GetPixels());
        textureCopy.Apply();
        Scale(textureCopy, _xRes, _yRes);

        var heights = _tData.GetHeights(0, 0, textureCopy.width, textureCopy.height);

        for (int i = 0; i < _xRes; i++)
        {
            for (int j = 0; j < _yRes; j++)
            {
                Color pixelColor = textureCopy.GetPixel(i, j);
                heights[i, _yRes - j - 1] = Mathf.Min((1 - pixelColor.r) / 10, 0.05f);
            }
        }

        _tData.SetHeights(0, 0, heights);
    }

    private void Scale(Texture2D textureToScale, int width, int height)
    {
        Rect texR = new Rect(0, 0, width, height);
        textureToScale.filterMode = FilterMode.Trilinear;
        textureToScale.Apply(true);
        RenderTexture rtt = new RenderTexture(width, height, 32);
        Graphics.SetRenderTarget(rtt);
        GL.LoadPixelMatrix(0, 1, 1, 0);
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), textureToScale);
        textureToScale.Resize(width, height);
        textureToScale.ReadPixels(texR, 0, 0, true);
        textureToScale.Apply(true);
    }
}