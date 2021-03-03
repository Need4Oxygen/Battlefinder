using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinder2e.GameData
{

    public class ResultButton : MonoBehaviour, IPooleable
    {
        public Button button;
        public TMP_Text levelText;
        public TMP_Text mainText;
        public Image actionCostImage;

        public void OnSpawn()
        { }

        public void Destroy()
        {
            ObjectPooler.Destroy(gameObject);
        }
    }

}
