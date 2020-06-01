using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PF2E_SceneManager : MonoBehaviour
{
    public static DVoid OnPlaySceneLoad;
    public static DVoid OnPlaySceneExit;

    [SerializeField] private WallTool wallTool = null;
    [SerializeField] private FloorTool floorTool = null;
    [SerializeField] private Terrain board = null;

    void Awake()
    {
        if (PF2E_Globals.CurrentBoard != null)
        {
            if (PF2E_Globals.CurrentBoard.terrainAlphamaps != null)
            {
                board.terrainData.SetAlphamaps(0, 0, PF2E_Globals.CurrentBoard.terrainAlphamaps);
                floorTool.SetAlphaMaps(false);
            }
            else
            {
                floorTool.SetAlphaMaps(true);
            }
            if (PF2E_Globals.CurrentBoard.terrainHeights != null)
            {
                board.terrainData.SetHeights(0, 0, PF2E_Globals.CurrentBoard.terrainHeights);
                floorTool.SetHeightMaps(false);
            }
            else
            {
                floorTool.SetHeightMaps(true);
            }



            if (PF2E_Globals.CurrentBoard.wallElements != null)
                wallTool.GenerateWallElements(PF2E_Globals.CurrentBoard.wallElements);
        }
        else
        {
            floorTool.SetAlphaMaps(true);
            floorTool.SetHeightMaps(true);
        }
    }

    void Start()
    {
        if (OnPlaySceneLoad != null)
            OnPlaySceneLoad();
        OnPlaySceneLoad = null;
    }

    public void OnClickExitButton()
    {
        if (PF2E_Globals.CurrentBoard != null)
        {
            // Save floors
            int ah = floorTool.board.terrainData.alphamapHeight;
            int aw = floorTool.board.terrainData.alphamapWidth;
            int r = floorTool.board.terrainData.heightmapResolution;
            Debug.Log(ah + "   " + aw + "   " + r);
            PF2E_Globals.CurrentBoard.terrainAlphamaps = floorTool.board.terrainData.GetAlphamaps(0, 0, ah, aw);
            PF2E_Globals.CurrentBoard.terrainHeights = floorTool.board.terrainData.GetHeights(0, 0, r, r);

            // Save walls
            PF2E_Globals.CurrentBoard.wallElements = wallTool.RetrieveWallElements();

            PF2E_Globals.SaveBoard(PF2E_Globals.CurrentBoard);
        }

        if (OnPlaySceneExit != null)
            OnPlaySceneExit();
        OnPlaySceneExit = null;

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
