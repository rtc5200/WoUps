using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipInfoUIManager : MonoBehaviour {

	private Slider HPBar;
	private Text ShipName;
	private ShipStats stats;
	private Image Icon;
	void Start(){
		HPBar = transform.Find("HPBar").GetComponent<Slider>();
		stats = transform.parent.GetComponent<ShipStats>();
		ShipName = transform.Find("ShipName").GetComponent<Text>();
		ShipName.text = transform.parent.name;
		ShipName.color = stats.color;
		Icon = transform.Find("Icon").GetComponent<Image>();
		if(stats.icon != null)Icon.sprite = stats.icon;
	}
	
	void Update () {
		UpdateTransform();
		HPBar.value = 100f * stats.HP / stats.maxHP;
	}

	private void UpdateTransform(){
		//transform.LookAt(GameObject.Find("Main Camera").transform);
		//Vector3 angle = GameObject.Find("Main Camera").transform.eulerAngles;
		//angle.y = -angle.y;
		//transform.eulerAngles = angle;
		var camera  = GameObject.Find("Main Camera").transform;
		transform.rotation = camera.rotation;
		//transform.Rotate(0f,0f,180f);
		if(camera.position.y < 50){
			transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		}else{
			var scale = 0.5f + Vector3.Distance(transform.position,camera.position)/1000;
			transform.localScale = Vector3.one * scale;
		}
	}
}
