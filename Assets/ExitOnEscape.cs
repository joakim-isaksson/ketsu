using UnityEngine;

namespace Assets.Scripts
{
    public class ExitOnEscape : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKey("escape"))
                Application.Quit();
        }
    }
}