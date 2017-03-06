using UnityEngine;

namespace Ketsu.UI
{
	public class KetsuSplitUI : MonoBehaviour
	{
		public GameObject Leader;
		public Renderer LeadVisible;

		void Start()
		{
			LeadVisible = GetComponentsInChildren<Renderer>()[0];
			LeadVisible.enabled = false;
		}

		void LateUpdate()
		{
			if (Leader == null)
			{
				Leader = GameObject.Find("Ketsu(Clone)");
				if (Leader != null)
				{
					LeadVisible.enabled = true;
					Vector3 targetPosition = Leader.transform.position;
					targetPosition.y = transform.position.y;
					transform.position = targetPosition;
				}
				else
					LeadVisible.enabled = false;
			}
			else if (Leader.activeSelf == false)
				LeadVisible.enabled = false;
			else
			{
				Vector3 targetPosition = Leader.transform.position;
				targetPosition.y = transform.position.y;
				transform.position = targetPosition;
			}
		}
	}
}

