using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Player.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        public bool MirrorMovement;
        public float MoveCooldown;

        bool moveOnCooldown;

        void Awake()
        {

        }

        void Start()
        {

        }

        void Update()
        {
            if (moveOnCooldown) return;

            if (Input.GetAxis("Left") > 0)
            {
                int direction = (MirrorMovement ? -1 : 1);
                transform.Translate(Vector3.left * direction);
                StartCoroutine(SetMovementOnCooldown());
            }
            else if (Input.GetAxis("Right") > 0)
            {
                int direction = (MirrorMovement ? -1 : 1);
                transform.Translate(Vector3.right * direction);
                StartCoroutine(SetMovementOnCooldown());
            }
            else if (Input.GetAxis("Up") > 0)
            {
                int direction = (MirrorMovement ? -1 : 1);
                transform.Translate(Vector3.forward * direction);
                StartCoroutine(SetMovementOnCooldown());
            }
            else if (Input.GetAxis("Down") > 0)
            {
                int direction = (MirrorMovement ? -1 : 1);
                transform.Translate(-Vector3.forward * direction);
                StartCoroutine(SetMovementOnCooldown());
            }
            else if (Input.GetAxis("Click") > 0)
            {
                Debug.Log("Click");
            }
        }

        IEnumerator SetMovementOnCooldown()
        {
            moveOnCooldown = true;
            yield return new WaitForSeconds(MoveCooldown);
            moveOnCooldown = false;
        }
    }
}