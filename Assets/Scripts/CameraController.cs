using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameObject target;

	[SerializeField, Range(0.1f, 100f)]
	private float wheelSpeed = 1f;

	[SerializeField, Range(0.1f, 100f)]
	private float moveSpeed = 0.3f;

	[SerializeField, Range(0.1f, 10f)]
	private float rotateSpeed = 0.3f;

	private Vector3 preMousePos;


	
	// Update is called once per frame
	void Update () {
		MouseUpdate();
		if(target != null){
			SetFollowPos();
		}
		if(Input.GetKeyDown(KeyCode.LeftControl)){
			target = null;
		}
	}

	void SetFollowPos(){
		if(target != null){
			var tarPos = target.transform.position - 40f * target.transform.forward;
			tarPos.y = transform.position.y;
			transform.position = tarPos;
		}
	}
	private void MouseUpdate()
  {
		float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
		if(scrollWheel != 0.0f)
			target = null;
			MouseWheel(scrollWheel);

		if(Input.GetMouseButtonDown(0) ||
			Input.GetMouseButtonDown(1) ||
			Input.GetMouseButtonDown(2))
			preMousePos = Input.mousePosition;

		MouseDrag(Input.mousePosition);
  }

	private void MouseWheel(float delta)
	{
	transform.position += transform.forward * delta * wheelSpeed;
	return;
	}

	private void MouseDrag(Vector3 mousePos)
	{
	Vector3 diff = mousePos - preMousePos;

	if(diff.magnitude < Vector3.kEpsilon)
	return;

	if(Input.GetMouseButton(2))
	transform.Translate(-diff * Time.deltaTime * moveSpeed);
	else if(Input.GetMouseButton(1))
	CameraRotate(new Vector2(-diff.y, diff.x) * rotateSpeed);

	preMousePos = mousePos;
	}

	public void CameraRotate(Vector2 angle)
	{
		transform.RotateAround(transform.position, transform.right, angle.x);
		transform.RotateAround(transform.position, Vector3.up, angle.y);
	}

	public void SetTarget(GameObject target){
		this.target = target;
		GameObject.Find("Main Camera").transform.rotation = Quaternion.LookRotation(target.transform.forward);
		var pos = transform.position;
		pos.y = 40f;
		transform.position = pos;
	}
}
