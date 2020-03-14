using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    public static float TileSize;

    [SerializeField] Transform verticalMain = null;
    [SerializeField] Transform verticalFill = null;
    [SerializeField] Transform horizontalMain = null;
    [SerializeField] Transform horizontalFill = null;

    [SerializeField] Vector2 boundaries = Vector2.zero;
    [SerializeField] float tileSize = 1;
    [SerializeField] float mainLinesHeight = 0.1f;
    [SerializeField] float fillLinesHeight = 0.09f;

    List<Transform> verticalLines = new List<Transform>();
    List<Transform> horizontalLines = new List<Transform>();

    void Start()
    {
        TileSize = tileSize;
        ReloadGrid();
    }

    void ReloadGrid()
    {
        // Vertical grid spawn
        int totalVLines = Mathf.CeilToInt(boundaries.x / tileSize);

        Vector3 spawnPointCenterMainVertical = new Vector3(0, mainLinesHeight, transform.position.y);
        Transform centerMainVertical = Instantiate(verticalMain, spawnPointCenterMainVertical, Quaternion.identity, transform).transform;
        centerMainVertical.SetAsLastSibling();
        verticalLines.Add(centerMainVertical);

        for (int i = 1; i < totalVLines; i++)
        {
            if (i % 5 == 0)
            {
                Vector3 spawnPointRight = new Vector3(transform.position.x + tileSize * i, mainLinesHeight, 0);
                Vector3 spawnPointLeft = new Vector3(transform.position.x - tileSize * i, mainLinesHeight, 0);

                Transform right = Instantiate(verticalMain, spawnPointRight, Quaternion.identity, transform).transform;
                right.SetAsLastSibling();
                verticalLines.Add(right);
                Transform left = Instantiate(verticalMain, spawnPointLeft, Quaternion.identity, transform).transform;
                left.SetAsLastSibling();
                verticalLines.Add(left);
            }
            else
            {
                Vector3 spawnPointRight = new Vector3(transform.position.x + tileSize * i, fillLinesHeight, 0);
                Vector3 spawnPointLeft = new Vector3(transform.position.x - tileSize * i, fillLinesHeight, 0);

                Transform right = Instantiate(verticalFill, spawnPointRight, Quaternion.identity, transform).transform;
                right.SetAsFirstSibling();
                verticalLines.Add(right);
                Transform left = Instantiate(verticalFill, spawnPointLeft, Quaternion.identity, transform).transform;
                left.SetAsFirstSibling();
                verticalLines.Add(left);
            }
        }

        // Horizontal grid spawn
        int totalHLines = Mathf.CeilToInt(boundaries.y / tileSize);

        Vector3 spawnPointCenterMainHorizontal = new Vector3(0, mainLinesHeight, transform.position.y);
        Transform centerMainHorizontal = Instantiate(horizontalMain, spawnPointCenterMainHorizontal, Quaternion.identity, transform).transform;
        centerMainHorizontal.SetAsLastSibling();
        horizontalLines.Add(centerMainHorizontal);

        for (int i = 1; i < totalHLines; i++)
        {

            if (i % 5 == 0)
            {
                Vector3 spawnPointUp = new Vector3(0, mainLinesHeight, transform.position.y + tileSize * i);
                Vector3 spawnPointDown = new Vector3(0, mainLinesHeight, transform.position.y - tileSize * i);

                Transform up = Instantiate(horizontalMain, spawnPointUp, Quaternion.identity, transform).transform;
                up.SetAsLastSibling();
                horizontalLines.Add(up);
                Transform down = Instantiate(horizontalMain, spawnPointDown, Quaternion.identity, transform).transform;
                down.SetAsLastSibling();
                horizontalLines.Add(down);
            }
            else
            {
                Vector3 spawnPointUp = new Vector3(0, fillLinesHeight, transform.position.y + tileSize * i);
                Vector3 spawnPointDown = new Vector3(0, fillLinesHeight, transform.position.y - tileSize * i);

                Transform up = Instantiate(horizontalFill, spawnPointUp, Quaternion.identity, transform).transform;
                up.SetAsFirstSibling();
                horizontalLines.Add(up);
                Transform down = Instantiate(horizontalFill, spawnPointDown, Quaternion.identity, transform).transform;
                down.SetAsFirstSibling();
                horizontalLines.Add(down);
            }
        }
    }

}
