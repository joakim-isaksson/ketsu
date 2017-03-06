using Ketsu.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
	public class Sheep : MapObject
	{
		public float MoveSpeed;
		public Transform Pumber;

		Map map;
		Vector3 target;

		void Awake()
		{

		}

		void Start()
		{
			UpdatePositionFromWorld();

			map = MapManager.LoadedMap;

			SetNewTarget();
		}

		void Update()
		{
			// Move forward
			transform.position = Vector3.MoveTowards(transform.position, target, MoveSpeed * Time.deltaTime);
			if (Vector3.Distance(transform.position, target) < 0.001f)
			{
				transform.position = target;
				SetNewTarget();
			}

			// If pumber is hitting something -> turn
			IntVector2 pumberPos = IntVector2.FromXZ(Pumber.position);
			if (!map.Contains(pumberPos) || Blocked(pumberPos) != null)
			{
				SetNewTarget();
			}
		}

		void SetNewTarget()
		{
			List<Vector3> directions = new List<Vector3>();
			directions.Add(transform.forward);
			directions.Add(-transform.right);
			directions.Add(transform.right);
			directions.Add(-transform.forward);
			directions.Add(Vector3.zero);

			foreach (Vector3 direction in directions)
			{
				IntVector2 targetPos = IntVector2.FromXZ(transform.position + direction);
				if (map.Contains(targetPos) && Blocked(targetPos) == null)
				{
					target = new Vector3(
						targetPos.X,
						transform.position.y,
						targetPos.Y
					);
					transform.LookAt(target);
					return;
				}
			}
		}

		MapObject Blocked(IntVector2 point)
		{
			foreach (MapObject obj in map.GetObjects(point))
			{
				if (obj == this) continue;
				if (obj.Type == MapObjectType.Water) return obj;
				if (obj.Layer != MapLayer.Ground) return obj;
			}

			// Nothing is blocking
			return null;
		}
	}
}