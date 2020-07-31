using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PF2E_ResultButton : MonoBehaviour, IPooleable
{
    public Button button;
    public TMP_Text levelText;
    public TMP_Text mainText;
    public Image actionCostImage;

    public string result;

    public void OnSpawn()
    {

        // throw new System.NotImplementedException();
    }

    public void OnDestroy()
    {
        gameObject.SetActive(false);
    }
}
