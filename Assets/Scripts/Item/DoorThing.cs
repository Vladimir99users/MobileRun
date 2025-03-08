using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class DoorThing : Thing, IInteractable
    {
        [SerializeField] private bool isOpen = false;
        public float rotationAngle = 90f;
        public float rotationSpeed = 2f;

        private Quaternion closedRotation;
        private Quaternion openRotation;

        private void Start()
        {
            closedRotation = transform.rotation;
            openRotation = closedRotation * Quaternion.Euler(0, rotationAngle, 0);
        }
        public void Interact()
        {
            isOpen = !isOpen;
            StartCoroutine(RotateObject(isOpen ? openRotation : closedRotation));
        }

        private IEnumerator RotateObject(Quaternion targetRotation)
        {
            var startRotation = transform.rotation;
            var timeElapsed = 0f;

            while (timeElapsed < 1f)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed);
                timeElapsed += Time.deltaTime * rotationSpeed;
                yield return null;
            }
            transform.rotation = targetRotation;
        }
    }
}