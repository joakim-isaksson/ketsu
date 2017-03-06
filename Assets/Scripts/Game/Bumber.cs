using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
	public class Bumber : MonoBehaviour
	{
		void Start()
		{

		}

		void Update()
		{

		}

		void OnTriggerEnter(Collider other)
		{
			MapObject obj = other.GetComponent<MapObject>();

		}
	}
}

