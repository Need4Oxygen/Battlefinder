using UnityEngine;

public class Draggable : MonoBehaviour
{
    [HideInInspector] public bool isClicked = false;

    [Header("Draggable Options")]
    [SerializeField] ESnap snapTo = 0;
    [SerializeField] ERotation canRotate = 0;
    public bool canDuplicate = false;
    [SerializeField] bool centerOnClick = false;
    [SerializeField] float hover = 0.2f;

    private Vector3 clickOffset = Vector3.zero;

    void Update()
    {
        if (isClicked)
        {
            DragWithMouse();
        }
    }

    public void OnSelect()
    {
        isClicked = true;
        if (!centerOnClick)
            clickOffset = transform.position - TablePoint();
    }

    public void OnDeselect()
    {
        isClicked = false;
        Snap();
    }

    /// <summary> Returns a dupplicate of this object. </summary>
    public Draggable Duplicate()
    {
        if (canDuplicate)
        {
            GameObject duplicate = Instantiate(gameObject, transform.position, transform.rotation, null);
            duplicate.name = name;

            Draggable draggable = duplicate.GetComponent<Draggable>();
            return draggable;
        }
        else
        {
            return null;
        }
    }

    /// <summary> Snap the object to grid. </summary>
    public void Snap()
    {
        if (snapTo == ESnap.None)
            return;
        else
            Snap(snapTo);
    }
    public void Snap(ESnap snapTo)
    {
        float x = 0f, z = 0f;

        if (snapTo == ESnap.Line)
        {
            x = Mathf.Round(transform.localPosition.x);
            z = Mathf.Round(transform.localPosition.z);
        }
        else
        {
            x = Mathf.Round(transform.localPosition.x - (1f / 2f)) + 1f / 2f;
            z = Mathf.Round(transform.localPosition.z - (1f / 2f)) + 1f / 2f;
        }

        transform.localPosition = new Vector3(x, transform.localPosition.y, z);
    }

    private void Rotate(Vector3 tablePoint)
    {
        if (canRotate == ERotation.None)
            return;
        else if (canRotate == ERotation._90º)
            transform.RotateAround(tablePoint, Vector3.up, 90);
        else if (canRotate == ERotation._45º)
            transform.RotateAround(tablePoint, Vector3.up, 45);
    }

    private void DragWithMouse()
    {
        Vector3 tablePoint = TablePoint();

        if (Input.GetKeyUp(KeyCode.R))
            Rotate(tablePoint);

        // Centra o no el objeto al clickar
        transform.position = tablePoint + clickOffset;
        transform.position = new Vector3(transform.position.x, hover, transform.position.z);
    }

    /// <summary> Returns point resulting from colliding ray from mouse to table. </summary>
    private Vector3 TablePoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20f, DraggablesMaster.Board))
            return hit.point;
        else
            return Vector3.zero;
    }
}
