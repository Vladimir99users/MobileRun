using Assets.Scripts.Input;
using Assets.Scripts.Item;
using Assets.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform camera;
        [SerializeField] private Transform takeObjectTransform;
        [SerializeField] private CharacterController characterController;
        [SerializeField][Range(3f, 10f)] private float distanceInteractable;
        public float movementSpeed = 5f;
        public float rotationSpeed = 5f;
        public float tiltSpeed = 2f;

        [Inject] private IMovable iMovable;
        [Inject] private UIWindow window;

        private const float GravityValue = -9.81f;
        private float currentPitch = 0f;
        private Vector2 lookInput;
        private Vector2 movedVector;

        private ITaked takedObject;

        private void Start()
        {
            iMovable.OnLooked += OnLook;
            iMovable.OnMoved += OnMoved;
            iMovable.OnTouched += InteractionObject;
            window.OnDropButton += DropItem;
        }

        private void FixedUpdate()
        {
            ApplyGravity();
            MoveCharacter();
        }

        private void ApplyGravity()
            => characterController.Move(new Vector3(0, GravityValue * Time.fixedDeltaTime, 0));
        private void MoveCharacter()
        {
            if (movedVector.sqrMagnitude == 0)
            {
                characterController.Move(Vector3.zero);
                return;
            }

            var movementVector = new Vector3(movedVector.x, 0, movedVector.y) * movementSpeed;
            movementVector = transform.TransformDirection(movementVector);
            characterController.Move(movementVector * Time.fixedDeltaTime);
        }

        private void OnLook(Vector2 vector)
        {
            lookInput = vector;
            RotatePlayer();
            TiltCamera();
        }

        private void RotatePlayer()
        {
            var yaw = lookInput.x * rotationSpeed;
            transform.Rotate(0, yaw, 0);
        }

        private void TiltCamera()
        {
            var pitch = lookInput.y * tiltSpeed;
            currentPitch = Mathf.Clamp(currentPitch - pitch, -40f, 50f);
            camera.transform.eulerAngles = new Vector3(currentPitch, camera.transform.eulerAngles.y, 0);
        }

        private void OnMoved(Vector2 vector)
        {
            movedVector = vector;
        }

        private void InteractionObject()
        {
            var ray = new Ray(camera.transform.position, camera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, distanceInteractable))
            {
                if (hit.transform.TryGetComponent<Thing>(out Thing thing))
                {
                    InteractedObject(thing as IInteractable);
                    TakeObject(thing as ITaked);
                }
            }
        }

        private void InteractedObject(IInteractable interactable)
            => interactable?.Interact();

        private void TakeObject(ITaked taked)
        {
            if (takedObject is not null || taked is null)
                return;

            takedObject = taked;
            GameObject obj = takedObject.Take();
            obj.transform.position = takeObjectTransform.position;
            obj.transform.SetParent(takeObjectTransform);
            window.EnableDropButton();
        }

        private void DropItem()
        {
            window.DisableDropButton();
            takedObject.Drop(camera.transform.forward);
            takedObject = null;
        }

    }
}

