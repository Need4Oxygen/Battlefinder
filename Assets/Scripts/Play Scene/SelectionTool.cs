using System.Collections;
using System.Collections.Generic;
using BoardItems;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class SelectionTool : MonoBehaviour
{
    public static List<ISelectable> SelectedItems = new List<ISelectable>();

    [HideInInspector] public bool isSelected = false;

    [SerializeField] private Button toolButton = null;
    [SerializeField] private LineRenderer line = null;
    [SerializeField] private float lineHover = 0.02f;

    private Vector3 firstPoint = Vector3.zero;
    private Vector3 lastPoint = Vector3.zero;
    private Coroutine selectCoroutine = null;

    void Awake()
    {
        CustomEvents.OnToolChange += OnToolChange;
        SceneManager_PF2E.OnPlaySceneExit += Unsubscribe;
    }

    void Update()
    {
        if (isSelected)
        {
            if (Input.GetMouseButtonDown(0))
                OnPointerDown();
            if (Input.GetMouseButtonUp(0))
                OnPointerUp();
        }

        if (Input.GetKeyDown(KeyCode.D))
            if (Input.GetKey(KeyCode.LeftControl))
                DeselectAll();

        if (Input.GetKeyDown(KeyCode.R))
        {
            int count = SelectedItems.Count;
            if (count > 0)
            {
                if (count == 1)
                    Rotation();
                else
                    BulkRotation();
            }
        }
    }

    private void Unsubscribe()
    {
        CustomEvents.OnToolChange -= OnToolChange;
        SceneManager_PF2E.OnPlaySceneExit -= Unsubscribe;
    }

    private void OnToolChange(E_Tools tool)
    {
        if (tool == E_Tools.Selection && !isSelected)
        {
            ToolSelected();
        }
        else if (tool != E_Tools.Selection && isSelected)
        {
            ToolUnselected();
        }
    }

    private void ToolSelected()
    {
        isSelected = true;
        toolButton.image.color = Globals.Theme["untrained"];
    }

    private void ToolUnselected()
    {
        isSelected = false;
        toolButton.image.color = Globals.Theme["text_1"];
    }

    private void OnPointerDown()
    {
        Vector3 point = InputManager.TablePoint(false, lineHover);
        firstPoint = point;
        lastPoint = point;

        selectCoroutine = StartCoroutine(SelectCorou());

        line.enabled = true;
    }

    private void OnPointerUp()
    {
        line.enabled = false;

        // Get modifiers
        bool altMod = Input.GetKey(KeyCode.LeftAlt);
        bool ctrMod = Input.GetKey(KeyCode.LeftControl);

        if (!ctrMod && !altMod)
            DeselectAll();

        // Stop defining search area
        StopCoroutine(selectCoroutine);
        selectCoroutine = null;

        // Define collision box
        Vector3 center = (lastPoint - firstPoint) / 2 + firstPoint; center = new Vector3(center.x, 1f, center.z);
        Vector3 halfExt = new Vector3(Mathf.Abs(firstPoint.x - lastPoint.x) / 2, 1f, Mathf.Abs(firstPoint.z - lastPoint.z) / 2);

        // Search for selectables
        Collider[] cols = Physics.OverlapBox(center, halfExt, Quaternion.identity, InputManager.SelectablesLayer);
        List<ISelectable> selection = new List<ISelectable>();

        if (cols.Length > 0)
        {
            foreach (var item in cols)
            {
                ISelectable selectable = item.GetComponent<ISelectable>();
                if (selectable != null)
                    selection.Add(selectable);
            }
        }
        else
        {
            ISelectable selectable = SearchSelectableUnderMouse();
            if (selectable != null)
                selection.Add(selectable);
        }

        // Do things with selected things
        if (selection.Count > 0)
            foreach (var item in selection)
                if (!altMod)
                    Select(item);
                else
                    Deselect(item);
    }

    private IEnumerator SelectCorou()
    {
        while (true)
        {
            Vector3[] positions = new Vector3[]{
                firstPoint,
                new Vector3(firstPoint.x,lineHover,lastPoint.z),
                lastPoint,
                new Vector3(lastPoint.x,lineHover,firstPoint.z),
                firstPoint};

            lastPoint = InputManager.TablePoint(false, lineHover);

            line.SetPositions(positions);

            yield return null;
        }
    }

    private void Rotation()
    {
        foreach (var item in SelectedItems)
        {
            IRotable rotable = item as IRotable;
            if (rotable != null)
                rotable.Rotate(InputManager.TablePoint(true), 90);
        }
    }

    private void BulkRotation()
    {
        // If they are moving, they should rotate around mouse
        // If they are not, they should rotate around mass centre
        foreach (var item in SelectedItems)
        {
            IBulkRotable rotable = item as IBulkRotable;
            if (rotable != null)
                rotable.BulkRotate(InputManager.TablePoint(true), 90);
        }
    }

    // Select logic global

    public static void Select(ISelectable selectable)
    {
        selectable.Select(true);
        SelectedItems.Add(selectable);
    }

    public static void Deselect(ISelectable selectable)
    {
        selectable.Select(false);
        SelectedItems.Remove(selectable);
    }

    public static void DeselectAll()
    {
        foreach (var item in SelectedItems)
            item.Select(false);
        SelectedItems.Clear();
    }

    public static ISelectable SearchSelectableUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, InputManager.SelectablesLayer))
        {
            ISelectable selectable = hit.collider.GetComponent<ISelectable>();
            if (selectable != null)
                return selectable;
        }
        return null;
    }

}
