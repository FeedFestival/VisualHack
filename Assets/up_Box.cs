using UnityEngine;
using System.Collections;

public class up_Box : MonoBehaviour {

	void OnTriggerEnter (Collider obj){
		
		if (obj.gameObject.name == "Coll"){
			this.gameObject.transform.parent.GetComponent<Box>().player = obj.transform.parent.gameObject;
			this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping = 3;
			
			if (this.gameObject.transform.parent.GetComponent<Box>().touch_Box_down == false){
				this.gameObject.transform.parent.GetComponent<Box>().Impinge(2,false);
				this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(2,false);
			} else { 
				
				this.gameObject.transform.parent.GetComponent<Box>().Impinge(2,true);
				this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(2,true);
			}
		}
		if (obj.gameObject.name == "down_Box"){
			if ((this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 0)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 1)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 4)){
			
				if (this.gameObject.transform.parent.GetComponent<Box>().player != null){
					this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(3,true);
				}
			}
			
			this.gameObject.transform.parent.GetComponent<Box>().touch_Box_up = true;
			this.gameObject.transform.parent.GetComponent<Box>().Impinge(3,true);
			
		}
		if (obj.gameObject.name == "Border_Up"){
			if ((this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 0)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 1)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 4)){
			
				if (this.gameObject.transform.parent.GetComponent<Box>().player != null){
					this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(3,true);
				}
			}
			
			this.gameObject.transform.parent.GetComponent<Box>().touch_Box_up = true;
			this.gameObject.transform.parent.GetComponent<Box>().Impinge(3,true);
			
		}
	}
	void OnTriggerExit (Collider obj){
		
		if (obj.gameObject.name == "Coll"){
			this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge (2,false);
			this.gameObject.transform.parent.GetComponent<Box>().player = null;
			this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping = 4;
			
			this.gameObject.transform.parent.GetComponent<Box>().Impinge(2,true);
		}
		if (obj.gameObject.name == "down_Box"){
			this.gameObject.transform.parent.GetComponent<Box>().touch_Box_up = false;
			
		}
	}
}
