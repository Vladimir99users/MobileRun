using UnityEngine;

namespace Assets.Scripts.Item
{
    public interface ITaked
    {
        public GameObject Take();
        public void Drop(Vector3 direction);
    }
}