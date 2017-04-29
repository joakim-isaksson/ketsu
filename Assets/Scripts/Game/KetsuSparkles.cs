using UnityEngine;

namespace Game
{
    public class KetsuSparkles : MonoBehaviour
    {
        public int ParticleMultiplier;

        ParticleSystem.EmissionModule emmisionModule;
        CharacterHandler charHandler;

        int oldValue = -1;

        void Awake()
        {
            emmisionModule = GetComponent<ParticleSystem>().emission;
        }

        void Start()
        {
            charHandler = FindObjectOfType<CharacterHandler>();
        }

        void Update()
        {
            if (charHandler.KetsuPower == oldValue) return;
            oldValue = charHandler.KetsuPower;

            emmisionModule.rateOverTime = charHandler.KetsuPower * ParticleMultiplier;
        }
    }
}