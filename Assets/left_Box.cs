using UnityEngine;
using System.Collections;

public class left_Box : MonoBehaviour {

	void OnTriggerEnter (Collider obj){
		
		if (obj.gameObject.name == "Coll"){
			// zic ca ating cu sfera.
			// zic din ce parte.
			this.gameObject.transform.parent.GetComponent<Box>().player = obj.transform.parent.gameObject;
			this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping = 1;
			
			// daca putem impinge - inseamna ca nu se atinge cu nimic la dreapta
			//					- putem impinge boxul
			//					- si sfera
			if (this.gameObject.transform.parent.GetComponent<Box>().touch_Box_right == false){
				this.gameObject.transform.parent.GetComponent<Box>().Impinge(0,false);
				this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(0,false);
			} else { 
				// daca nu putem impinge - nu putem impinge boxul
				//						- nu putem impinge sfera
				this.gameObject.transform.parent.GetComponent<Box>().Impinge(0,true);
				this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(0,true);
			}
						
		}
		
		// daca atingem cu partea stanga, partea dreapta a unui box - 
		if (obj.gameObject.name == "right_Box"){
			if ((this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 2)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 3)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 4)){
				
				if (this.gameObject.transform.parent.GetComponent<Box>().player != null){
					this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(1,true);
				}
			}
			
			this.gameObject.transform.parent.GetComponent<Box>().touch_Box_left = true;
			this.gameObject.transform.parent.GetComponent<Box>().Impinge(1,true); // nu poate trece
			
		}
		//	daca atingem border left
		if (obj.gameObject.name == "Border_Left"){
	//		Debug.Log(obj.gameObject.name);
			if ((this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 2)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 3)
				&&(this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping != 4)){
				
				if (this.gameObject.transform.parent.GetComponent<Box>().player != null){
					this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge(1,true);
				}
			}
			
			this.gameObject.transform.parent.GetComponent<Box>().touch_Box_left = true;
			this.gameObject.transform.parent.GetComponent<Box>().Impinge(1,true); // nu poate trece
			
		}
	}
	void OnTriggerExit (Collider obj){
		
		if (obj.gameObject.name == "Coll"){
			this.gameObject.transform.parent.GetComponent<Box>().player.GetComponent<Sfera>().Impinge (0,false);
			this.gameObject.transform.parent.GetComponent<Box>().player = null;
			this.gameObject.transform.parent.GetComponent<Box>().cu_ce_imping = 4;
			
			this.gameObject.transform.parent.GetComponent<Box>().Impinge(0,true);
		}
		if (obj.gameObject.name == "right_Box"){
			this.gameObject.transform.parent.GetComponent<Box>().touch_Box_left = false;
			
		}
	}
}
