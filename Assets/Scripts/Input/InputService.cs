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

        private AndroidInput input;
        private bool isSendingLookData = false;
        private Vector2 movingVector;
        private Vector2 lookingVector;
        public void Initialize()
        {
            input = new AndroidInput();
            input.Enable();
            input.Player.Move.performed += OnMovePerformed;
            input.Player.Move.canceled += OnMoveCanceled;
            input.Player.Touch.started += OnTouchStarted;
            input.Player.Touch.canceled += OnTouchCanceled;
            input.Player.MousePosition.performed += OnLookAround;
            input.Player.Fire.canceled += OnFire;
        }

        private void OnFire(InputAction.CallbackContext obj)
        {
            if (lookingVector.Equals(Vector2.zero) && movingVector.x > DeathZone())
            {
                isSendingLookData = obj.ReadValueAsButton();
                OnTouched?.Invoke();
            }

        }
        private void OnTouchStarted(InputAction.CallbackContext obj)
            => isSendingLookData = obj.ReadValueAsButton();
        private void OnTouchCanceled(InputAction.CallbackContext obj)
        {
            lookingVector = Vector2.zero;
            HandleLookedEvent();
        }
        private void OnLookAround(InputAction.CallbackContext obj)
        {
            movingVector = obj.ReadValue<Vector2>();
            if (!isSendingLookData || movingVector.x < DeathZone())
                return;

            lookingVector = input.Player.Look.ReadValue<Vector2>();
            if (lookingVector.Equals(Vector2.zero))
                return;
            OnLooked?.Invoke(lookingVector);
        }
        private void OnMovePerformed(InputAction.CallbackContext obj)
            => OnMoved?.Invoke(obj.ReadValue<Vector2>());
        private void OnMoveCanceled(InputAction.CallbackContext obj)
            => OnMoved?.Invoke(Vector2.zero);
        private void HandleLookedEvent()
        {
            isSendingLookData = false;
            OnLooked?.Invoke(lookingVector);
        }
        private float DeathZone()
            => Screen.width / 3f;

    }

    public interface IMovable
    {
        public event System.Action<Vector2> OnMoved;
        public event System.Action<Vector2> OnLooked;
        public event System.Action OnTouched;
    }


}