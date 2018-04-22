using UnityEngine;

public class Orbiter : MonoBehaviour
{
	public GameObject ConsumeEffectPrefab;
	
	[HideInInspector] public Vector3 RelativePosition;

	public void Consume()
	{
		Instantiate(ConsumeEffectPrefab, transform.position, transform.rotation);
		Destroy();
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}
}