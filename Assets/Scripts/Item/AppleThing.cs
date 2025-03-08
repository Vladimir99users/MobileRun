using UnityEngine;

namespace Assets.Scripts.Item
{
    public class AppleThing : Thing, ITaked
    {
        private SphereCollider boxCollider => GetComponent<SphereCollider>();
        private Rigidbody rigidbody => GetComponent<Rigidbody>();

        private float force = 7f;
        public GameObject Take()
        {
            Disable();
            return gameObject;
        }

        public void Drop(Vector3 direction)
        {
            Enable();
            rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }

        public void Disable()
        {
            boxCollider.enabled = false;
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rigidbody.isKinematic = true;
        }
        public void Enable()
        {
            boxCollider.enabled = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            transform.SetParent(null);

        }
    }
}
