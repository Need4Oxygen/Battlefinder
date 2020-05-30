using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallTool : MonoBehaviour
{
    [HideInInspector] public bool isSelected = false;
    [HideInInspector] public bool isWalling = false;

    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private Button toolButton = null;

    [Space(15)]
    [SerializeField] private GameObject wallTypesPanel = null;
    [SerializeField] private Transform wallTypesContainer = null;
    [SerializeField] private Transform wallTypesButton = null;
    [SerializeField] private SO_Wall defaultWallType = null;
    [SerializeField] private SO_Wall phantomWallType = null;
    [SerializeField] private List<SO_Wall> wallTypes = null;

    private SO_Wall currentWallType = null;
    private List<Transform> phantomKnots = new List<Transform>();
    private List<Transform> phantomWalls = new List<Transform>();
    private Transform pointer = null;

    void Awake()
    {
        CustomEvents.OnToolChange += OnToolChange;
    }

    void Start()
    {
        pointer = Instantiate(phantomWallType.knot, Vector3.zero, Quaternion.identity);
        ToolUnselected();
        GenerateWallTypeButtons();
    }

    void Update()
    {
        if (isSelected)
        {
            if (Input.GetButtonDown("Walls"))
            {
                isWalling = true;
                pointer.gameObject.SetActive(true);
            }
            if (Input.GetButtonUp("Walls"))
            {
                isWalling = false;
                pointer.gameObject.SetActive(false);
                GenerateWall();
            }

            if (isWalling)
            {
                UpdatePointerPos();

                if (Input.GetMouseButtonDown(0))
                    OnPointerDown();
            }
        }
    }

    private void OnToolChange(E_Tools tool)
    {
        if (tool == E_Tools.Walls && !isSelected)
        {
            ToolSelected();
        }
        else if (tool != E_Tools.Walls && isSelected)
        {
            ToolUnselected();
        }
    }

    private void GenerateWallTypeButtons()
    {
        if (wallTypes != null)
            foreach (var item in wallTypes)
            {
                Transform newButton = Instantiate(wallTypesButton, Vector3.zero, Quaternion.identity, wallTypesContainer);
                Button newButtonScript = newButton.GetComponent<Button>();

                newButtonScript.image.sprite = item.buttonSprite;

                newButtonScript.onClick.AddListener(() => SelectWallType(item));
            }
    }

    private void SelectWallType(SO_Wall wall)
    {
        currentWallType = wall;
    }

    private void UpdatePointerPos()
    {
        pointer.position = inputManager.MousePosInBoard(true);
    }

    private void ToolSelected()
    {
        isSelected = true;
        isWalling = true;

        toolButton.image.color = Color.red;

        wallTypesPanel.SetActive(true);

        UpdatePointerPos();
    }

    private void ToolUnselected()
    {
        isSelected = false;
        isWalling = false;

        toolButton.image.color = Color.white;

        pointer.gameObject.SetActive(false);
        wallTypesPanel.SetActive(false);
    }

    public void OnPointerDown()
    {
        if (isWalling)
            PlacePhantom();
    }

    // A "phantom" is a transparent wall or knot
    private void PlacePhantom()
    {
        Transform phantomKnot = Instantiate(phantomWallType.knot, pointer.position, Quaternion.identity);
        phantomKnots.Add(phantomKnot);

        for (int i = phantomKnots.Count - 1; i < phantomKnots.Count; i++)
        {
            if (i > 0)
            {
                Vector3 direction = phantomKnots[i].position - phantomKnots[i - 1].position;
                Vector3 position = direction / 2 + phantomKnots[i - 1].position;

                Transform phantomWall = Instantiate(phantomWallType.wall, position, Quaternion.identity);
                phantomWall.localScale = new Vector3(phantomWall.localScale.x * direction.magnitude, phantomWall.localScale.y, phantomWall.localScale.z);
                phantomWall.LookAt(phantomKnots[i].position, Vector3.up);
                phantomWall.RotateAround(phantomWall.position, Vector3.up, 90);

                phantomWalls.Add(phantomWall);
            }
        }
    }

    private void ClearPhantoms()
    {
        foreach (var item in phantomKnots)
            Destroy(item.gameObject);
        phantomKnots.Clear();

        foreach (var item in phantomWalls)
            Destroy(item.gameObject);
        phantomWalls.Clear();
    }

    private void GenerateWall()
    {
        if (phantomKnots.Count < 0) return;

        SO_Wall wallType;
        if (currentWallType != null)
            wallType = currentWallType;
        else
            wallType = defaultWallType;

        for (int i = 0; i < phantomKnots.Count; i++)
        {
            Instantiate(wallType.knot, phantomKnots[i].position, phantomKnots[i].rotation);
        }
        for (int i = 0; i < phantomWalls.Count; i++)
        {
            Transform newWall = Instantiate(wallType.wall, phantomWalls[i].position, phantomWalls[i].rotation) as Transform;
            newWall.localScale = phantomWalls[i].localScale;
        }

        ClearPhantoms();
    }
}
