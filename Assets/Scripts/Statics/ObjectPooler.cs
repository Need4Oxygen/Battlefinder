using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IPooleable
{
    void OnSpawn();
    void Destroy();
}

public class ObjectPooler : MonoBehaviour
{
    public static Dictionary<string, Queue<GameObject>> Pools = new Dictionary<string, Queue<GameObject>>();

    // Pools only contain deactivated gameobjects
    // When spawned via ObjectPooler, object is dequeued
    // When destroyed via IPooleable, object is requeued

    public static void OnSceneChange(Scene current, Scene next)
    {
        Pools.Clear();
    }

    public static List<GameObject> CreatePool(GameObject prefab, int size)
    { return CreatePool(prefab, null, size); }
    public static List<GameObject> CreatePool(GameObject prefab, Transform parent, int size)
    {
        if (!Pools.ContainsKey(prefab.name))
        {

            IPooleable pooleable = prefab.GetComponent<IPooleable>();
            if (pooleable == null)
            {
                Debug.LogError($"<color=white>[ObjPooler]</color> Not pooleable object detected: {prefab.name}");
                return null;
            }

            Queue<GameObject> newPool = new Queue<GameObject>();
            Pools.Add(prefab.name, newPool);

            for (int i = 0; i < size; i++)
            {
                GameObject obj = null;
                if (parent != null)
                    obj = Instantiate(prefab, parent);
                else
                    obj = Instantiate(prefab);
                obj.SetActive(false);
                obj.name = prefab.name;

                newPool.Enqueue(obj);
            }

            Debug.Log($"<color=white>[ObjPooler]</color> New pool: {prefab.name} with size: {size}");
            return newPool.ToList(); ;
        }
        else
        {
            Debug.LogWarning($"<color=white>[ObjPooler]</color> Pool with name: {prefab.name} already exist!");
            return null;
        }
    }

    public static GameObject Spawn(GameObject prefab) { return Spawn(prefab, Vector3.zero, Quaternion.identity, null); }
    public static GameObject Spawn(GameObject prefab, Transform parent) { return Spawn(prefab, Vector3.zero, Quaternion.identity, parent); }
    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject obj = null;

        if (Pools[prefab.name].Count > 0)
        {
            obj = Pools[prefab.name].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.SetParent(parent);
        }
        else
        {
            obj = Instantiate(prefab, position, rotation, parent);
        }

        obj.SetActive(false);

        obj.name = prefab.name;

        IPooleable pooleable = obj.GetComponent<IPooleable>();
        pooleable.OnSpawn();

        return obj;
    }

    public static void Destroy(GameObject prefab)
    {
        prefab.SetActive(false);
        if (Pools.ContainsKey(prefab.name))
            Pools[prefab.name].Enqueue(prefab);
    }
}
