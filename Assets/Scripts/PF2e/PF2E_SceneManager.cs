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

    public PF2E_BoardDetails currentBoardMaps = null;

    void Awake()
    {
        floorTool.ResetAlphaMaps();
        floorTool.ResetHeightMaps();

        if (PF2E_Globals.CurrentBoard != null)
        {
            currentBoardMaps = Json.Deserialize<PF2E_BoardDetails>(PF2E_Globals.CurrentBoard.boardMaps);

            if (currentBoardMaps == null)
                currentBoardMaps = new PF2E_BoardDetails();

            // Apply found maps
            string[] strArray = null;
            if (currentBoardMaps.terrainAlphamaps != null)
                foreach (var item in currentBoardMaps.terrainAlphamaps)
                {
                    strArray = item.Key.Split(',');
                    floorTool.alphamap[int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2])] = item.Value;
                }

            if (currentBoardMaps.terrainHeightmaps != null)
                foreach (var item in currentBoardMaps.terrainHeightmaps)
                {
                    strArray = item.Key.Split(',');
                    floorTool.heightmap[int.Parse(strArray[0]), int.Parse(strArray[1])] = item.Value;
                }

            floorTool.SetAlphaMaps();
            floorTool.SetHeightMaps();

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

        Audio.Instance.Stop_Music(0f, 0f);
        Audio.Instance.Stop_Ambient(0f, 0f);
        Audio.Instance.Play_Ambient("Old City Ambience", 0f, 0f);
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

            Dictionary<string, float> alphaDic = new Dictionary<string, float>();
            Dictionary<string, float> heightDic = new Dictionary<string, float>();

            string str = "";

            // This proccess should be paralelized into multiple coroutines, doing so should make this near instant
            for (int i = 0; i < aw; i++)
                for (int j = 0; j < ah; j++)
                    for (int k = 0; k < l; k++)
                        if ((k == 0 && alphaMaps[i, j, k] != 1f) || (k != 0 && alphaMaps[i, j, k] != 0f))
                        {
                            str = i + "," + j + "," + k;
                            alphaDic.Add(str, alphaMaps[i, j, k]);
                        }

            for (int i = 0; i < r; i++)
                for (int j = 0; j < r; j++)
                    if (heightsMaps[i, j] != 0.5f)
                    {
                        str = i + "," + j;
                        heightDic.Add(str, heightsMaps[i, j]);
                    }

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
