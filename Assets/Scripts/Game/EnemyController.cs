using Ketsu.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
	public class EnemyController : MapObject, BumberListener
    {
		public float MoveSpeed;

        public bool Consumable;
        public int Damage;
        public int KetsuPowerGain;

		public string DieSfx;

		Vector3 target;

		void Awake()
		{

		}

		void Start()
		{
			SetNewTarget();
		}

		void Update()
		{
			transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * MoveSpeed);
			if (Vector3.Distance(transform.position, target) < 0.001f)
			{
				transform.position = target;
				SetNewTarget();
			}
		}

        void SetNewTarget()
        {
            switch(Type)
            {
                case MapObjectType.Chiken:
                    SetRandomTarget();
                    break;
                case MapObjectType.Sheep:
                    SetTurn();
                    break;
                case MapObjectType.Hedgehog:
                    break;
            }
        }

		Vector3 GetRandomTarget()
		{
			List<Vector3> directions = new List<Vector3>();
			directions.Add(Vector3.forward);
			directions.Add(Vector3.back);
			directions.Add(Vector3.left);
			directions.Add(Vector3.right);

			while (directions.Count > 0)
			{
				Vector3 direction = directions[Random.Range(0, directions.Count)];
				directions.Remove(direction);

				Vector3 newTarget = transform.position + direction;
                if (!IsBlocked(newTarget))
				{
                    return newTarget;
				}
			}

            return Vector3.zero;
		}

        public void OnBumb(Collider other)
        {
            MapObject obj = other.GetComponent<MapObject>();
            if (IsBlockedByType(obj))
            {
                SetNewTarget();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            Character character = other.GetComponent<Character>();
            if (character != null)
            {
                if (Consumable)
                {
                    AkSoundEngine.PostEvent(DieSfx, gameObject);
                    Destroy(gameObject);
                }

                if (DealDamage)
                {
                    character.TakeDamage(DealDamage);
                }
            }
        }
    }
}