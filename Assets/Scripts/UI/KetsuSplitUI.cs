using UnityEngine;
using System.Collections;
using Game;


namespace Ketsu.UI
{
	public class KetsuSplitUI : MonoBehaviour
	{
		public GameObject Leader;
		public GameObject Char;
		public Renderer LeadVisible;
		public Vector3 SplitPos;
		public Vector3 KetsuPos;
		public bool SplitAccepted = false;

		CharacterHandler charHandler;
		MapManager mapManger;

		void Start()
		{
			LeadVisible = GetComponentsInChildren<Renderer>()[0];
			LeadVisible.enabled = false;
            charHandler = FindObjectOfType<CharacterHandler>();
            mapManger = FindObjectOfType<MapManager>();
            SplitPos = new Vector3(0,0,0);
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
				KetsuPos = Leader.transform.position;
				if(charHandler.KetsuPower < 0.1f){
					mapManger.Split = true;
					LeadVisible.enabled = true;
					Vector3 targetPosition = Leader.transform.position;
					targetPosition.x = Leader.transform.position.x-0.5f;
					transform.position = targetPosition;
					if(!SplitAccepted)
						StartCoroutine(Split());
					
					if(SplitPos != Vector3.zero){
						KetsuPos = new Vector3(KetsuPos.x+SplitPos.x, KetsuPos.y+SplitPos.y, KetsuPos.z+SplitPos.z);
						charHandler.SplitKetsu(KetsuPos);
						mapManger.Split = false;
						SplitAccepted = false;
						SplitPos = new Vector3(0,0,0);
					}

				}
			}
		}
		IEnumerator Split()
	    {
	        Debug.Log("Waiting for user to accept split... (Press Space to accept!)");
	        yield return new WaitUntil(() => SplitAccepted);
	        yield break;
	    }
	}
}
