using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoFireStarter : MonoBehaviour {

	[SerializeField]
	public GameObject torpedo;

	private float nextFire;
	public float coolTimeLeft{
		get{return Mathf.Clamp(nextFire - Time.time,0,Mathf.Infinity);}
	}

	private float fireDeltaTime = 10f;

	public void Fire(Vector3 targetPos){
		if(Time.time > nextFire && Time.time - GameController.gameStartedTime > 1f){
			nextFire = Time.time + fireDeltaTime;
			targetPos.y = 0;
			var fireBasePos = new Vector3(transform.position.x, -0.5f, transform.position.z);
			var direction = targetPos - fireBasePos;
			direction.y = 0;
			var vertical = Vector3.Cross(direction, Vector3.up).normalized * 10f;
			var firePos2 = fireBasePos + vertical;
			var firePos3 = fireBasePos - vertical;
			var t1 = Instantiate(torpedo,firePos2,Quaternion.identity);
			var t2 = Instantiate(torpedo,fireBasePos,Quaternion.identity);
			var t3 = Instantiate(torpedo,firePos3,Quaternion.identity);
			t2.transform.LookAt(targetPos);
			t1.transform.rotation = t2.transform.rotation;
			t3.transform.rotation = t2.transform.rotation;
			t1.GetComponent<Mover>().parentShip = transform.gameObject;
			t2.GetComponent<Mover>().parentShip = transform.gameObject;
			t3.GetComponent<Mover>().parentShip = transform.gameObject;
		}
	}
}
