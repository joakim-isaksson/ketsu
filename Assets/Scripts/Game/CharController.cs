using UnityEngine;

namespace Game
{
	public class CharController : MonoBehaviour
	{
		[Header("Swipe Controls")]
        public bool UseSwiping;
        public float DragDistance;

        [Header("Tap Controls")]
        public bool UseTapping;
        public bool UseRelativeTap;
        public LayerMask TapLayer;

        [Header("Other")]
        public bool AllowCharacterSelection;
        
        CharacterHandler charHandler;
		MapManager mapManager;
        int tapRaycastMask;

		Vector2 touchStartPos;

		void Awake()
		{
            
        }

		void Start()
		{
            charHandler = FindObjectOfType<CharacterHandler>();
            mapManager = FindObjectOfType<MapManager>();
        }

		void Update()
		{
            if (mapManager.Solved || mapManager.Split) return;

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
			HandleKeyInputs();
#else
            HandleTouchInputs();
#endif
		}

		void HandleKeyInputs()
		{
			if (Input.GetButtonDown("Left")) charHandler.MoveAction(Vector3.left);
			else if (Input.GetButtonDown("Right")) charHandler.MoveAction(Vector3.right);
			else if (Input.GetButtonDown("Forward")) charHandler.MoveAction(Vector3.forward);
			else if (Input.GetButtonDown("Back")) charHandler.MoveAction(Vector3.back);
			else if (Input.GetMouseButtonDown(0)) ClickOrTap(Input.mousePosition);
		}

		void HandleTouchInputs()
		{
			if (Input.touchCount == 1)
			{
				Touch touch = Input.GetTouch(0);

				// Touch Started
				if (touch.phase == TouchPhase.Began)
				{
                    if (UseSwiping)
                    {
                        touchStartPos = touch.position;
                    }
                    else if (UseTapping)
                    {
                        // Early TAP (when using tapping and not swiping)
                        ClickOrTap(Camera.main.ScreenToWorldPoint(touch.position));
                    }
					
				}

				// Touch Ended
				else if (touch.phase == TouchPhase.Ended)
				{
                    if (UseSwiping)
                    {
                        // Check for SWIPE
                        if (Vector3.Distance(touchStartPos, touch.position) > Screen.height * DragDistance)
                        {
                            if (Mathf.Abs(touchStartPos.x - touch.position.x) > Mathf.Abs(touchStartPos.y - touch.position.y))
                            {
                                if ((touchStartPos.x < touch.position.x)) charHandler.MoveAction(Vector3.right);
                                else charHandler.MoveAction(Vector3.left);
                            }
                            else
                            {
                                if (touchStartPos.y < touch.position.y) charHandler.MoveAction(Vector3.forward);
                                else charHandler.MoveAction(Vector3.back);
                            }
                        }
                        else if (UseTapping)
                        {
                            // Late TAP (when using swiping with tapping)
                            ClickOrTap(touch.position);
                        }
                    }
				}
			}
		}

		void ClickOrTap(Vector3 screenPosition)
		{
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, Camera.main.farClipPlane, TapLayer.value)) return;

            // Character selection
            if (AllowCharacterSelection && charHandler.SelectCharacter(hit.point)) return;

            // Move action
            if (UseRelativeTap)
            {
                if (Mathf.Abs(charHandler.ActiveCharacter.transform.position.x - hit.point.x) >
                        Mathf.Abs(charHandler.ActiveCharacter.transform.position.y - hit.point.y))
                {
                    if (charHandler.ActiveCharacter.transform.position.x < hit.point.x) charHandler.MoveAction(Vector3.right);
                    else charHandler.MoveAction(Vector3.left);
                }
                else
                {
                    if (charHandler.ActiveCharacter.transform.position.z < hit.point.z) charHandler.MoveAction(Vector3.forward);
                    else charHandler.MoveAction(Vector3.back);
                }
            }
            else // Fixed tapping
            {
                if (Mathf.Abs(Camera.main.transform.position.x - hit.point.x) >
                        Mathf.Abs(Camera.main.transform.position.z - hit.point.z))
                {
                    if (Camera.main.transform.position.x < hit.point.x) charHandler.MoveAction(Vector3.right);
                    else charHandler.MoveAction(Vector3.left);
                }
                else
                {
                    if (Camera.main.transform.position.z < hit.point.z) charHandler.MoveAction(Vector3.forward);
                    else charHandler.MoveAction(Vector3.back);
                }
            }
            
        }
	}
}