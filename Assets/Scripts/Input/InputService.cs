using UnityEngine;
using UnityEngine.InputSystem;
using AndroidInput = UnityEngine.InputSystem.AndroidInput;

namespace Assets.Scripts.Input
{
    public class InputService : MonoBehaviour, IMovable
    {
        public event System.Action<Vector2> OnMoved;
        public event System.Action<Vector2> OnLooked;
        public event System.Action OnTouched;
        public AndroidInput input;

        private bool isSendLookData = false;
        private Vector2 movingVector;
        private Vector2 startingVector;
        private Vector2 lookingVector;
        public void Initialize()
        {
            input = new AndroidInput();
            input.Enable();
            input.Player.Move.Enable();
            input.Player.Look.Enable();
            input.Player.Touch.Enable();
            input.Player.Fire.Enable();
            input.Player.MousePosition.Enable();

            input.Player.Move.performed += SendMovingPlayer;
            input.Player.Move.canceled += SendMovingPlayerCanceled;
            // input.Player.Look.performed += SendLookAround;

            input.Player.Touch.started += SendLookStarted;
            input.Player.Touch.canceled += SendLookCanceled;
            input.Player.MousePosition.performed += SendLookAround;
            input.Player.Fire.canceled += Fire;
        }

        private void Fire(InputAction.CallbackContext obj)
        {
            if (lookingVector.Equals(Vector2.zero) && movingVector.x > (Screen.width / 3))
            {
                isSendLookData = obj.ReadValueAsButton();
                OnTouched?.Invoke();
            }
            // ;
        }
        private void SendLookStarted(InputAction.CallbackContext obj)
        {
            isSendLookData = obj.ReadValueAsButton();
            startingVector = input.Player.MousePosition.ReadValue<Vector2>();
            //  Debug.Log("Started");
            //  OnTouched?.Invoke(startingVector);
        }

        private void SendLookCanceled(InputAction.CallbackContext obj)
        {
            lookingVector = Vector2.zero;
            if (movingVector.x < (Screen.width / 3))
            {
                isSendLookData = false;
                OnLooked?.Invoke(lookingVector);
                return;
            }

            isSendLookData = false;
            OnLooked?.Invoke(lookingVector);
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

            lookingVector = input.Player.Look.ReadValue<Vector2>();

            if (lookingVector.Equals(Vector2.zero))
                return;

            OnLooked?.Invoke(lookingVector);
        }



        private void SendMovingPlayer(InputAction.CallbackContext obj)
        {
            var movingVector = obj.ReadValue<Vector2>();
            OnMoved?.Invoke(movingVector);
        }

        private void SendMovingPlayerCanceled(InputAction.CallbackContext obj)
        {
            OnMoved?.Invoke(Vector2.zero);
        }


    }

    public interface IMovable
    {
        public event System.Action<Vector2> OnMoved;
        public event System.Action<Vector2> OnLooked;
        public event System.Action OnTouched;
    }


}