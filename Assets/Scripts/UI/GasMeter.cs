using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Ketsu.UI
{
    public class GasMeter : MonoBehaviour
    {
        public Component Hand;
        int StartGas=0;
        bool start = true;

        void Start()
        {
        }

        void Update()
        {
        	//hack to get the startgas initialized
        	if(start){
        		while(StartGas==0)
        			StartGas=Gas.GasToCollect;
        		start=false;
        	}
        	else if(Gas.GasToCollect!=0){
        		Quaternion rot = Quaternion.Euler(0,0,(1.0f/StartGas*180)*(Gas.GasToCollect-StartGas)+90);
        		if(Hand.transform.rotation != rot){
        	    	Hand.transform.rotation = Quaternion.Lerp(Hand.transform.rotation, rot, 0.05f);
        		}
        	}
            else {
                Hand.transform.rotation = Quaternion.Lerp(Hand.transform.rotation, Quaternion.Euler(0,0,-90), 0.05f);
            }
        }
    }
}