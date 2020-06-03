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

    public PF2E_BoardMaps currentBoardMaps = null;

    void Awake()
    {
        floorTool.ResetAlphaMaps();
        floorTool.ResetHeightMaps();

        if (PF2E_Globals.CurrentBoard != null)
        {
            currentBoardMaps = Json.Deserialize<PF2E_BoardMaps>(PF2E_Globals.CurrentBoard.boardMaps);

            if (currentBoardMaps == null)
                currentBoardMaps = new PF2E_BoardMaps();

            // Apply found maps
            if (currentBoardMaps.terrainAlphamaps != null)
                foreach (var item in currentBoardMaps.terrainAlphamaps)
                    floorTool.alphamap[(int)item.Key.x, (int)item.Key.y, (int)item.Key.z] = item.Value;

            if (currentBoardMaps.terrainHeightmaps != null)
                foreach (var item in currentBoardMaps.terrainHeightmaps)
                    floorTool.heightmap[(int)item.Key.x, (int)item.Key.y] = item.Value;

            // Appply foudn wall elements
            if (currentBoardMaps.wallElements != null)
                wallTool.GenerateWallElements(currentBoardMaps.wallElements);
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
            int l = floorTool.board.terrainData.alphamapLayers;
            int r = floorTool.board.terrainData.heightmapResolution;

            float[,,] alphaMaps = floorTool.board.terrainData.GetAlphamaps(0, 0, ah, aw);
            float[,] heightsMaps = floorTool.board.terrainData.GetHeights(0, 0, r, r);

            Dictionary<Vector3, float> alphaDic = new Dictionary<Vector3, float>();
            Dictionary<Vector2, float> heightDic = new Dictionary<Vector2, float>();

            // This proccess should be paralelized into multiple coroutines, doing so should make this near instant
            for (int i = 0; i < aw; i++)
                for (int j = 0; j < ah; j++)
                    for (int k = 0; k < l; k++)
                        if (k == 0) // Main layer should be all 1
                        {
                            if (alphaMaps[i, j, k] != 1f)
                                alphaDic.Add(new Vector3(i, j, k), alphaMaps[i, j, k]);
                        }
                        else
                        {
                            if (alphaMaps[i, j, k] != 0f)
                                alphaDic.Add(new Vector3(i, j, k), alphaMaps[i, j, k]);
                        }

            for (int i = 0; i < r; i++)
                for (int j = 0; j < r; j++)
                    if (heightsMaps[i, j] != 0.5f)
                        heightDic.Add(new Vector3(i, j), heightsMaps[i, j]);

            currentBoardMaps.terrainAlphamaps = alphaDic;
            currentBoardMaps.terrainHeightmaps = heightDic;
            currentBoardMaps.wallElements = wallTool.RetrieveWallElements();

            PF2E_Globals.CurrentBoard.boardMaps = Json.Serialize(currentBoardMaps);
            PF2E_Globals.SaveBoard(PF2E_Globals.CurrentBoard);
        }

        if (OnPlaySceneExit != null)
            OnPlaySceneExit();
        OnPlaySceneExit = null;

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
