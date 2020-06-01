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
    [SerializeField] private GameObject wallStylesPanel = null;
    [SerializeField] private Transform wallStylesContainer = null;
    [SerializeField] private Transform wallStylesButton = null;
    [SerializeField] private SO_WallStyle defaultWallStyle = null;
    [SerializeField] private SO_WallStyle phantomWallStyle = null;
    [SerializeField] private List<SO_WallStyle> wallStyles = null;

    private SO_WallStyle currentWallStyle = null;
    private List<Transform> phantomKnots = new List<Transform>();
    private List<Transform> phantomWalls = new List<Transform>();
    private List<Vector3> path = new List<Vector3>();
    private Transform pointer = null;

    private List<WallElement> everyWall = new List<WallElement>();
    private List<WallElement> everyKnot = new List<WallElement>();

    void Awake()
    {
        CustomEvents.OnToolChange += OnToolChange;
        PF2E_SceneManager.OnPlaySceneExit += Unsubscribe;
    }

    void Start()
    {
        SetPointer();
        ToolUnselected();
        GenerateWallStyleButtons();
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
                MaterializePhantoms();
            }

            if (isWalling)
            {
                UpdatePointerPos();

                if (Input.GetMouseButtonDown(0))
                    OnPointerDown();
            }
        }
    }

    private void Unsubscribe()
    {
        CustomEvents.OnToolChange -= OnToolChange;
        PF2E_SceneManager.OnPlaySceneExit -= Unsubscribe;
    }

    private void SetPointer()
    {
        pointer = Instantiate(phantomWallStyle.knot, Vector3.zero, Quaternion.identity);
        pointer.GetComponent<BoxCollider>().enabled = false;
        pointer.transform.parent = transform;
        pointer.name = "Wall Pointer";
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

    private void GenerateWallStyleButtons()
    {
        if (wallStyles != null)
            foreach (var item in wallStyles)
            {
                Transform newButton = Instantiate(wallStylesButton, Vector3.zero, Quaternion.identity, wallStylesContainer);
                Button newButtonScript = newButton.GetComponent<Button>();

                newButtonScript.image.sprite = item.buttonSprite;

                newButtonScript.onClick.AddListener(() => SelectWallStyle(item));
            }
    }

    private void SelectWallStyle(SO_WallStyle wall)
    {
        currentWallStyle = wall;
    }

    private void UpdatePointerPos()
    {
        pointer.position = inputManager.MousePosInBoard(true);
    }

    private void ToolSelected()
    {
        isSelected = true;
        isWalling = true;

        toolButton.image.color = Globals.Theme["untrained"];

        wallStylesPanel.SetActive(true);

        UpdatePointerPos();
    }

    private void ToolUnselected()
    {
        isSelected = false;
        isWalling = false;

        toolButton.image.color = Globals.Theme["text_1"];

        pointer.gameObject.SetActive(false);
        wallStylesPanel.SetActive(false);
    }

    public void OnPointerDown()
    {
        if (isWalling)
            PlacePhantom();
    }

    // A "phantom" is a transparent wall or knot
    private void PlacePhantom()
    {
        if (path.Count > 0)
            if (path[path.Count - 1] == pointer.position) // Avoid placing knots in the same position several times
                return;

        if (!path.Contains(pointer.position)) // Avoid spawn 2 knots in the same location
        {
            Collider[] overlaping = Physics.OverlapBox(
                new Vector3(pointer.position.x, pointer.position.y + 2, pointer.position.z),
                new Vector3(0.01f, 0.01f, 0.01f));

            if (overlaping.Length > 0) // Overlaping knots can only happen in diferent wall path placements
                foreach (var item in overlaping)
                {
                    WallElement possibleKnot = item.GetComponent<WallElement>();
                    if (possibleKnot != null)
                        if (possibleKnot.type == E_WallElement.Knot)
                            item.GetComponent<IDestroyable>().Destroy();
                }

            Transform phantomKnot = Instantiate(phantomWallStyle.knot, pointer.position, Quaternion.identity);
            phantomKnots.Add(phantomKnot);
        }

        path.Add(pointer.position);

        for (int i = path.Count - 1; i < path.Count; i++) // Spawn wall except in the first knot placement
            if (i > 0)
            {
                Vector3 direction = path[i] - path[i - 1];
                Vector3 position = direction / 2 + path[i - 1];

                Transform phantomWall = Instantiate(phantomWallStyle.wall, position, Quaternion.identity);
                phantomWall.localScale = new Vector3(phantomWall.localScale.x * direction.magnitude - 0.25f, phantomWall.localScale.y, phantomWall.localScale.z);
                phantomWall.LookAt(path[i], Vector3.up);
                phantomWall.RotateAround(phantomWall.position, Vector3.up, 90);

                phantomWalls.Add(phantomWall);
            }
    }

    private void ClearPhantoms()
    {
        path.Clear();

        foreach (var item in phantomKnots)
            Destroy(item.gameObject);
        phantomKnots.Clear();

        foreach (var item in phantomWalls)
            Destroy(item.gameObject);
        phantomWalls.Clear();
    }

    private void MaterializePhantoms()
    {
        if (phantomKnots.Count < 0) return;

        SO_WallStyle wallStyle;
        if (currentWallStyle != null)
            wallStyle = currentWallStyle;
        else
            wallStyle = defaultWallStyle;

        for (int i = 0; i < phantomKnots.Count; i++)
            InstantiateKnot(wallStyle.knot, phantomKnots[i].position, phantomKnots[i].rotation);
        for (int i = 0; i < phantomWalls.Count; i++)
            InstantiateWall(wallStyle.wall, phantomWalls[i].position, phantomWalls[i].rotation, phantomWalls[i].localScale);

        ClearPhantoms();
    }

    public void GenerateWallElements(Dictionary<string, List<PWallElement>> wallElements)
    {
        foreach (var elementList in wallElements)
        {
            SO_WallStyle wallStyle = wallStyles.Find(ctx => ctx.name == elementList.Key);
            if (wallStyle == null)
                wallStyle = defaultWallStyle;

            foreach (var element in elementList.Value)
                if (element.type == E_WallElement.Knot)
                    InstantiateKnot(wallStyle.knot, element.position, element.rotation);
                else if (element.type == E_WallElement.Wall)
                    InstantiateWall(wallStyle.wall, element.position, element.rotation, element.localScale);
        }
    }

    public Dictionary<string, List<PWallElement>> RetrieveWallElements()
    {
        Dictionary<string, List<PWallElement>> dic = new Dictionary<string, List<PWallElement>>();

        List<WallElement> everyElement = new List<WallElement>();
        everyElement.AddRange(everyKnot);
        everyElement.AddRange(everyWall);

        foreach (var element in everyElement)
        {
            PWallElement pElement = new PWallElement(
            element.name, element.transform.position, element.transform.rotation, element.transform.localScale,
            element.type, element.style.styleName);

            if (dic.ContainsKey(element.style.styleName))
            {
                dic[element.style.styleName].Add(pElement);
            }
            else
            {
                List<PWallElement> list = new List<PWallElement>() { pElement };
                dic.Add(element.style.styleName, list);
            }
        }

        return dic;
    }

    private void InstantiateKnot(Transform original, Vector3 position, Quaternion rotation)
    {
        Transform newKnot = Instantiate(original, position, rotation);

        WallElement newKnotWE = newKnot.GetComponent<WallElement>();
        newKnotWE.wallTool = this;

        everyKnot.Add(newKnotWE);
    }

    private void InstantiateWall(Transform original, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        Transform newWall = Instantiate(original, position, rotation);
        newWall.localScale = localScale;

        WallElement newWallWE = newWall.GetComponent<WallElement>();
        newWallWE.wallTool = this;

        everyWall.Add(newWallWE);
    }

    public void DestroyElement(WallElement element)
    {
        if (element.type == E_WallElement.Knot)
        {
            if (everyKnot.Contains(element))
                everyKnot.Remove(element);
        }
        else if (element.type == E_WallElement.Wall)
        {
            if (everyWall.Contains(element))
                everyWall.Remove(element);
        }
    }
}
