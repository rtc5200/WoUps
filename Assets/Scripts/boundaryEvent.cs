using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class boundaryEvent : MonoBehaviour {
	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Bullet" || other.gameObject.tag == "Torpedo"){
			Destroy(other.gameObject);
			return;
		}
		if(other.gameObject.tag == "Player"){
			//TODO 他の船にかぶらないように
			var pos = new Vector3(Random.Range(-500,500),0f,Random.Range(-500,500));
			var objs = GameObject.FindGameObjectsWithTag("Player");
			objs = objs.Where(e => !e.Equals(other.gameObject)).ToArray();
			bool blocked = false;
			while(true){
				foreach(var obj in objs){
					if(Vector3.Distance(other.gameObject.transform.position, obj.transform.position) > 30f)continue;
					blocked = true;
					break;
				}
				if(!blocked)break;
			}
			other.gameObject.transform.position = pos;
		}		
	}
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			var pos = new Vector3(Random.Range(-500,500),0f,Random.Range(-500,500));
			var objs = GameObject.FindGameObjectsWithTag("Player");
			objs = objs.Where(e => !e.Equals(other.gameObject)).ToArray();
			bool blocked = false;
			while(true){
				foreach(var obj in objs){
					if(Vector3.Distance(pos, obj.transform.position) > 30f)continue;
					blocked = true;
					break;
				}
				if(!blocked)break;
				pos = new Vector3(Random.Range(-500,500),0f,Random.Range(-500,500));
			}
			other.gameObject.transform.position = pos;
		}
	}
}
