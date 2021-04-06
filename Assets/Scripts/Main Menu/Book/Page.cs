using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Book
{

    public class Page : MonoBehaviour
    {

        // Pages have to be deactivated on game start, or grads explode

        public bool hasTab;

        [HideInInspector] public BookScript bookScript;

        [SerializeField] private int pageNum = 0;
        [SerializeField] private TMP_Text frontPageNum = null;
        [SerializeField] private TMP_Text backPageNum = null;
        [SerializeField] private GameObject pageRotatingTab = null;
        [SerializeField] private Image[] gradients = null;

        private void OnEnable()
        {
            ShowGradient();
            SetPageNums();
        }

        private void SetPageNums()
        {
            frontPageNum.text = pageNum.ToString();
            backPageNum.text = (pageNum + 1).ToString();
        }

        public void HideGradient()
        {
            foreach (Image grad in gradients)
            {
                grad.CrossFadeAlpha(1f, 0f, false);
                grad.CrossFadeAlpha(0f, bookScript.gradFadeInTime, false);
            }
        }

        public void ShowGradient()
        {
            foreach (Image grad in gradients)
            {
                grad.CrossFadeAlpha(0f, 0f, false);
                grad.CrossFadeAlpha(1f, bookScript.gradFadeOutTime, false);
            }
        }

        public void ShowTab()
        {
            pageRotatingTab.SetActive(true);
        }

        public void HideTab()
        {
            pageRotatingTab.SetActive(false);
        }
    }

}
