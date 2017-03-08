using Ketsu.Utils;
using UnityEngine;

namespace Ketsu.Game
{
	public class CharController : MonoBehaviour
	{
		[Header("Controls")]
		[Tooltip("Percentage of the screens height")]
		public float DragDistance;
        public bool UseRelativeTap;
        public bool AllowCharacterSelection;

        MapManager mapManager;
        CharacterHandler charHandler;

		Vector2 touchStartPos;

		void Awake()
		{
			mapManager = FindObjectOfType<MapManager>();
            charHandler = FindObjectOfType<CharacterHandler>();
        }

		void Start()
		{
			
		}

		void Update()
		{
            if (MapManager.Solved) return;

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
			else if (Input.GetMouseButtonDown(0))
			{
                ClickOrTap(Input.mousePosition);
            }
		}

		void HandleTouchInputs()
		{
			if (Input.touchCount == 1)
			{
				Touch touch = Input.GetTouch(0);

				// Touch Started
				if (touch.phase == TouchPhase.Began)
				{
					touchStartPos = touch.position;
				}

				// Touch Ended
				else if (touch.phase == TouchPhase.Ended)
				{
					// It's a SWIPE
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

					// It's a TAP
					else
					{
                        ClickOrTap(Camera.main.ScreenToWorldPoint(touch.position));
					}
				}
			}
		}

        // Is this 2d or 3d tap TODO
		void ClickOrTap(Vector3 point)
		{
            // Character selection
            if (AllowCharacterSelection && charHandler.SelectCharacter(point)) return;

            // Move action
            if (UseRelativeTap)
            {
                if (Mathf.Abs(charHandler.ActiveCharacter.transform.position.x - point.x) >
                        Mathf.Abs(charHandler.ActiveCharacter.transform.position.y - point.y))
                {
                    if (charHandler.ActiveCharacter.transform.position.x < point.x) charHandler.MoveAction(Vector3.right);
                    else charHandler.MoveAction(Vector3.left);
                }
                else
                {
                    if (charHandler.ActiveCharacter.transform.position.y < point.y) charHandler.MoveAction(Vector3.forward);
                    else charHandler.MoveAction(Vector3.back);
                }
            }
            else
            {
                // TODO
                Vector2 centerPoint = new Vector3(Screen.width / 2f, Screen.height / 2f);
            }
            
        }
	}
}