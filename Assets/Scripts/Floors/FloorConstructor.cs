using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorConstructor : MonoBehaviour
{

    [HideInInspector] public bool isFlooring = false;

    [SerializeField] private Camera cam = null;
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

    void Start()
    {
        pointer = Instantiate(pointerPrefab, Vector3.zero, Quaternion.identity);
        pointer.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isFlooring)
            Flooring();
    }

    public void StartFlooring()
    {
        isFlooring = true;

        pointer.gameObject.SetActive(true);
    }

    public void StopFlooring()
    {
        isFlooring = false;
        pointer.gameObject.SetActive(false);

        List<Vector2> vertices = GenerateVerticesFromLandMarks();

        if (landMarkList.Count >= 3)
            GenerateFloor(vertices);

        ClearLandMarksAndLine();
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

    private void Flooring()
    {
        cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Vector3 closestRoundedPos = new Vector3(
                Mathf.Round(hit.point.x), 0f,
                Mathf.Round(hit.point.z));

            pointer.position = closestRoundedPos;
        }
    }

    public void GenerateFloor(List<Vector2> vertices2D)
    {
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

    public void OnPointerDown()
    {
        PlaceLandMark();
        if (landMarkList.Count >= 3)
            RedrawLine();
    }

    public void OnPointerUp()
    {
        // throw new NotImplementedException();
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
