using UnityEngine;

public class Draggable : MonoBehaviour
{
    [HideInInspector] public bool isClicked = false;

    [Header("Draggable Options")]
    [SerializeField] ESnap snapTo = 0;
    [SerializeField] ERotation canRotate = 0;
    [SerializeField] bool canScale = false;
    public bool canDuplicate = false;
    [SerializeField] bool centerOnClick = false;
    [SerializeField] float hover = 0.2f;

    private Vector3 clickOffset = Vector3.zero;

    protected virtual void Update()
    {
        if (isClicked)
            DragWithMouse();
    }

    public virtual void OnSelect()
    {
        isClicked = true;
        if (!centerOnClick)
            clickOffset = transform.position - TablePoint();
    }

    public virtual void OnDeselect()
    {
        isClicked = false;
        Snap();
    }

    /// <summary> Returns a dupplicate of this object. </summary>
    public virtual Draggable Duplicate()
    {
        if (canDuplicate)
        {
            GameObject duplicate = Instantiate(gameObject, transform.position, transform.rotation, null);
            duplicate.name = name + " +";

            Draggable draggable = duplicate.GetComponent<Draggable>();
            return draggable;
        }
        else
        {
            return null;
        }
    }

    /// <summary> Snap the object to grid. </summary>
    public virtual void Snap()
    {
        if (snapTo == ESnap.None)
            return;
        else
            Snap(snapTo);
    }
    public virtual void Snap(ESnap snapTo)
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

    /// <summary> Rotate the object. </summary>
    protected virtual void Rotate(Vector3 tablePoint)
    {
        if (canRotate == ERotation.None)
            return;
        else if (canRotate == ERotation._90º)
            transform.RotateAround(tablePoint, Vector3.up, 90);
        else if (canRotate == ERotation._45º)
            transform.RotateAround(tablePoint, Vector3.up, 45);
    }

    /// <summary> Scale the object to given scale. </summary>
    protected virtual void Scale(int scale)
    {
        transform.localScale = new Vector3(scale, transform.localScale.y, scale);

        if (scale % 2 == 0) // If even
            snapTo = ESnap.Line;
        else
            snapTo = ESnap.Slot;
    }

    protected virtual void DragWithMouse()
    {
        Vector3 tablePoint = TablePoint();

        if (Input.GetKeyUp(KeyCode.R))
            Rotate(tablePoint);
        else if (canScale)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                Scale(1);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                Scale(2);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                Scale(3);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                Scale(4);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                Scale(5);
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                Scale(6);
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                Scale(7);
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                Scale(8);
            else if (Input.GetKeyDown(KeyCode.Alpha9))
                Scale(9);
        }

        // Centra o no el objeto al clickar
        transform.position = tablePoint + clickOffset;
        transform.position = new Vector3(transform.position.x, hover, transform.position.z);
    }

    /// <summary> Returns point resulting from colliding ray from mouse to table. </summary>
    protected virtual Vector3 TablePoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20f, DraggablesMaster.Board))
            return hit.point;
        else
            return Vector3.zero;
    }
}
