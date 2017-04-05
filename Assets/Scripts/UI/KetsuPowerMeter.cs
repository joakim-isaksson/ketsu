using Ketsu.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.UI
{
    public class KetsuPowerMeter : MonoBehaviour
    {
        Text ketsuPowerText;
        CharacterHandler charHandler;

        void Awake()
        {
            ketsuPowerText = GetComponent<Text>();
        }

        void Start()
        {
            charHandler = FindObjectOfType<CharacterHandler>();
        }

        void Update()
        {
            ketsuPowerText.text = charHandler.KetsuPower.ToString("0");
        }
    }
}