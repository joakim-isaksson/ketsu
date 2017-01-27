using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Player.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        public bool MirrorMovement;

        void Awake()
        {

        }

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetAxis("Left") > 0)
            {
                Debug.Log("Left");
                transform.Translate(Vector3.left * (MirrorMovement ? -1 : 1));
            }
            else if (Input.GetAxis("Right") > 0)
            {
                Debug.Log("Right");
                transform.Translate(Vector3.right * (MirrorMovement ? -1 : 1));
            }
            else if (Input.GetAxis("Up") > 0)
            {
                Debug.Log("Up");
                transform.Translate(Vector3.up * (MirrorMovement ? -1 : 1));
            }
            else if (Input.GetAxis("Down") > 0)
            {
                Debug.Log("Down");
                transform.Translate(Vector3.down * (MirrorMovement ? -1 : 1));
            }
            else if (Input.GetAxis("Click") > 0)
            {
                Debug.Log("Click");
            }
        }
    }
}