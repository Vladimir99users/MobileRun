using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIWindow : MonoBehaviour
    {
        [SerializeField] private Button dropButton;
        [SerializeField] private CanvasGroup group;

        public event System.Action OnDropButton;

        public void EnableDropButton()
        {
            dropButton.onClick.AddListener(() =>
            {
                OnDropButton?.Invoke();
            });

            group.blocksRaycasts = true;
            group.interactable = true;
            group.alpha = 1;
        }

        public void DisableDropButton()
        {
            dropButton.onClick.RemoveAllListeners();
            group.blocksRaycasts = false;
            group.interactable = false;
            group.alpha = 0;
        }

    }
}