using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF2E_DataBase : MonoBehaviour
{
    public static PF2E_DataBase Instance;

    [SerializeField] private List<SO_PF2E_Ancestries> ancestries;
    [SerializeField] private List<SO_PF2E_Backgrounds> backgrounds;
    [SerializeField] private List<SO_PF2E_Classes> classes;

    public static Dictionary<E_PF2E_Ancestry, SO_PF2E_Ancestries> ancestriesDic;
    public static Dictionary<E_PF2E_Background, SO_PF2E_Backgrounds> backgroundsDic;
    public static Dictionary<E_PF2E_Class, SO_PF2E_Classes> classesDic;

    void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        foreach (var item in ancestries)
            ancestriesDic.Add(item.ancestry, item);
        foreach (var item in backgrounds)
            backgroundsDic.Add(item.background, item);
        foreach (var item in classes)
            classesDic.Add(item.gameClass, item);
    }
}
