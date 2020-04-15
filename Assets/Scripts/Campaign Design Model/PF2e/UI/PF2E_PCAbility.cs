using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PF2E_PCAbility : MonoBehaviour
{
    [SerializeField] PF2E_PlayerCreationController controller = null;
    [SerializeField] E_PF2E_Ability ability = E_PF2E_Ability.None;
    [SerializeField] TMP_Text scoreText = null;
    [SerializeField] TMP_Text modText = null;

    private int _score;
    private int _mod;

    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            _mod = Mathf.FloorToInt(value - 10);

            scoreText.text = _score.ToString();
            modText.text = "+" + _mod.ToString();
        }
    }
}
