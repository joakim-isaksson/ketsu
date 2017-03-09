using UnityEngine;
using Ketsu.Game;

namespace Ketsu.UI
{
	public class KetsuSplitUI : MonoBehaviour
	{
		public GameObject Leader;
		public GameObject Char;
		public Renderer LeadVisible;

		CharacterHandler charHandler;

		void Start()
		{
			LeadVisible = GetComponentsInChildren<Renderer>()[0];
			LeadVisible.enabled = false;
            charHandler = FindObjectOfType<CharacterHandler>();
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
				if(charHandler.KetsuPower < 0.1f){
					LeadVisible.enabled = true;
					Vector3 targetPosition = Leader.transform.position;
					targetPosition.x = Leader.transform.position.x-0.5f;
					transform.position = targetPosition;
				}
			}
		}
	}
}

