using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UIElements
{

    public class Window : MonoBehaviour
    {

        public const float openAnimDuration = 0.15f;
        public const float closeAnimDuration = 0.20f;

        public bool IsOpen { get { return _isOpen; } }
        private bool _isOpen;

        private VisualElement root;

        public static bool OverUI;

        private void Awake()
        {

            root = GetComponent<UIDocument>().rootVisualElement;

            root.RegisterCallback<MouseOverEvent>((evt) =>
                    {
                        OverUI = evt.target != root;
                    });

        }

        public void Open()
        {

            if (root == null) return;

            _isOpen = true;
            StartCoroutine(Fader.Fade(root, 0f, 1f, openAnimDuration));

        }

        public void Close()
        {

            if (root == null) return;

            _isOpen = false;
            StartCoroutine(Fader.Fade(root, 0f, 1f, closeAnimDuration));

        }

    }

}
