using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorConstructor : MonoBehaviour
{
    [HideInInspector] public bool isFlooring = false;

    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private Button toolButton = null;
    [SerializeField] private LineRenderer lineRenderer = null;

    [Space(15)]
    [SerializeField] private Transform pointerPrefab = null;
    [SerializeField] private Transform landMarkPrefab = null;
    [SerializeField] private float landMarkLineHeight = 0.3f;

    [Space(15)]
    [SerializeField] public Transform floorPrefab;
    [SerializeField] public Terrain board;

    private List<Transform> landMarkList = new List<Transform>();
    private Transform pointer = null;

    void Awake()
    {
        CustomEvents.OnToolChange += OnToolChange;
    }

    void Start()
    {
        pointer = Instantiate(pointerPrefab, Vector3.zero, Quaternion.identity);
        pointer.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isFlooring)
        {
            if (Input.GetMouseButtonDown(0))
                OnPointerDown();
            if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Floors"))
                GenerateFloor();

            UpdatePointerPos();
        }
    }

    private void OnToolChange(E_Tools tool)
    {
        if (tool == E_Tools.Floors && !isFlooring)
        {
            StartFlooring();
        }
        else if (tool != E_Tools.Floors && isFlooring)
        {
            StopFlooring();
        }
    }

    private void UpdatePointerPos()
    {
        pointer.position = inputManager.MousePosInBoard(true);
    }

    private void StartFlooring()
    {
        isFlooring = true;
        toolButton.image.color = Color.red;
        UpdatePointerPos();
        pointer.gameObject.SetActive(true);
    }

    private void StopFlooring()
    {
        isFlooring = false;
        toolButton.image.color = Color.magenta;
        pointer.gameObject.SetActive(false);

        GenerateFloor();
    }

    private List<Vector2> GenerateVerticesFromLandMarks()
    {
        List<Vector2> vertices = new List<Vector2>();

        for (int i = 0; i < landMarkList.Count; i++)
            vertices.Add(new Vector2(Mathf.RoundToInt(landMarkList[i].position.x), Mathf.RoundToInt(landMarkList[i].position.z)));

        if (vertices[0] == vertices[vertices.Count - 1])
            vertices.RemoveAt(vertices.Count - 1);

        return vertices;
    }

    public void GenerateFloor()
    {
        if (landMarkList.Count < 3) return;

        List<Vector2> vertices2D = GenerateVerticesFromLandMarks();

        Vector2[] v = vertices2D.ToArray();
        Mesh newFloorMesh = GenerateMesh(v);

        if (newFloorMesh.normals[0].y > 0) // If polygon is facing up
        {
            Transform newFloor = Instantiate(floorPrefab, transform.position, Quaternion.identity);
            Floor newFloorScript = newFloor.GetComponent<Floor>();

            newFloorScript.mesh.mesh = newFloorMesh;
            newFloorScript.meshCollider.sharedMesh = newFloorMesh;
        }
        else
        {
            Debug.LogWarning("[FloorConstructor] Cloudn't generate floor, polygon facing down!");
        }

        ClearLandMarksAndLine();
    }

    public Mesh GenerateMesh(Vector2[] vertices2D)
    {
        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = new Vector3(vertices2D[i].x, 0, vertices2D[i].y);

        // Create the mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Set up game object with mesh;
        return mesh;
    }

    private void OnPointerDown()
    {
        PlaceLandMark();
        if (landMarkList.Count >= 3)
            RedrawLine();
    }

    private void PlaceLandMark()
    {
        Transform newLandMark = Instantiate(landMarkPrefab, pointer.position, Quaternion.identity);
        landMarkList.Add(newLandMark);
    }

    private void RedrawLine()
    {
        List<Vector3> list = new List<Vector3>();

        for (int i = 0; i < landMarkList.Count; i++)
            list.Add(new Vector3(landMarkList[i].position.x, landMarkLineHeight, landMarkList[i].position.z));
        list.Add(new Vector3(landMarkList[0].position.x, landMarkLineHeight, landMarkList[0].position.z));

        lineRenderer.positionCount = list.Count;
        lineRenderer.SetPositions(list.ToArray());
    }

    private void ClearLandMarksAndLine()
    {
        foreach (var item in landMarkList)
            Destroy(item.gameObject);
        landMarkList.Clear();
        lineRenderer.positionCount = 0;
    }
}
