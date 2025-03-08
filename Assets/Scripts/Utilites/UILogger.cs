using Assets.Scripts.Input;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Utilites
{
    public class UILogger : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loggerText;
        [Inject] private IMovable iMovable;


        private void Start()
        {
            iMovable.OnLooked += (vector2) =>
            {
                WriteToLogger($"Looked to {vector2}");
            };
            iMovable.OnMoved += (vector2) =>
            {
                WriteToLogger($"Moved to {vector2}");
            };
        }


        private void WriteToLogger(string data)
        {
            loggerText.text += data + "\n";
        }

    }
}
