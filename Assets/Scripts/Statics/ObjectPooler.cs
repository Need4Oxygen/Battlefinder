using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IPooleable
{
    void OnSpawn();
    void OnDestroy();
}

public class ObjectPooler : MonoBehaviour
{
    public static Dictionary<string, Queue<GameObject>> Pools = new Dictionary<string, Queue<GameObject>>();

    public static void OnSceneChange(Scene current, Scene next)
    {
        Pools.Clear();
    }

    public static List<GameObject> CreatePool(GameObject prefab, int size)
    {
        if (!Pools.ContainsKey(prefab.name))
        {
            Queue<GameObject> newPool = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                newPool.Enqueue(obj);
            }

            Pools.Add(prefab.name, newPool);
            Debug.Log($"[ObjPooler] New pool: {prefab.name} with size: {size}");
            return newPool.ToList(); ;
        }
        else
        {
            Debug.LogWarning($"[ObjPooler] Pool with name: {prefab.name} already exist!");
            return null;
        }
    }

    public static GameObject Spawn(GameObject prefab, Transform parent) { return Spawn(prefab, Vector3.zero, Quaternion.identity, parent); }
    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (Pools.ContainsKey(prefab.name))
        {
            GameObject objectToSpawn;

            if (!Pools[prefab.name].Peek().activeSelf)
            {
                objectToSpawn = Pools[prefab.name].Dequeue();
                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;
                objectToSpawn.transform.parent = parent;
                objectToSpawn.SetActive(true);
            }
            else
            {
                objectToSpawn = Instantiate(prefab, position, rotation, parent);
            }

            IPooleable obj = objectToSpawn.GetComponent<IPooleable>();
            if (obj != null) { obj.OnSpawn(); }
            Pools[prefab.name].Enqueue(objectToSpawn);

            return objectToSpawn;
        }
        else
        {
            Debug.LogWarning($"[ObjPooler] Couldn't find pool wiht name: {prefab.name}, creating one now!");
            CreatePool(prefab, 1);
            return Spawn(prefab, position, rotation, parent);
        }
    }
}
