using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinder2e.Containers
{

    public class BuildButton : MonoBehaviour, IPooleable
    {
        [Header("Build Button")]
        public string itemType;
        public Button button;
        public Image icon;
        public TMP_Text title;
        public TMP_Text subtitle;

        public void OnSpawn()
        { }

        public void Destroy()
        {
            ObjectPooler.Destroy(gameObject);
        }
    }

}
