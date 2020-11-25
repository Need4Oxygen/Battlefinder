using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TraitButton : MonoBehaviour, IPooleable
{
    public TMP_Text text;

    public void OnSpawn()
    { }

    public void Destroy()
    {
        ObjectPooler.Destroy(gameObject);
    }
}
