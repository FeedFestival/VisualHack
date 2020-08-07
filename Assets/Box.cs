using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

	
	Vector3 startMarker;
    Vector3 endMarkerR;
	Vector3 endMarkerL;
	Vector3 endMarkerU;
	Vector3 endMarkerD;
	
    void Start() {
		
		calculateNewCoord();
		
    }
	
	public GameObject player;
		
	float i = 0f;
	float ratie = 0.02f;
	float speed = 3f;
	
	bool right = false;
	bool left = false;
	bool up = false;
	bool down = false;
	// Locked
	public bool up_Locked = true;
	public bool left_Locked = true;
	public bool down_Locked = true;
	public bool right_Locked = true;
	
	
    void FixedUpdate() {
		if (right_Locked == false){
		if ((left == false)&&(up == false)&&(down == false)){
			if (Input.GetKey(KeyCode.D)){
				
					right = true;
				}
			}
		}
		if (left_Locked == false){
		if ((right == false)&&(up == false)&&(down == false)){
			if (Input.GetKey(KeyCode.A)){
				
					left = true;
				}
			}
		}	
		if (up_Locked == false){
		if ((right == false)&&(left == false)&&(down == false)){
			if (Input.GetKey(KeyCode.W)){
				
					up = true;
				}
				
			}
		}	
		if (down_Locked == false){
		if ((right == false)&&(left == false)&&(up == false)){
			if (Input.GetKey(KeyCode.S)){
				
					down = true;
				}
			}
		}
		
		if (right == true){
	        transform.position = Vector3.Lerp(startMarker, endMarkerR, i);
			i = i + ratie * speed;
//			Debug.Log(i);
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
	// cu nimic, e 4;
	public int cu_ce_imping = 4;
	
	public bool touch_Box_right = false;
	public bool touch_Box_left = false;
	public bool touch_Box_up = false;
	public bool touch_Box_down = false;
	
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
	
	public void calculateNewCoord(){
		startMarker = this.transform.position;
		endMarkerR = new Vector3(this.transform.position.x + 1f,this.transform.position.y,this.transform.position.z);
		endMarkerL = new Vector3(this.transform.position.x - 1f,this.transform.position.y,this.transform.position.z);
		endMarkerU = new Vector3(this.transform.position.x,this.transform.position.y + 1f,this.transform.position.z);
		endMarkerD = new Vector3(this.transform.position.x,this.transform.position.y - 1f,this.transform.position.z);
	}
	
	public void Destroyed (int n){
		if (n == 0){
			Debug.Log("Play_Animation (Cube_died_by_Water).");
			Impinge(0,true);
			Impinge(1,true);
			Impinge(2,true);
			Impinge(3,true);
			
			foreach (Transform child in transform){
				Destroy(child.gameObject);
			}
		}
	}
}