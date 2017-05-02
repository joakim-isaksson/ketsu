using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.UI
{
    public class GasMeter : MonoBehaviour
    {
        public Component Hand;
        int StartGas = 0;

        void Start()
        {
            StartGas = Gas.GasToCollect;
        }

        void Update()
        {
        	if(Gas.GasToCollect!=0)
        	    Hand.transform.rotation = Quaternion.Euler(0,0,(1.0f/StartGas*180)*(Gas.GasToCollect-StartGas)+90);
            else 
                Hand.transform.rotation = Quaternion.Euler(0,0,-90);
        }
    }
}