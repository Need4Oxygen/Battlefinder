using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallConstructor : MonoBehaviour
{
    [HideInInspector] public bool isWalling = false;

    [SerializeField] private Camera cam = null;
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

    void Start()
    {
        pointer = Instantiate(pointerPrefab, Vector3.zero, Quaternion.identity);
        pointer.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isWalling)
            Walling();
    }

    public void StartWalling()
    {
        isWalling = true;

        pointer.gameObject.SetActive(true);
    }

    public void StopWalling()
    {
        isWalling = false;
        pointer.gameObject.SetActive(false);

        GenerateWall();
        ClearLandMarksAndLine();
    }

    private void Walling()
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

    private void GenerateWall()
    {
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
    }

    public void OnPointerDown()
    {
        PlaceLandMark();
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
