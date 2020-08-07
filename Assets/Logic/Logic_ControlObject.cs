using UnityEngine;
using System.Collections;

public class Logic_ControlObject : MonoBehaviour
{

    public GameObject coll;

    // Use this for initialization
    void Awake()
    {
        calculateNewCoord();
    }

    // Input from user
    public bool rightPushing_Locked = false;
    bool userPressed_Right;
    bool pushRight;
    public bool leftPushing_Locked = false;
    bool userPressed_Left;
    bool pushLeft;
    public bool upPushing_Locked = false;
    bool userPressed_Up;
    bool pushUp;
    public bool downPushing_Locked = false;
    bool userPressed_Down;
    bool pushDown;

    // Input logic variables
    float i = 0f;
    float ratie = 0.02f;
    float speed = 3f;

    void FixedUpdate()
    {

        if ((pushLeft == false) &&
            (pushUp == false) &&
            (pushDown == false))
        {
            if ((Input.GetKey(KeyCode.D)) || (userPressed_Right))
            {
                if (rightPushing_Locked == false)
                {
                    pushRight = true;

                }
            }
        }
        if ((pushRight == false) &&
            (pushUp == false) &&
            (pushDown == false))
        {
            if ((Input.GetKey(KeyCode.A)) || (userPressed_Left))
            {
                if (leftPushing_Locked == false)
                {
                    pushLeft = true;

                }
            }
        }
        if ((pushRight == false) &&
            (pushLeft == false) &&
            (pushDown == false))
        {
            if ((Input.GetKey(KeyCode.W)) || (userPressed_Up))
            {
                if (upPushing_Locked == false)
                {
                    pushUp = true;

                }

            }
        }
        if ((pushRight == false) &&
            (pushLeft == false) &&
            (pushUp == false))
        {
            if ((Input.GetKey(KeyCode.S)) || (userPressed_Down))
            {
                if (downPushing_Locked == false)
                {
                    pushDown = true;

                }
            }
        }

        if (pushRight == true)
        {
            transform.position = Vector3.Lerp(startMarker, endMarkerR, i);
            i = i + ratie * speed;

            if (i >= 1)
            {
                i = 0f;
                transform.position = endMarkerR;
                calculateNewCoord();
                pushRight = false;

            }
        }
        if (pushLeft == true)
        {
            transform.position = Vector3.Lerp(startMarker, endMarkerL, i);
            i = i + ratie * speed;

            if (i >= 1)
            {
                i = 0f;
                transform.position = endMarkerL;
                calculateNewCoord();
                pushLeft = false;

            }
        }
        if (pushUp == true)
        {
            transform.position = Vector3.Lerp(startMarker, endMarkerU, i);
            i = i + ratie * speed;

            if (i >= 1)
            {
                i = 0f;
                transform.position = endMarkerU;
                calculateNewCoord();
                pushUp = false;

            }
        }
        if (pushDown == true)
        {
            transform.position = Vector3.Lerp(startMarker, endMarkerD, i);
            i = i + ratie * speed;

            if (i >= 1)
            {
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

    public void calculateNewCoord()
    {
        startMarker = this.transform.position;
        endMarkerR = new Vector3(this.transform.position.x + 1f, this.transform.position.y, this.transform.position.z);
        endMarkerL = new Vector3(this.transform.position.x - 1f, this.transform.position.y, this.transform.position.z);
        endMarkerU = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
        endMarkerD = new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z);
    }



    /***************************\//----------------------------------------------\
    *                           												*
    * The Colliding effects 													*
    * 					        												*
    *                           												*
    \***************************/
    // ---------------------------------------------\

    

    public bool pushingFrom_Right;
    public bool pushingFrom_Left;
    public bool pushingFrom_Up;
    public bool pushingFrom_Down;

    void OnTriggerEnter(Collider obj)
    {

        if (obj.gameObject.name == "Border_Up")
        {
            upPushing_Locked = true;
        }
        if (obj.gameObject.name == "Border_Left")
        {
            leftPushing_Locked = true;
        }
        if (obj.gameObject.name == "Border_Down")
        {
            downPushing_Locked = true;
        }
        if (obj.gameObject.name == "Border_Right")
        {
            rightPushing_Locked = true;
        }

        if (obj.gameObject.name == "right_Side")
        {
            pushingFrom_Right = true;
        }
        if (obj.gameObject.name == "left_Side")
        {
            pushingFrom_Left = true;
        }
        if (obj.gameObject.name == "up_Side")
        {
            pushingFrom_Up = true;
        }
        if (obj.gameObject.name == "down_Side")
        {
            pushingFrom_Down = true;
        }
    }
    void OnTriggerExit(Collider obj)
    {

        if (obj.gameObject.name == "Border_Up")
        {
            upPushing_Locked = false;
        }
        if (obj.gameObject.name == "Border_Left")
        {
            leftPushing_Locked = false;
        }
        if (obj.gameObject.name == "Border_Down")
        {
            downPushing_Locked = false;
        }
        if (obj.gameObject.name == "Border_Right")
        {
            rightPushing_Locked = false;
        }

        if (obj.gameObject.name == "right_Side")
        {
            pushingFrom_Right = false;
        }
        if (obj.gameObject.name == "left_Side")
        {
            pushingFrom_Left = false;
        }
        if (obj.gameObject.name == "up_Side")
        {
            pushingFrom_Up = false;
        }
        if (obj.gameObject.name == "down_Side")
        {
            pushingFrom_Down = false;
        }
    }

    public bool lock_rightPushing;
    public bool lock_leftPushing;
    public bool lock_upPushing;
    public bool lock_downPushing;

    public void Push(int n)
    {
        if (n == 6)
        {
            if (lock_rightPushing == false)
            {
                rightPushing_Locked = false;
            }
            else
            {
                rightPushing_Locked = true;
            }
        }
        else
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
            }
            else
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
                }
                else
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
        }
        else
            if (n == 4)
            {
                leftPushing_Locked = true;
            }
            else
                if (n == 8)
                {
                    upPushing_Locked = true;
                }
                else
                {
                    downPushing_Locked = true;
                }
    }

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
            lock_rightPushing = false;
            Push(6);
        }
        else
            if (n == 4)
            {
                lock_leftPushing = false;
                Push(4);
            }
            else
                if (n == 8)
                {
                    lock_upPushing = false;
                    Push(8);
                }
                else
                {
                    lock_downPushing = false;
                    Push(2);
                }
    }


    public void Press(int i, bool n)
    {
        if (i == 0)
        {
            userPressed_Right = n;
        }
        else if (i == 1)
        {
            userPressed_Up = n;
        }
        else if (i == 2)
        {
            userPressed_Left = n;
        }
        else
        {
            userPressed_Down = n;
        }
    }

    public void Death()
    {
        Debug.Log("Sphere - Play_Animation (Death by Water).\n");

        rightPushing_Locked = true;
        leftPushing_Locked = true;
        upPushing_Locked = true;
        downPushing_Locked = true;
    }
}
