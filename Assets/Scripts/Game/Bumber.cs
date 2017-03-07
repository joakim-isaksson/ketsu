using UnityEngine;

namespace Ketsu.Game
{
    public interface BumberListener
    {
        void OnBumb(Collider other);
    }

    public class Bumber : MonoBehaviour
    {
        public BumberListener Listener;

        void OnTriggerEnter(Collider other)
        {
            if (Listener != null) Listener.OnBumb(other);
        }
    }
}

