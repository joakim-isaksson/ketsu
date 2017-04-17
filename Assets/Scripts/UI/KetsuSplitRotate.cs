using UnityEngine;

namespace Ketsu.UI
{
	public class KetsuSplitRotate : MonoBehaviour
	{
		public Transform dummyPointer;
		public Transform disc;
		KetsuSplitUI SplitUI;
		Vector3 tempPos;
		float dummyOffset;
		float discOffset;
		bool mouseIsUp = true;
		RaycastHit hit;

		void Start(){
			SplitUI = FindObjectOfType<KetsuSplitUI>();
			tempPos = new Vector3(0,0,-1);
		} 

		void Update () { 
			
			// If Mouse is Pressed:
			if (Input.GetMouseButton(0)){

				// Shoot out a RayCast:
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
				
				// If the Raycast hits something AND if it hits something called "MyHitArea":
				if (Physics.Raycast (ray, out hit) && (hit.transform.name == "MyHitArea")){ 
		        	
		        	// Always Make the Dummy Pointer Look at the HitPoint:
		        	dummyPointer.LookAt(hit.point);
		        	
		        	// If this is the initial mouse press:			
					if(mouseIsUp){
						
						// Remember the angles of the DummyPointer AND the Disc:
						dummyOffset = dummyPointer.eulerAngles.y; 
		     			discOffset = disc.eulerAngles.y;
						
						// Note that it is no longer the initial mouse press:
		     			mouseIsUp = false;

					}else{
						
						// If is is no longer the initial mouse press (we must be dragging)
						Vector3 newAngle = new Vector3(disc.eulerAngles.x, discOffset + (dummyPointer.eulerAngles.y - dummyOffset), disc.eulerAngles.z);
						disc.eulerAngles = newAngle;

						if((disc.eulerAngles.y<90 && disc.eulerAngles.y>0)){
							tempPos = new Vector3(-1,0,0);
							disc.eulerAngles = new Vector3(disc.eulerAngles.x, 90, disc.eulerAngles.z);
						}else if(disc.eulerAngles.y<180 && disc.eulerAngles.y>=90){
							tempPos = new Vector3(0,0,1);
							disc.eulerAngles = new Vector3(disc.eulerAngles.x, 180, disc.eulerAngles.z);
						}else if(disc.eulerAngles.y<270 && disc.eulerAngles.y>=180){
							tempPos = new Vector3(1,0,0);
							disc.eulerAngles = new Vector3(disc.eulerAngles.x, 270, disc.eulerAngles.z);
						}else if(disc.eulerAngles.y<360 && disc.eulerAngles.y>=270){
							tempPos = new Vector3(0,0,-1);
							disc.eulerAngles = new Vector3(disc.eulerAngles.x, 0, disc.eulerAngles.z);
						}
						else{
							tempPos = new Vector3(0,0,-1);
							disc.eulerAngles = new Vector3(disc.eulerAngles.x, 0, disc.eulerAngles.z);
						}

					}
		            
		      	} 
				
			}else if (!mouseIsUp){
				// Reset the Flag now the mouse is up, so we know when it goes down again it will be the initial mouse press:
				mouseIsUp = true;
				
			} 
			if (Input.GetKeyDown("space")){
				SplitUI.SplitAccepted = true;
				SplitUI.SplitPos = tempPos;
			}
				      
		}

	}
}