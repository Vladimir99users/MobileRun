using Assets.Scripts.Input;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform camera;
        [SerializeField] private CharacterController characterController;
        public float movementSpeed = 5f; // Скорость вращения
        public float rotationSpeed = 5f; // Скорость вращения
        public float tiltSpeed = 2f; // Скорость наклона
        [Inject] private IMovable iMovable;
        private float currentPitch = 0f; // Текущий угол наклона
        private Vector2 lookInput;
        private Vector2 movedVector;
        public void Start()
        {
            iMovable.OnLooked += OnLook;
            iMovable.OnMoved += OnMoved;
        }

        private void Update()
        {
            // Вращаем персонажа на основе входных данных
            if (lookInput != Vector2.zero)
            {
                // Вращение влево и вправо
                float yaw = lookInput.x * rotationSpeed;
                transform.Rotate(0, yaw, 0);

                float pitch = lookInput.y * tiltSpeed;
                currentPitch -= pitch; // Обновляем текущий угол наклона
                currentPitch = Mathf.Clamp(currentPitch, -30f, 30f); // Ограничиваем угол наклона

                // Применяем наклон к персонажу
                camera.transform.eulerAngles = new Vector3(currentPitch, camera.transform.eulerAngles.y, 0);
            }
        }

        public void FixedUpdate()
        {
            if (movedVector == Vector2.zero)
            {
                characterController.Move(Vector3.zero);
                return;
            }


            Vector3 movement = new Vector3(movedVector.x, 0, movedVector.y) * movementSpeed;
            characterController.Move(movement);
        }

        // Метод для обработки ввода
        public void OnLook(Vector2 vector)
        {
            lookInput = vector;
        }

        public void OnMoved(Vector2 vector)
        {
            movedVector = vector;
        }
    }
}

