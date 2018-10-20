using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
	[SerializeField]
	private GameObject bullet;

	[SerializeField]
	private GameObject afterEffect;

	private Transform muzzle;
	private Vector3 targetPos;

	private bool AutoCtrEnabled = false;

	public bool AutoCtrlEnabled{
		get{return AutoCtrEnabled;}
	}


	private float rotationSpeed;

	public float RotationSpeed{
		get{
			return rotationSpeed;
		}
		set{
			var clamped = Mathf.Clamp(value,-1f,1f);
			if(clamped != value)Debug.LogError("Gun Rotation Speed is Out of Range !!");
			rotationSpeed = clamped;
		}
	}
	private float fireDeltaTime = 3f;

	public float coolTimeLeft{
		get{ return Mathf.Clamp(nextFire - Time.time,0f,Mathf.Infinity); }
	}
	private float nextFire;

	void Awake(){
		muzzle = transform.Find("muzzle");
		nextFire = Time.time;
	}

	public void Rotate(){
		if(AutoCtrEnabled){
			var dif = targetPos - muzzle.position;
			var angle = Vector3.Angle(muzzle.forward, dif); // 0~180 degree
			if(angle < 1f){
				rotationSpeed = Vector3.Cross(muzzle.forward,dif).y > 0 ? angle : -angle;
			}else{
				rotationSpeed = Vector3.Cross(muzzle.forward,dif).y > 0 ? 1f : -1f;
			}
		}
		transform.Rotate(Vector3.up,rotationSpeed);// speed 1f = 1 degree / call
	}

	public void StartAutoCtrl(Vector3 pos){
		targetPos = pos;
		AutoCtrEnabled = true;
	}
	public void StopAutoCtrl(){
		AutoCtrEnabled = false;
	}

	public bool Fire(){
		if(Time.time > nextFire){
			nextFire = Time.time + fireDeltaTime;
			if(isFireable()){
				var bulletObj = Instantiate(bullet, muzzle.position, muzzle.rotation);
				bulletObj.GetComponent<Mover>().parentShip = transform.parent.gameObject;
				Destroy(Instantiate(afterEffect,muzzle),1f);
				return true;
			}
		}
		return false;
	}

	public bool isFireable(){
		muzzle = transform.Find("muzzle");
		var ray = new Ray(muzzle.position,muzzle.forward);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 15.0f)){
			if(hit.collider.isTrigger){
				return false;
			}
		}
		return true;
	}
}
