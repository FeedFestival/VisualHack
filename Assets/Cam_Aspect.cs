using UnityEngine;
using System.Collections;

public class Cam_Aspect : MonoBehaviour {

	// we'll try to calculate what's the biggest resolution we can accomplish
	// to work in the desired aspect ratio
	void Awake () {	
		float targetAspectWidth = 16;
		float targetAspectHeight = 9;
		// get screen size
		float sw  = Screen.width;
		float sh  = Screen.height;
		
		// let's check if we should modify the height first...
		// calculate the targeted size height
		float th = sw * (targetAspectHeight / targetAspectWidth);
		
		// these variables will hold the percentage of height or width we need to
		// apply to the camera.rect property
		// by default, we set them up in 1.0
		float ptw = 1.0f;
        //float pthat = 1.0f;
		
		// these variables will help us adjust the margin to center the screen
		float tx  = 0.0f;
		float ty = 0.0f;
		float half  = 0.0f;
		
		// let's try the height...
		// to do this, we check how much the targeted height represents on the screen height
		// so, if the result is greater than one, it means the height should not be modified since
		// the width is the one needing to be adjusted
		float pth = th / sh;
		
		// check if either the height or the width needs to be adjusted
		if (pth > 1.0f)
		{
			// since the result was greater than 1.0, we'll work on the width
			// we do the same thing as above, but with the width
			float tw = sh * (targetAspectWidth / targetAspectHeight);
			ptw = tw / sw;
			
			// get half of the percentage we're taking from the width
			half = (1.0f - ptw) / 2.0f;
			
			// adjust the margin
			tx = half;
			
		}
		else
		{
			// get half of the percentage we're taking from the height
			half = (1.0f - pth) / 2.0f;
			
			// adjust the margin
			ty = half;
		}
		
		
		// apply the camera.rect	
		Rect r = new Rect();
		r.x = tx;
		r.y = ty;
		r.width = ptw;
		r.height = pth;
		GetComponent<Camera>().rect = r;
	}
}
