using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
	
	void Start (){
		this.transform.position = new Vector3 (Mathf.Round ( this.transform.position.x),
											   Mathf.Round ( this.transform.position.y),
														this.transform.position.z);
	}

	void OnTriggerEnter (Collider obj){
		if (obj.gameObject.name == "Coll"){
			obj.gameObject.transform.parent.GetComponent<Sfera>().Death(0);
		}
		if ((obj.gameObject.name == "right_Box")||(obj.gameObject.name == "left_Box")
			||(obj.gameObject.name == "up_Box")||(obj.gameObject.name == "down_Box")){
			
			obj.gameObject.transform.parent.GetComponent<Box>().Destroyed(0);
			Destroy( this.gameObject.GetComponent<Water>());
		}
	}
}
