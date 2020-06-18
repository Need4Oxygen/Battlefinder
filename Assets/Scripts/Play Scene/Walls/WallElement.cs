using System.Collections;
using BoardItems;
using Knife.HDRPOutline.Core;
using UnityEngine;

public class WallElement : BoardItem, ISelectable, ISnapable, IMovable, IRotable, IBulkRotable, IScalable, IDuplicable, IDestroyable
{
    [HideInInspector] public WallTool wallTool = null;

    [SerializeField] private OutlineObject outline = null;

    public E_WallElement type = 0;
    public SO_WallStyle style = null;

    // private bool isSelected = false;

    void Awake()
    {
        if (outline != null)
            outline.enabled = false;
    }

    // --------------------------------------SELECTION--------------------------------------

    public void Select(bool value)
    {
        if (outline != null)
            if (value)
            {
                // isSelected = true;
                outline.enabled = true;
            }
            else
            {
                // isSelected = false;
                outline.enabled = false;
            }
    }


    // --------------------------------------MOVEMENT--------------------------------------

    private Coroutine moveCorou = null;
    private Vector3 clickOffset = Vector3.zero;

    public void MoveStart()
    {
        if (moveCorou == null)
        {
            clickOffset = transform.position - InputManager.TablePoint(false);
            moveCorou = StartCoroutine(Move());
        }
    }

    public void MoveStop(bool snap)
    {
        if (moveCorou != null)
        {
            StopCoroutine(moveCorou);
            moveCorou = null;
            if (snap)
                Snap();
        }
    }

    private IEnumerator Move()
    {
        while (true)
        {
            Vector3 tablePoint = InputManager.TablePoint(false);
            transform.position = tablePoint + clickOffset;
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            yield return null;
        }
    }


    // --------------------------------------ROTATION--------------------------------------

    public int rotation = 90;

    /// <summary>Rotate the object. </summary>
    public void Rotate()
    {
        Vector3 tablePoint = InputManager.TablePoint(false);
        transform.RotateAround(tablePoint, Vector3.up, rotation);
        snapTo = WallTool.DetermineWallElementSnap(this);
    }


    // --------------------------------------ROTATION--------------------------------------

    /// <summary>Rotate the object. </summary>
    public void BulkRotate(Vector3 centre, int angle)
    {
        transform.RotateAround(centre, Vector3.up, angle);
        snapTo = WallTool.DetermineWallElementSnap(this);
    }


    // --------------------------------------SCALE--------------------------------------

    /// <summary>Scale the object to given scale. </summary>
    public void Scale(int scale)
    {
        throw new System.NotImplementedException();

        // transform.localScale = new Vector3(scale, transform.localScale.y, scale);

        // if (scale % 2 == 0) // If even
        //     snapTo = E_Snap.Line;
        // else
        //     snapTo = E_Snap.Centre;
    }


    // --------------------------------------SNAP--------------------------------------

    public E_Snap snapTo = 0;

    /// <summary>Snap the object to grid. </summary>
    public void Snap()
    {
        if (snapTo == E_Snap.None)
            return;
        else
            Snap(snapTo);
    }

    public void Snap(E_Snap snapTo)
    {
        float x = 0f, z = 0f;

        if (snapTo == E_Snap.Line)
        {
            x = Mathf.Round(transform.localPosition.x);
            z = Mathf.Round(transform.localPosition.z);
        }
        else if (snapTo == E_Snap.Tile)
        {
            x = Mathf.Round(transform.localPosition.x - (1f / 2f)) + 1f / 2f;
            z = Mathf.Round(transform.localPosition.z - (1f / 2f)) + 1f / 2f;
        }
        else if (snapTo == E_Snap.LineH)
        {
            x = Mathf.Round(transform.localPosition.x - (1f / 2f)) + 1f / 2f;
            z = Mathf.Round(transform.localPosition.z);
        }
        else if (snapTo == E_Snap.LineV)
        {
            x = Mathf.Round(transform.localPosition.x);
            z = Mathf.Round(transform.localPosition.z - (1f / 2f)) + 1f / 2f;
        }

        transform.localPosition = new Vector3(x, transform.localPosition.y, z);
    }


    // --------------------------------------DUPLICATION--------------------------------------

    public GameObject Duplicate()
    {
        GameObject duplicate = Instantiate(gameObject, transform.position, transform.rotation, null);
        duplicate.name = name + " +";

        return duplicate;
    }


    // --------------------------------------DESTRUCTION--------------------------------------

    public void Destroy()
    {
        if (wallTool != null)
            wallTool.DestroyElement(this);
    }

}
