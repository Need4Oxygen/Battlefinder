using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class FloorBrush : MonoBehaviour
{
    [SerializeField] private FloorTool floorTool = null;
    [SerializeField] private DecalProjector decal = null;

    [SerializeField] public float brushIncrement = 0.25f;

    [HideInInspector] public bool isPainting = false;
    [HideInInspector] public int layerToPaint = 0;

    private float fixedSize = 1.04f;
    private float[,,] map;

    private float _size = 1;
    public float size
    {
        get { return _size; }
        set
        {
            Debug.Log(floorTool.board.terrainData.alphamapLayers);
            decal.size = new Vector3(fixedSize * _size, fixedSize * _size, 0.1f);
        }
    }

    void Awake()
    {
        SetMap();
    }

    void Update()
    {
        if (isPainting)
        {
            ChangeTextureOnPoints();
        }
    }

    void SetMap()
    {
        map = new float[floorTool.board.terrainData.alphamapWidth, floorTool.board.terrainData.alphamapHeight, floorTool.board.terrainData.alphamapLayers];

        Debug.Log(floorTool.board.terrainData.alphamapLayers);

        // For each point on the alphamap...
        for (int y = 0; y < floorTool.board.terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < floorTool.board.terrainData.alphamapWidth; x++)
            {
                // Get the normalized terrain coordinate that
                // corresponds to the the point.
                // float normX = x * 1.0f / (floorTool.board.terrainData.alphamapWidth - 1);
                // float normY = y * 1.0f / (floorTool.board.terrainData.alphamapHeight - 1);

                // Get the steepness value at the normalized coordinate.
                // var angle = floorTool.board.terrainData.GetSteepness(normX, normY);

                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                // var frac = angle / 90.0;
                map[x, y, 0] = 1;
                map[x, y, 1] = 0;
            }
        }

        floorTool.board.terrainData.SetAlphamaps(0, 0, map);
    }

    void ChangeTextureOnPoints()
    {
        Vector3 pos = transform.position;

        int terrainPosX = (int)((pos.z / floorTool.board.terrainData.size.z) * floorTool.board.terrainData.alphamapWidth);
        int terrainPosY = (int)((pos.x / floorTool.board.terrainData.size.x) * floorTool.board.terrainData.alphamapHeight);

        int brushWidth = 2;
        int brushHeight = 2;

        for (int i = -1; i < brushWidth; i++)
            for (int j = -1; j < brushHeight; j++)
            {
                map[terrainPosX + i, terrainPosY + j, 0] = 0;
                map[terrainPosX + i, terrainPosY + j, 1] = 1;
            }

        floorTool.board.terrainData.SetAlphamaps(0, 0, map);
    }
}
