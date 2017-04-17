using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.UI
{
    public class KetsuPowerMeter : MonoBehaviour
    {
        public Text ketsuPowerText;
        CharacterHandler charHandler;

        [Header("PowerBar")]
        public Component BG;
        Image[] Colors;

        void Awake()
        {

        }

        void Start()
        {
            charHandler = FindObjectOfType<CharacterHandler>();
            Colors = BG.GetComponentsInChildren<Image>();
            for (int i=0; i<Colors.Length-1; i++)
        	{
        		if(i > charHandler.KetsuPower)
        			Colors[i].enabled = false;
        	}
        }

        void Update()
        {
            ketsuPowerText.text = charHandler.KetsuPower.ToString("0");
            for (int i=0; i<Colors.Length-1; i++)
        	{
        		if(i > charHandler.KetsuPower)
        			Colors[i].enabled = false;
        		else
        			Colors[i].enabled = true;
        	}
        }
    }
}