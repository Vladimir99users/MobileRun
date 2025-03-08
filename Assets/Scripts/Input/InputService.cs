using UnityEngine;
using UnityEngine.InputSystem;
using AndroidInput = UnityEngine.InputSystem.AndroidInput;

namespace Assets.Scripts.Input
{
    public class InputService : MonoBehaviour, IMovable
    {
        public event System.Action<Vector2> OnMoved;
        public event System.Action<Vector2> OnLooked;
        public AndroidInput input;

        private bool isSendLookData = false;
        private Vector2 movingVector;
        private Vector2 startingVector;
        public void Initialize()
        {
            input = new AndroidInput();
            input.Enable();
            input.Player.Move.Enable();
            input.Player.Look.Enable();
            input.Player.Touch.Enable();
            input.Player.MousePosition.Enable();

            input.Player.Move.performed += SendMovingPlayer;
            input.Player.Move.canceled += SendMovingPlayerCalceled;
            // input.Player.Look.performed += SendLookAround;

            input.Player.Touch.started += SendLookStarted;
            input.Player.Touch.canceled += SendLookCanceled;
            input.Player.MousePosition.performed += SendLookAround;
        }

        private void SendLookStarted(InputAction.CallbackContext obj)
        {
            isSendLookData = obj.ReadValueAsButton();
            startingVector = input.Player.MousePosition.ReadValue<Vector2>();
            Debug.Log($"Touch is starter {isSendLookData} position {startingVector}");
        }

        private void SendLookCanceled(InputAction.CallbackContext obj)
        {
            if (movingVector.x < (Screen.width / 3))
            {
                isSendLookData = false;
                OnLooked?.Invoke(Vector2.zero);
                return;
            }

            Debug.Log($"Touch is ending delta equals = {startingVector - movingVector} <> {input.Player.Look.ReadValue<Vector2>()}");
            isSendLookData = false;
            OnLooked?.Invoke(Vector2.zero);
        }

        private void SendLookAround(InputAction.CallbackContext obj)
        {
            if (!isSendLookData)
                return;

            movingVector = obj.ReadValue<Vector2>();

            if (movingVector.x < (Screen.width / 3))
            {
                return;
            }

            OnLooked?.Invoke(input.Player.Look.ReadValue<Vector2>());

            // Debug.Log($"Input System Look around {movingVector}");
            //
        }



        private void SendMovingPlayer(InputAction.CallbackContext obj)
        {
            var movingVector = obj.ReadValue<Vector2>();
            Debug.Log($"Input System moving {movingVector}");
            OnMoved?.Invoke(movingVector);
        }

        private void SendMovingPlayerCalceled(InputAction.CallbackContext obj)
        {
            OnMoved?.Invoke(Vector2.zero);
        }


    }

    public interface IMovable
    {
        public event System.Action<Vector2> OnMoved;
        public event System.Action<Vector2> OnLooked;
    }


}