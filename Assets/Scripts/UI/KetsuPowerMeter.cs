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
            charHandler = FindObjectOfType<CharacterHandler>();
        }

        void Start()
        {

        }

        void Update()
        {
            ketsuPowerText.text = charHandler.KetsuPower.ToString("0.00") + " / " + charHandler.MaxKetsuPower.ToString("0.00");
        }
    }
}