using UnityEngine;
using System.Collections;

public class Logic_PushedObject : MonoBehaviour {

    public GameObject Pic;

	// Use this for initialization
	void Awake () {
        calculateNewCoord();
	}
	
	// Input from user
	public bool rightPushing_Locked = true;
		bool userPressed_Right;
		bool pushRight;
    public bool leftPushing_Locked = true;
		bool userPressed_Left;
		bool pushLeft;
    public bool upPushing_Locked = true;
		bool userPressed_Up;
		bool pushUp;
    public bool downPushing_Locked = true;
		bool userPressed_Down;
		bool pushDown;
		
	// Input logic variables
	float i = 0f;
	float ratie = 0.02f;
	float speed = 3f;
	
	void FixedUpdate() {
        // Input

		if (rightPushing_Locked == false){
		if ((pushLeft == false)&&
			(pushUp == false)&&
			(pushDown == false)){
			if ((Input.GetKey(KeyCode.D))||(userPressed_Right)){
				
					pushRight = true;
				}
			}
		}
		if (leftPushing_Locked == false){
		if ((pushRight == false)&&
			(pushUp == false)&&
			(pushDown == false)){
			if ((Input.GetKey(KeyCode.A))||(userPressed_Left)){
				
					pushLeft = true;
				}
			}
		}	
		if (upPushing_Locked == false){
		if ((pushRight == false)&&
            (pushLeft == false)&&
            (pushDown == false)){
			if ((Input.GetKey(KeyCode.W))||(userPressed_Up)){
				
					pushUp = true;
				}
				
			}
		}	
		if (downPushing_Locked == false){
		if ((pushRight == false)&&
            (pushLeft == false)&&
            (pushUp == false)){
			if ((Input.GetKey(KeyCode.S))||(userPressed_Down)){
				
					pushDown = true;
				}
			}
		}
        // Logic

		if (pushRight){
	        transform.position = Vector3.Lerp(startMarker, endMarkerR, i);
			i = i + ratie * speed;

			if (i >= 1){
				i = 0f;
				transform.position = endMarkerR;
				calculateNewCoord();
				pushRight = false;
				
			}
		}
		if (pushLeft){
	        transform.position = Vector3.Lerp(startMarker, endMarkerL, i);
			i = i + ratie * speed;
			
			if (i >= 1){
				i = 0f;
				transform.position = endMarkerL;
				calculateNewCoord();
				pushLeft = false;
			}
		}
		if (pushUp){
	        transform.position = Vector3.Lerp(startMarker, endMarkerU, i);
			i = i + ratie * speed;
			
			if (i >= 1){
				i = 0f;
				transform.position = endMarkerU;
				calculateNewCoord();
				pushUp = false;
			}
		}
		if (pushDown){
	        transform.position = Vector3.Lerp(startMarker, endMarkerD, i);
			i = i + ratie * speed;
			
			if (i >= 1){
				i = 0f;
				transform.position = endMarkerD;
				calculateNewCoord();
				pushDown = false;
			}
		}
    }

    Vector3 startMarker;
    Vector3 endMarkerR;
    Vector3 endMarkerL;
    Vector3 endMarkerU;
    Vector3 endMarkerD;

    private void calculateNewCoord()
    {
        startMarker = this.transform.position;
        endMarkerR = new Vector3(this.transform.position.x + 1f,
                                 this.transform.position.y,
                                 this.transform.position.z);
        endMarkerL = new Vector3(this.transform.position.x - 1f, 
                                 this.transform.position.y,
                                 this.transform.position.z);
        endMarkerU = new Vector3(this.transform.position.x,
                                 this.transform.position.y + 1f,
                                 this.transform.position.z);
        endMarkerD = new Vector3(this.transform.position.x,
                                 this.transform.position.y - 1f,
                                 this.transform.position.z);

        if (isDead){
            Pic.transform.parent = null;
            Pic.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            Destroy(this.gameObject);
        }
    }


    public void Push(int n)
    {
        if (n == 6)
        {
            if (lock_rightPushing == false)
            {
                rightPushing_Locked = false;
            }
            else {
                rightPushing_Locked = true;
            }
        } else
        if (n == 4)
        {
            if (lock_leftPushing == false)
            {
                leftPushing_Locked = false;
            }
            else
            {
                leftPushing_Locked = true;
            }
        } else
        if (n == 8)
        {
            if (lock_upPushing == false)
            {
                upPushing_Locked = false;
            }
            else
            {
                upPushing_Locked = true;
            }
        } else
        {
            if (lock_downPushing == false)
            {
                downPushing_Locked = false;
            }
            else
            {
                downPushing_Locked = true;
            }
        }
    }
    public void DontPush(int n)
    {
        if (n == 6)
        {
            rightPushing_Locked = true;
        } else
        if (n == 4)
        {
            leftPushing_Locked = true;
        } else
        if (n == 8)
        {
            upPushing_Locked = true; 
        } else
        {
            downPushing_Locked = true;
        }
    }

    public bool lock_rightPushing;
    public bool lock_leftPushing;
    public bool lock_upPushing;
    public bool lock_downPushing;

    public void LockPushing(int n)
    {
        if (n == 6)
        {
            DontPush(6);
            lock_rightPushing = true;
        }
        else
            if (n == 4)
            {
                DontPush(4);
                lock_leftPushing = true;
            }
            else
                if (n == 8)
                {
                    DontPush(8);
                    lock_upPushing = true;
                }
                else
                {
                    DontPush(2);
                    lock_downPushing = true;
                }
    }
    public void DontLockPushing(int n)
    {
        if (n == 6)
        {
            Push(6);
            lock_rightPushing = false;
            
        }
        else
            if (n == 4)
            {
                Push(4);
                lock_leftPushing = false;
            }
            else
                if (n == 8)
                {
                    Push(8);
                    lock_upPushing = false;
                }
                else
                {
                    Push(2);
                    lock_downPushing = false;
                }
    }

    // Parent
    public Logic_ControlObject Player;
    public void setPushingParent(Logic_ControlObject player)
    {
        Player = player;
    }
    public void removePushingParent()
    {
        Player = null;
    }

    //  If Phone.
    public void userPressed(int i, bool n)
    {
        if (i == 6)
        {
            userPressed_Right = n;
        }
        else if (i == 4)
        {
            userPressed_Left = n;
        }
        else if (i == 8)
        {
            userPressed_Up = n;
        }
        else
        {
            userPressed_Down = n;
        }
    }

    bool isDead = false;
    public void Death()
    {
        Debug.Log("Cube - Play_Animation (Death by Water).\n");

        // Destroy the side colliders (except the 'Pic') so no other restrictions will be applied to the player.
        foreach (Transform child in transform) {
            if (child.GetInstanceID() != Pic.transform.GetInstanceID())
                Destroy(child.gameObject);
        }

        isDead = true;
    }
}