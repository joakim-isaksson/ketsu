using System;
using System.Collections;
using UnityEngine;

namespace Ketsu.Utils
{
	public class Flasher : MonoBehaviour
	{
	    public Color FlashColor;
	    public float FlashSpeed;

	    Material[] materials;
	    Color[] startingColors;
        
	    public bool flashing;

        void Start()
	    {
	        materials = GetComponentInChildren<Renderer>().materials;
	        startingColors = new Color[materials.Length];
	        for (int i = 0; i < materials.Length; i++)
	        {
	            startingColors[i] = materials[i].color;
	        }
        }

	    public void StartFlashing()
	    {
            flashing = true;
	    }

	    public void StopFlashing()
	    {
	        if (!flashing) return;

	        flashing = false;

	        for (int i = 0; i < materials.Length; i++)
	        {
	            materials[i].color = startingColors[i];
	        }
        }

        void Update()
        {
            if (!flashing) return;

            float state = ((Time.realtimeSinceStartup % FlashSpeed) / FlashSpeed) * 2.0f;

            if (state <= 1.0f)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].color = Color.Lerp(startingColors[i], FlashColor, state);
                }
            }
            else
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].color = Color.Lerp(FlashColor, startingColors[i], state - 1.0f);
                }
            }
	        
        }
    }
}