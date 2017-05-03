using UnityEngine;

namespace Game
{
	public class Home : MapObject
	{
		public MapObjectType ForType;

        [HideInInspector]
		public int Occupants;

	    bool ready;
	    Animator anim;

		void Awake()
		{
		    anim = GetComponentInChildren<Animator>();
		}

	    void Update()
	    {
	        if (ready) return;

	        if (Gas.GasToCollect == 0)
	        {
	            anim.SetTrigger("Ready");
	            ready = true;
	        }
	    }

		void OnTriggerEnter(Collider other)
		{
            MapObject obj = other.GetComponent<MapObject>();
            if (obj != null && obj.Type == ForType) Occupants++;
        }

		private void OnTriggerExit(Collider other)
		{
            MapObject obj = other.GetComponent<MapObject>();
            if (obj != null && obj.Type == ForType) Occupants--;
		}
	}
}