using Ketsu.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
	public class Chicken : MapObject, Consumable
	{
		public float MoveSpeed;
		public Transform Pumber;
		public int KetsuPowerGain;

		public string DieSfx;

		CharController controller;

		Vector3 target;

		MapManager map;

		void Awake()
		{

		}

		void Start()
		{
			controller = FindObjectOfType<CharController>();
			map = FindObjectOfType<MapManager>();
			SetNewTarget();
		}

		void Update()
		{
			// Move towards target
			transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * MoveSpeed);
			if (Vector3.Distance(transform.position, target) < 0.001f)
			{
				transform.position = target;
				SetNewTarget();
			}

			// If pumber is hitting something -> get new target
			if (IsBlocked(Pumber.position))
			{
				SetNewTarget();
			}
		}

		void SetNewTarget()
		{
			List<Vector3> directions = new List<Vector3>();
			directions.Add(Vector3.forward);
			directions.Add(Vector3.back);
			directions.Add(Vector3.left);
			directions.Add(Vector3.right);
			directions.Add(Vector3.zero);

			while (directions.Count > 0)
			{
				Vector3 direction = directions[Random.Range(0, directions.Count)];
				directions.Remove(direction);

				Vector3 newTarget = transform.position + direction;
                if (!IsBlocked(newTarget))
				{
					target = newTarget;
                    transform.LookAt(target);
					break;
				}
			}
		}

		public void Consume()
		{
			controller.KetsuPower += KetsuPowerGain;
			AkSoundEngine.PostEvent(DieSfx, gameObject);
			Destroy(gameObject);
		}
	}
}