using UnityEngine;
using System.Collections;

public class Sfera : MonoBehaviour {
	
	public GameObject coll;
	
	Vector3 startMarker;
    Vector3 endMarkerR;
	Vector3 endMarkerL;
	Vector3 endMarkerU;
	Vector3 endMarkerD;
	
    void Start() {
		
		calculateNewCoord();
		
    }
	
	float i = 0f;
	float ratie = 0.02f;
	float speed = 3f;
	
	bool right = false;
	bool left = false;
	bool up = false;
	bool down = false;
	// Locked
	
	public bool up_Locked;
	public bool left_Locked;
	public bool down_Locked;
	public bool right_Locked;
	
    void FixedUpdate() {
		if ((left == false)&&(up == false)&&(down == false)){
			if (Input.GetKey(KeyCode.D)){
				if (right_Locked == false){
					right = true;
					
				}
			}
		}
		if ((right == false)&&(up == false)&&(down == false)){
			if (Input.GetKey(KeyCode.A)){
				if (left_Locked == false){
					left = true;
					
				}
			}
		}	
		if ((right == false)&&(left == false)&&(down == false)){
			if (Input.GetKey(KeyCode.W)){
				if (up_Locked == false){
					up = true;
					
				}
				
			}
		}	
		if ((right == false)&&(left == false)&&(up == false)){
			if (Input.GetKey(KeyCode.S)){
				if (down_Locked == false){
					down = true;
					
				}
			}
		}
		
		if (right == true){
	        transform.position = Vector3.Lerp(startMarker, endMarkerR, i);
			i = i + ratie * speed;
			
			
			if (i >= 1){
				i = 0f;
				transform.position = endMarkerR;
				calculateNewCoord();
				right = false;
				
			}
		}
		if (left == true){
	        transform.position = Vector3.Lerp(startMarker, endMarkerL, i);
			i = i + ratie * speed;
			
			if (i >= 1){
				i = 0f;
				transform.position = endMarkerL;
				calculateNewCoord();
				left = false;
				
			}
		}
		if (up == true){
	        transform.position = Vector3.Lerp(startMarker, endMarkerU, i);
			i = i + ratie * speed;
			
			if (i >= 1){
				i = 0f;
				transform.position = endMarkerU;
				calculateNewCoord();
				up = false;
				
			}
		}
		if (down == true){
	        transform.position = Vector3.Lerp(startMarker, endMarkerD, i);
			i = i + ratie * speed;
			
			if (i >= 1){
				i = 0f;
				transform.position = endMarkerD;
				calculateNewCoord();
				down = false;
				
			}
		}
		
    }
	
	public void Impinge (int n,bool t){
		// right push - false
		if (n == 0){
			right_Locked = t;
		}
		// left push - false
		if (n == 1){
			left_Locked = t;
		}
		// down push
		if (n == 2){
			down_Locked = t;
		}
		// up push
		if (n == 3){
			up_Locked = t;
		}
	}

	void OnTriggerEnter (Collider obj){
//		Debug.Log(obj.gameObject.name);
		if (obj.gameObject.name == "Border_Up"){
			up_Locked = true;
		}
		if (obj.gameObject.name == "Border_Left"){
			left_Locked = true;
		}
		if (obj.gameObject.name == "Border_Down"){
			down_Locked = true;
		}
		if (obj.gameObject.name == "Border_Right"){
			right_Locked = false;
		}
	}
	void OnTriggerExit (Collider obj){
		Debug.Log(obj.gameObject.name);
		if (obj.gameObject.name == "Border_Up"){
			up_Locked = false;
		}
		if (obj.gameObject.name == "Border_Left"){
			left_Locked = false;
		}
		if (obj.gameObject.name == "Border_Down"){
			down_Locked = false;
		}
		if (obj.gameObject.name == "Border_Right"){
			right_Locked = false;
		}
	}
	
	public void calculateNewCoord(){
		startMarker = this.transform.position;
		endMarkerR = new Vector3(this.transform.position.x + 1f,this.transform.position.y,this.transform.position.z);
		endMarkerL = new Vector3(this.transform.position.x - 1f,this.transform.position.y,this.transform.position.z);
		endMarkerU = new Vector3(this.transform.position.x,this.transform.position.y + 1f,this.transform.position.z);
		endMarkerD = new Vector3(this.transform.position.x,this.transform.position.y - 1f,this.transform.position.z);
	}
	
	public void Death (int n){
		if (n == 0){
			Debug.Log("Play_Animation (Death by Water).\n");
			
			Impinge(0,true);
			Impinge(1,true);
			Impinge(2,true);
			Impinge(3,true);
			
		}
		
	}
}
