using Ketsu.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public enum EnemyMovementType
    {
        Random, PriorityList, PingPong
    }

    public class EnemyController : MapObject
    {
        [Header("Movement")]
        public bool CanMove;
        public float MovementSpeed;
        public EnemyMovementType MovementType;

        [Header("Consuming")]
        public bool CanBeConsumed;
        public float GivesKetsuPower;

        [Header("Dazing")]
        public bool CanBeDazed;
        public float DazeTime;

        [Header("Damage")]
        public bool DealsDamage;
        public int DamageAmount;

        [Header("Sounds")]
        public string DieSfx;

        CharacterHandler charHandler;

		Vector3 target;
        bool dazed;

		void Awake()
		{
            
		}

		void Start()
		{
            charHandler = FindObjectOfType<CharacterHandler>();
            SetNewTarget();
		}

		void Update()
		{
            if (CanMove && !dazed)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * MovementSpeed);
                if (Vector3.Distance(transform.position, target) < 0.001f)
                {
                    transform.position = target;
                    SetNewTarget();
                }
            }
		}

        public void OnBumb(Collider other)
        {
            MapObject obj = other.GetComponent<MapObject>();
            if (IsBlockedBy(obj))
            {
                SetNewTarget();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            Character character = other.GetComponent<Character>();
            if (character != null)
            {
                if (DealsDamage) character.TakeDamage(DamageAmount);

                if (CanBeDazed)
                {
                    dazed = true;
                    DelayedAction(DazeTime, delegate { dazed = false; });
                }
                else if (CanBeConsumed)
                {
                    charHandler.AddKetsuPower(GivesKetsuPower);
                    AkSoundEngine.PostEvent(DieSfx, gameObject);
                    Destroy(gameObject);
                }
            }
        }

        void SetNewTarget()
        {
            List<Vector3> directions = new List<Vector3>();

            switch (MovementType)
            {
                case EnemyMovementType.Random:
                    directions.Add(Vector3.forward);
                    directions.Add(Vector3.back);
                    directions.Add(Vector3.left);
                    directions.Add(Vector3.right);
                    ListUtils.Shuffle(directions);
                    break;
                case EnemyMovementType.PriorityList:
                    directions.Add(transform.forward);
                    directions.Add(-transform.right);
                    directions.Add(transform.right);
                    directions.Add(-transform.forward);
                    break;
                case EnemyMovementType.PingPong:
                    directions.Add(transform.forward);
                    directions.Add(-transform.forward);
                    break;
            }

            target = FirstNotBlockedDirection(directions);
            transform.LookAt(target);
        }

        Vector3 FirstNotBlockedDirection(List<Vector3> directions)
        {
            foreach (Vector3 direction in directions)
            {
                Vector3 newTarget = transform.position + direction;
                if (!IsBlocked(newTarget)) return newTarget;
            }

            return transform.position;
        }
    }
}