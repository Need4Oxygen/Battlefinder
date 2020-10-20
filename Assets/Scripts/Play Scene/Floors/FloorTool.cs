using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloorTool : MonoBehaviour
{
    [HideInInspector] public bool isSelected = false;
    [HideInInspector] public int currentFloorLayer = 0;

    [SerializeField] private FloorBrush brush = null;
    [SerializeField] public Terrain board = null;
    [SerializeField] private Button toolButton = null;

    [Space(15)]
    [SerializeField] private GameObject floorTypesPanel = null;
    [SerializeField] private Transform floorTypesContainer = null;
    [SerializeField] private Transform floorTypesButton = null;
    [SerializeField] private List<SO_Floor> floorTypes = null;

    [HideInInspector] public int alphamapWidth;
    [HideInInspector] public int alphamapHeight;
    [HideInInspector] public int alphamapLayers;
    [HideInInspector] public int heightmapRes;

    [HideInInspector] public float[,,] alphamap;
    [HideInInspector] public float[,] heightmap;

    void Awake()
    {
        CustomEvents.OnToolChange += OnToolChange;
        SceneManager_PF2E.OnPlaySceneExit += Unsubscribe;
    }

    void Start()
    {
        brush.gameObject.SetActive(false);
        ToolUnselected();
        GenerateFloorTypeButtons();
    }

    void Update()
    {
        if (isSelected)
        {
            if (!MouseInputUIBlocker.BlockedByUI)
            {
                if (Input.GetMouseButtonDown(0))
                    OnPointerDown();
                if (Input.GetMouseButtonUp(0))
                    OnPointerUp();
            }

            if (Input.GetKeyDown(KeyCode.KeypadPlus))
                brush.size += 2;
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
                brush.size -= 2;

            UpdateBrushPos();
        }
    }

    private void Unsubscribe()
    {
        CustomEvents.OnToolChange -= OnToolChange;
        SceneManager_PF2E.OnPlaySceneExit -= Unsubscribe;
    }

    private void OnToolChange(E_Tools tool)
    {
        if (tool == E_Tools.Floors && !isSelected)
        {
            ToolSelected();
        }
        else if (tool != E_Tools.Floors && isSelected)
        {
            ToolUnselected();
        }
    }

    private void ToolSelected()
    {
        isSelected = true;

        toolButton.image.color = Globals.Theme["untrained"];

        brush.gameObject.SetActive(true);
        floorTypesPanel.SetActive(true);

        UpdateBrushPos();
    }

    private void ToolUnselected()
    {
        isSelected = false;

        toolButton.image.color = Globals.Theme["text_1"];

        brush.gameObject.SetActive(false);
        floorTypesPanel.SetActive(false);
    }

    private void UpdateBrushPos()
    {
        brush.transform.position = InputManager.TablePoint(false);
    }

    private void OnPointerDown()
    {
        brush.isPainting = true;
    }

    private void OnPointerUp()
    {
        brush.isPainting = false;
    }

    private void GenerateFloorTypeButtons()
    {
        if (floorTypes != null)
            foreach (var item in floorTypes)
            {
                Transform newButton = Instantiate(floorTypesButton, Vector3.zero, Quaternion.identity, floorTypesContainer);
                Button newButtonScript = newButton.GetComponent<Button>();

                newButtonScript.image.sprite = item.buttonSprite;

                newButtonScript.onClick.AddListener(() => SelectFloorType(item));
            }
    }

    private void SelectFloorType(SO_Floor floor)
    {
        if (floor.terrainLayer <= board.terrainData.alphamapLayers - 1)
            currentFloorLayer = floor.terrainLayer;
        else
            currentFloorLayer = 0;
    }

    public void SetAlphaMaps()
    {
        board.terrainData.SetAlphamaps(0, 0, alphamap);
    }

    public void ResetAlphaMaps()
    {
        alphamapHeight = board.terrainData.alphamapHeight;
        alphamapWidth = board.terrainData.alphamapWidth;
        alphamapLayers = board.terrainData.alphamapLayers;

        alphamap = new float[alphamapWidth, alphamapHeight, alphamapLayers];

        for (int i = 0; i < alphamapWidth; i++)
            for (int j = 0; j < alphamapHeight; j++)
                for (int k = 0; k < alphamapLayers; k++)
                    if (k == 0)
                        alphamap[i, j, k] = 1;
                    else
                        alphamap[i, j, k] = 0;

        board.terrainData.SetAlphamaps(0, 0, alphamap);
    }

    public void SetHeightMaps()
    {
        board.terrainData.SetHeights(0, 0, heightmap);
    }

    public void ResetHeightMaps()
    {
        heightmapRes = board.terrainData.heightmapResolution;

        heightmap = new float[heightmapRes, heightmapRes];

        for (int i = 0; i < heightmapRes; i++)
            for (int j = 0; j < heightmapRes; j++)
                heightmap[i, j] = 0.5f;

        board.terrainData.SetHeights(0, 0, heightmap);
    }

    // This code may be used when heights are implemented


    // Get the normalized terrain coordinate that corresponds to the the point.
    // float normX = x * 1.0f / (floorTool.board.terrainData.alphamapWidth - 1);
    // float normY = y * 1.0f / (floorTool.board.terrainData.alphamapHeight - 1);

    // Get the steepness value at the normalized coordinate.
    // var angle = floorTool.board.terrainData.GetSteepness(normX, normY);

    // Steepness is given as an angle, 0..90 degrees. Divide by 90 to get an alpha blending value in the range 0..1.
    // var frac = angle / 90.0;


    // This code may be used when wall enclosures are implemented


    //     private List<Vector2> GenerateVerticesFromLandMarks()
    //     {
    //         List<Vector2> vertices = new List<Vector2>();

    //         for (int i = 0; i < landMarkList.Count; i++)
    //             vertices.Add(new Vector2(Mathf.RoundToInt(landMarkList[i].position.x), Mathf.RoundToInt(landMarkList[i].position.z)));

    //         if (vertices[0] == vertices[vertices.Count - 1])
    //             vertices.RemoveAt(vertices.Count - 1);

    //         return vertices;
    //     }

    //     public void GenerateFloor()
    //     {
    //         if (landMarkList.Count < 3) return;

    //         List<Vector2> vertices2D = GenerateVerticesFromLandMarks();

    //         Vector2[] v = vertices2D.ToArray();
    //         Mesh newFloorMesh = GenerateMesh(v);

    //         if (newFloorMesh.normals[0].y > 0) // If polygon is facing up
    //         {
    //             Transform newFloor = Instantiate(floorPrefab, transform.position, Quaternion.identity);
    //             Floor newFloorScript = newFloor.GetComponent<Floor>();

    //             newFloorScript.mesh.mesh = newFloorMesh;
    //             newFloorScript.meshCollider.sharedMesh = newFloorMesh;
    //         }
    //         else
    //         {
    //             Debug.LogWarning("[FloorConstructor] Cloudn't generate floor, polygon facing down!");
    //         }

    //         ClearLandMarksAndLine();
    //     }

    //     public Mesh GenerateMesh(Vector2[] vertices2D)
    //     {
    //         // Use the triangulator to get indices for creating triangles
    //         Triangulator tr = new Triangulator(vertices2D);
    //         int[] indices = tr.Triangulate();

    //         // Create the Vector3 vertices
    //         Vector3[] vertices = new Vector3[vertices2D.Length];
    //         for (int i = 0; i < vertices.Length; i++)
    //             vertices[i] = new Vector3(vertices2D[i].x, 0, vertices2D[i].y);

    //         // Create the mesh
    //         Mesh mesh = new Mesh();
    //         mesh.vertices = vertices;
    //         mesh.triangles = indices;
    //         mesh.RecalculateNormals();
    //         mesh.RecalculateBounds();

    //         // Set up game object with mesh;
    //         return mesh;
    //     }

    //     private void PlaceLandMark()
    //     {
    //         Transform newLandMark = Instantiate(landMarkPrefab, brush.position, Quaternion.identity);
    //         landMarkList.Add(newLandMark);
    //     }

    //     private void RedrawLine()
    //     {
    //         List<Vector3> list = new List<Vector3>();

    //         for (int i = 0; i < landMarkList.Count; i++)
    //             list.Add(new Vector3(landMarkList[i].position.x, landMarkLineHeight, landMarkList[i].position.z));
    //         list.Add(new Vector3(landMarkList[0].position.x, landMarkLineHeight, landMarkList[0].position.z));

    //         lineRenderer.positionCount = list.Count;
    //         lineRenderer.SetPositions(list.ToArray());
    //     }

    //     private void ClearLandMarksAndLine()
    //     {
    //         foreach (var item in landMarkList)
    //             Destroy(item.gameObject);
    //         landMarkList.Clear();
    //         lineRenderer.positionCount = 0;
    //     }
}
