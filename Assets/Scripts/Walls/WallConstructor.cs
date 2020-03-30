using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallConstructor : MonoBehaviour
{
    [HideInInspector] public bool isWalling = false;

    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private Button toolButton = null;
    [SerializeField] private LineRenderer lineRenderer = null;

    [Space(15)]
    [SerializeField] private Transform pointerPrefab = null;
    [SerializeField] private Transform landMarkPrefab = null;
    [SerializeField] private float landMarkLineHeight = 0.3f;

    [Space(15)]
    [SerializeField] private Transform knot = null;
    [SerializeField] private Transform wall = null;

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
        if (isWalling)
        {
            if (Input.GetMouseButtonDown(0))
                OnPointerDown();
            if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Walls"))
                GenerateWall();

            UpdatePointerPos();
        }
    }

    private void OnToolChange(ETools tool)
    {
        if (tool == ETools.Walls && !isWalling)
        {
            StartWalling();
        }
        else if (tool != ETools.Walls && isWalling)
        {
            StopWalling();
        }
    }

    private void UpdatePointerPos()
    {
        pointer.position = inputManager.MousePosInBoard(true);
    }

    private void StartWalling()
    {
        isWalling = true;
        toolButton.image.color = Globals.iconsSelectedColorTemp;
        UpdatePointerPos();
        pointer.gameObject.SetActive(true);
    }

    private void StopWalling()
    {
        isWalling = false;
        toolButton.image.color = Globals.iconsColorTemp;
        pointer.gameObject.SetActive(false);

        GenerateWall();
    }

    private void GenerateWall()
    {
        if (landMarkList.Count < 0) return;

        for (int i = 0; i < landMarkList.Count; i++)
        {
            Instantiate(knot, landMarkList[i].position, Quaternion.identity);
            if (i > 0)
            {
                Vector3 direction = landMarkList[i].position - landMarkList[i - 1].position;
                Vector3 position = direction / 2 + landMarkList[i - 1].position;

                Transform newWall = Instantiate(wall, position, Quaternion.identity);
                newWall.localScale = new Vector3(newWall.localScale.x * direction.magnitude, newWall.localScale.y, newWall.localScale.z);
                newWall.LookAt(landMarkList[i].position, Vector3.up);
                newWall.RotateAround(newWall.position, Vector3.up, 90);
            }
        }

        List<Vector2> vertices = new List<Vector2>();
        for (int i = 0; i < landMarkList.Count; i++)
            vertices.Add(new Vector2(landMarkList[i].position.x, landMarkList[i].position.z));

        ClearLandMarksAndLine();
    }

    public void OnPointerDown()
    {
        PlaceLandMark();
        RedrawLine();
    }

    private void PlaceLandMark()
    {
        Transform newLandMark = Instantiate(landMarkPrefab, pointer.position, Quaternion.identity);
        landMarkList.Add(newLandMark);
    }

    private void RedrawLine()
    {
        Vector3[] array = new Vector3[landMarkList.Count];
        for (int i = 0; i < landMarkList.Count; i++)
            array[i] = new Vector3(landMarkList[i].position.x, landMarkLineHeight, landMarkList[i].position.z);

        lineRenderer.positionCount = array.Length;
        lineRenderer.SetPositions(array);
    }

    private void ClearLandMarksAndLine()
    {
        foreach (var item in landMarkList)
            Destroy(item.gameObject);
        landMarkList.Clear();
        lineRenderer.positionCount = 0;
    }
}
