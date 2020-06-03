using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class FloorBrush : MonoBehaviour
{
    [SerializeField] private FloorTool tool = null;
    [SerializeField] private DecalProjector decal = null;

    [HideInInspector] public bool isPainting = false;
    [HideInInspector] public int layerToPaint = 0;

    private float fixSize = 0.10f;
    private int _size = 9;
    public int size
    {
        get { return _size; }
        set
        {
            if (value < 1)
                _size = 1;
            else
                _size = value;
            decal.size = new Vector3(fixSize * _size, fixSize * _size, 0.1f);
        }
    }

    void Update()
    {
        if (isPainting)
            ChangeTextureOnPoints();
    }

    private void ChangeTextureOnPoints()
    {
        Vector3 pos = transform.position;

        int boardPosX = (int)((pos.z / tool.board.terrainData.size.z) * tool.alphamapWidth);
        int boardPosY = (int)((pos.x / tool.board.terrainData.size.x) * tool.alphamapHeight);
        int layers = tool.board.terrainData.alphamapLayers;

        // Resposible for the behaviour of the square brush
        int sz = size / 2;
        for (int i = -sz; i < sz + 1; i++)
            for (int j = -sz; j < sz + 1; j++)
                for (int k = 0; k < layers; k++)
                {
                    int x = Mathf.Clamp(boardPosX + i, 0, tool.alphamapWidth - 1);
                    int y = Mathf.Clamp(boardPosY + j, 0, tool.alphamapHeight - 1);

                    if (k == tool.currentFloorLayer)
                        tool.alphamap[x, y, k] = 1;
                    else
                        tool.alphamap[x, y, k] = 0;
                }
        tool.board.terrainData.SetAlphamaps(0, 0, tool.alphamap);
    }

    // int layers = tool.board.terrainData.alphamapLayers;

    // map = new float[width, height, layers];

    // for (int y = 0; y < floorTool.board.terrainData.alphamapHeight; y++)
    // {
    //     for (int x = 0; x < floorTool.board.terrainData.alphamapWidth; x++)
    //     {
    //         // Get the normalized terrain coordinate that
    //         // corresponds to the the point.
    //         // float normX = x * 1.0f / (floorTool.board.terrainData.alphamapWidth - 1);
    //         // float normY = y * 1.0f / (floorTool.board.terrainData.alphamapHeight - 1);

    //         // Get the steepness value at the normalized coordinate.
    //         // var angle = floorTool.board.terrainData.GetSteepness(normX, normY);

    //         // Steepness is given as an angle, 0..90 degrees. Divide
    //         // by 90 to get an alpha blending value in the range 0..1.
    //         // var frac = angle / 90.0;
    //         map[x, y, 0] = 1;
    //         map[x, y, 1] = 0;
    //     }
    // }

    // tool.board.terrainData.SetAlphamaps(0, 0, map);
}


