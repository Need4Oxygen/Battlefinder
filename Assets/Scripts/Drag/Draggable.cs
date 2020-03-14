using UnityEngine;

public class Draggable : MonoBehaviour
{
    enum SnapTo { None, Slot, Line }

    [HideInInspector]
    public bool isClicked;

    [Header("Options")]
    [SerializeField] SnapTo snapTo = 0;
    [SerializeField] float hover = 0.2f;
    [Space(15)]
    [SerializeField] bool centerOnClick = false;
    [SerializeField] bool canDuplicate = false;
    [SerializeField] bool canRotate = false;

    Vector3 clickOffset = Vector3.zero;

    void Start()
    {
        Invoke("Snap", 0.1f);
    }

    void Update()
    {
        if (isClicked)
        {
            Vector3 tablePoint = TablePoint();

            if (Input.GetKeyUp(KeyCode.R) && canRotate)
                Rotate(tablePoint);

            // Centra o no el objeto al clickar
            transform.position = tablePoint + clickOffset;
            transform.position = new Vector3(transform.position.x, hover, transform.position.z);
        }
    }

    public void OnPointerDown()
    {
        CheckInput(true);
    }

    public void OnPointerUp()
    {
        isClicked = false;
        Snap();
    }

    public void CheckInput(bool checkDuplicate)
    {
        if (Input.GetKey(KeyCode.LeftAlt) && checkDuplicate && canDuplicate)
        {
            Duplicate();
        }
        else
        {
            // Start Dragging
            isClicked = true;
            if (!centerOnClick)
                clickOffset = transform.position - TablePoint();
        }
    }

    private void Duplicate()
    {
        // Duplicate object
        GameObject duplicate = Instantiate(gameObject, transform.position, transform.rotation, null);
        Draggable duplicateDraggable = duplicate.GetComponent<Draggable>();
        duplicateDraggable.CheckInput(false);
        DragController.ItemBeingDragged = duplicateDraggable;
        duplicate.name = name;
    }

    private void Rotate(Vector3 tablePoint)
    {
        transform.RotateAround(tablePoint, Vector3.up, 90);
    }

    private float RoundToNumber(float number, float factor)
    { return Mathf.Round(number / factor) * factor; }

    // Returns point resulting from colliding ray from mouse to table
    private Vector3 TablePoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20f, LayerMask.GetMask("Table")))
            return hit.point;
        else
            return Vector3.zero;
    }

    private void Snap()
    {
        if (snapTo == SnapTo.None) return;

        float factor = GridController.TileSize;
        float x = 0f, z = 0f;

        if (snapTo == SnapTo.Line)
        {
            x = RoundToNumber(transform.localPosition.x, factor);
            z = RoundToNumber(transform.localPosition.z, factor);
        }
        else
        {
            x = RoundToNumber(transform.localPosition.x - (factor / 2), factor) + factor / 2;
            z = RoundToNumber(transform.localPosition.z - (factor / 2), factor) + factor / 2;
        }

        transform.localPosition = new Vector3(x, transform.localPosition.y, z);
    }
}
