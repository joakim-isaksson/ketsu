// Links to Other Objects:
var dummyPointer : Transform; // This can be just an empty gameobject (you don't need to see it!)
var disc : Transform; // This is the Main Disc

// Variables:
private var dummyOffset : float;
private var discOffset : float;

// A Flag, so we can do things only once:
private var mouseIsUp : boolean = true;

// Raycast SetUp:
var hit : RaycastHit;  

function Update () { 
	
	// If Mouse is Pressed:
	if (Input.GetMouseButton(0)){
		
		// Shoot out a RayCast:
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
		
		// If the Raycast hits something AND if it hits something called "MyHitArea":
		if (Physics.Raycast (ray, hit) && (hit.transform.name == "MyHitArea")){ 
        	
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
				disc.eulerAngles.y = discOffset + (dummyPointer.eulerAngles.y - dummyOffset); 

				if((disc.eulerAngles.y<90 && disc.eulerAngles.y>0))
					disc.eulerAngles.y = 90;
				else if(disc.eulerAngles.y<180 && disc.eulerAngles.y>=90)
					disc.eulerAngles.y = 180;
				else if(disc.eulerAngles.y<270 && disc.eulerAngles.y>=180)
					disc.eulerAngles.y = 270;
				else if(disc.eulerAngles.y<360 && disc.eulerAngles.y>=270)
					disc.eulerAngles.y = 0;
				else if(disc.eulerAngles.y<0 && disc.eulerAngles.y>=-90)
					disc.eulerAngles.y = -90;
				else
					disc.eulerAngles.y = 0;
			}
            
      	} 
		
	}else if (!mouseIsUp){
		// Reset the Flag now the mouse is up, so we know when it goes down again it will be the initial mouse press:
		mouseIsUp = true;
		
	} 
}
