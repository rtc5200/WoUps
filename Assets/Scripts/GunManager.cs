using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour {

	private GunController gun1;
	private GunController gun2;

	// Use this for initialization
	void Awake () {
		gun1 = transform.Find("gun1").GetComponent<GunController>();
		gun2 = transform.Find("gun2").GetComponent<GunController>();
	}
	
	public void Rotate(int index){
		switch(index){
			case 0 : gun1.Rotate();break;
			case 1 : gun2.Rotate();break;
			default: Debug.LogError("Index out of Range!!");break;
		}
	}
	public void SetRotationSpeed(int index, float speed){
		switch(index){
			case 0 : gun1.RotationSpeed = speed;break;
			case 1 : gun2.RotationSpeed = speed;break;
			default: Debug.LogError("Index out of Range!!");break;
		}
	}
	public void StartAutoCtrl(Vector3 pos){
		gun1.StartAutoCtrl(pos);
		gun2.StartAutoCtrl(pos);
	}
	public void StopAutoCtrl()=> gun1.StopAutoCtrl();
	public bool Fire(int index){
		switch(index){
			case 0 : return gun1.Fire();
			case 1 : return gun2.Fire();
			default: Debug.LogError("Index out of Range!!");return false;
		}
	}
	public bool Fire()=>Fire(0) && Fire(1);

	public bool isFireable(int index){
		switch(index){
			case 0 : return gun1.isFireable();
			case 1 : return gun2.isFireable();
			default: Debug.LogError("Index out of Range!!");return false;
		}
	}
	public float getCoolTimeLeft(int index){
		switch(index){
			case 0 : return gun1.coolTimeLeft;
			case 1 : return gun2.coolTimeLeft;
			default: Debug.LogError("Index out of Range!!");return Mathf.Infinity;
		}
	}
	public Transform getTransform(int index){
		switch(index){
			case 0 : return gun1.transform;
			case 1 : return gun2.transform;
			default: Debug.LogError("Index out of Range!!");return null;
		}
	}
}
