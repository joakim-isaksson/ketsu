#pragma strict

var texture1 : Texture2D;
var texture2 : Texture2D;
var texture3 : Texture2D;
var texture4 : Texture2D;
var texture5 : Texture2D;
var texture6 : Texture2D;
var texture7 : Texture2D;
var texture8 : Texture2D;
var texture9 : Texture2D;
var texture10 : Texture2D;
var change : boolean = true;
 
function Start() {
        changeTexture();
}
 
function Update () {
 
}
 
function changeTexture () {
 
        while(change) {
                yield WaitForSeconds(5);
                GetComponent.<Renderer>().material.mainTexture = texture1;      
                yield WaitForSeconds(0.05);
                GetComponent.<Renderer>().material.mainTexture = texture2;
                yield WaitForSeconds(0.05);
                GetComponent.<Renderer>().material.mainTexture = texture3;
                yield WaitForSeconds(0.05);
                GetComponent.<Renderer>().material.mainTexture = texture4;
                yield WaitForSeconds(0.05);
                GetComponent.<Renderer>().material.mainTexture = texture5;
                yield WaitForSeconds(0.05);
                GetComponent.<Renderer>().material.mainTexture = texture6;
                yield WaitForSeconds(0.05);
                GetComponent.<Renderer>().material.mainTexture = texture7;
                yield WaitForSeconds(0.05);
                GetComponent.<Renderer>().material.mainTexture = texture8;
                yield WaitForSeconds(0.05);
                GetComponent.<Renderer>().material.mainTexture = texture9;
                yield WaitForSeconds(0.05);
                GetComponent.<Renderer>().material.mainTexture = texture10;

                
        }
}