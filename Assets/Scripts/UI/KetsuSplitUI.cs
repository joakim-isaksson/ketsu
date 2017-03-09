using UnityEngine;
using Ketsu.Game;

namespace Ketsu.UI
{
	public class KetsuSplitUI : MonoBehaviour
	{
		public GameObject Leader;
		public GameObject Char;
		public Renderer LeadVisible;
		CharController ctrl;

		void Start()
		{
			LeadVisible = GetComponentsInChildren<Renderer>()[0];
			LeadVisible.enabled = false;
			ctrl = Char.GetComponent<CharController>();
		}

		void LateUpdate()
		{
			if (Leader == null){
				Leader = GameObject.Find("Ketsu(Clone)");
				LeadVisible.enabled = false;
			}
			else if (Leader.activeSelf == false)
				LeadVisible.enabled = false;
			else{
				if(ctrl.KetsuPower < 0.1f){
					LeadVisible.enabled = true;
					Vector3 targetPosition = Leader.transform.position;
					targetPosition.x = Leader.transform.position.x-0.5f;
					transform.position = targetPosition;
				}
			}
		}
	}
}

