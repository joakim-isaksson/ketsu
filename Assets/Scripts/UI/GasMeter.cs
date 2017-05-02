using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.UI
{
    public class GasMeter : MonoBehaviour
    {
        CharacterHandler charHandler;
        public Component Hand;

        void Awake()
        {

        }

        void Start()
        {
            charHandler = FindObjectOfType<CharacterHandler>();
        }

        void Update()
        {

        }
    }
}