using System.Collections;
using System.Collections.Generic;
using BoardItems;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class SelectionTool : MonoBehaviour
{
    [HideInInspector] public bool isSelected = false;

    [SerializeField] private Button toolButton = null;
    [SerializeField] private LineRenderer line = null;
    [SerializeField] private float lineHover = 0.02f;

    public static List<ISelectable> SelectedItems = new List<ISelectable>();

    private Vector3 firstPoint = Vector3.zero;
    private Vector3 lastPoint = Vector3.zero;
    private bool isSelecting = false;
    private Coroutine selectCoroutine = null;

    void Awake()
    {
        CustomEvents.OnToolChange += OnToolChange;
        PF2E_SceneManager.OnPlaySceneExit += Unsubscribe;
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
    }

    private void Unsubscribe()
    {
        CustomEvents.OnToolChange -= OnToolChange;
        PF2E_SceneManager.OnPlaySceneExit -= Unsubscribe;
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

    public void DeselectAll()
    {
        foreach (var item in SelectedItems)
            item.Select(false);
        SelectedItems.Clear();
    }

    public void OnPointerDown()
    {
        DeselectAll();

        Vector3 point = InputManager.TablePoint(false, lineHover);
        firstPoint = point;
        lastPoint = point;

        selectCoroutine = StartCoroutine(SelectCorou());

        line.enabled = true;
    }

    public void OnPointerUp()
    {
        line.enabled = false;

        StopCoroutine(selectCoroutine);
        selectCoroutine = null;

        Vector3 center = (lastPoint - firstPoint) / 2 + firstPoint;
        center = new Vector3(center.x, 1f, center.z);
        Vector3 halfExt = new Vector3(Mathf.Abs(firstPoint.x - lastPoint.x) / 2, 1f, Mathf.Abs(firstPoint.z - lastPoint.z) / 2);

        Collider[] cols = Physics.OverlapBox(center, halfExt, Quaternion.identity, InputManager.SelectablesLayer);

        if (cols != null)
            if (cols.Length > 0)
                foreach (var item in cols)
                {
                    ISelectable selectable = item.GetComponent<ISelectable>();
                    if (selectable != null)
                    {
                        selectable.Select(true);
                        SelectedItems.Add(selectable);
                    }
                }
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

}
