using UnityEngine;
using System.Collections;

public class right_Box : MonoBehaviour {

	void OnTriggerEnter (Collider obj){
//		Debug.Log(obj.gameObject.name);
		if (obj.gameObject.name == "Coll"){
			this.gameObject.transform.parent.GetComponent<Box>().player = obj.transform.parent.gameObject;
			this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping = 0;
			
			if (this.gameObject.transform.parent.GetComponent<Box>().touch_Box_left == false){
				this.gameObject.transform.parent.GetComponent<Box>().Impinge(1,false);
				this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(1,false);
			} else { 
				
				this.gameObject.transform.parent.GetComponent<Box>().Impinge(1,true);
				this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(1,true);
			}
		}
		// daca atingem cu partea dreapta, partea stanga a unui box - 
		if (obj.gameObject.name == "left_Box"){
			
			if ((this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 2)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 3)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 4)){
				
				if (this.gameObject.transform.parent.GetComponent<Box>().player != null){
					this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(0,true);
				}
			
			}
			
			this.gameObject.transform.parent.GetComponent<Box>().touch_Box_right = true;
			this.gameObject.transform.parent.GetComponent<Box>().Impinge(0,true);
			
		}
		
		if (obj.gameObject.name == "Border_Right"){
			if ((this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 2)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 3)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 4)){
				
				if (this.gameObject.transform.parent.GetComponent<Box>().player != null){
					this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(0,true);
				}
			
			}
			
			this.gameObject.transform.parent.GetComponent<Box>().touch_Box_right = true;
			this.gameObject.transform.parent.GetComponent<Box>().Impinge(0,true);
			
		}
	}
	void OnTriggerExit (Collider obj){
		
		if (obj.gameObject.name == "Coll"){
			this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge (1,false);
			this.gameObject.transform.parent.GetComponent<Box>().player = null;
			this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping = 4;
			
			this.gameObject.transform.parent.GetComponent<Box>().Impinge(1,true);
		}
		if (obj.gameObject.name == "left_Box"){
			this.gameObject.transform.parent.GetComponent<Box>().touch_Box_right = false;
			
		}
		
	}
}
