using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructCategory
{
    public string name;
    public List<Transform> prefabList;
}

public class ConstructionController : MonoBehaviour
{
    [SerializeField] Transform constructCategoryButtonPrefab;
    [SerializeField] Transform constructableButtonPrefab;

    [SerializeField] List<ConstructCategory> categoriesList = new List<ConstructCategory>();

    void Start()
    {
        // Spawn Category Buttons
        for (int i = 0; i < categoriesList.Count; i++)
            SpawnCategoryButton(categoriesList[i].name, i);
    }

    void SpawnCategoryButton(string name, int id)
    {
        // spawnea el botón categoría y le mete la funcionalidad de CategoryButtonPressed
        // GameObject newButton = Instantiate(constructCategoryButtonPrefab)
    }

    void CategoryButtonPressed()
    {
        // spawnea los botones de los prefabs que tiene esta categoría
    }

}
