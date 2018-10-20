using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIShipsElementUpdater : MonoBehaviour {

	public GameObject myShip;
	private ShipStats stats;
	private Text ID;
	private Text Name;
	private Text HP;
	private Slider HPSlider;
	private Text Cordinate;
	private Slider GunCoolTime0;
	private Slider GunCoolTime1;
	private Slider TorCoolTime;
	private Image Icon;


	// Use this for initialization
	void Start () {
		ID = transform.Find("ID").GetComponent<Text>();
		Name = transform.Find("Name").GetComponent<Text>();
		HP = transform.Find("HP").GetComponent<Text>();
		HPSlider = HP.gameObject.transform.Find("Slider").GetComponent<Slider>();
		Cordinate = transform.Find("Cordinate").GetComponent<Text>();
		transform.Find("Button").GetComponent<Button>().onClick.AddListener(onClick);
		stats = myShip.GetComponent<ShipStats>();
		Name.text = stats.ID + stats.shipName;
		Name.color = stats.color;
		GunCoolTime0 = transform.Find("Gun0").Find("GunCoolTime0").GetComponent<Slider>();
		GunCoolTime1 = transform.Find("Gun1").Find("GunCoolTime1").GetComponent<Slider>();
		TorCoolTime = transform.Find("Torpedo").Find("TorpedoCoolTime").GetComponent<Slider>();
		Icon = transform.Find("Icon").GetComponent<Image>();
		if(stats.icon != null)Icon.sprite = stats.icon;
	}
	
	// Update is called once per frame
	void Update () {
		ID.text = "ID : " + stats.ID;
		Name.text = stats.shipName;
		Name.color = stats.color;
		HP.text = string.Format("HP: {0}({1:0}%)",stats.HP, 100f * stats.HP/stats.maxHP);
		HPSlider.value = (float)stats.HP/(float)stats.maxHP;
		Cordinate.text = string.Format("({0:0},,{1:0})",myShip.transform.position.x,myShip.transform.position.z);
		var gm = myShip.GetComponent<GunManager>();
		GunCoolTime0.value = gm.getCoolTimeLeft(0) / 3f;
		GunCoolTime1.value = gm.getCoolTimeLeft(1) / 3f;
		var fs = myShip.GetComponent<TorpedoFireStarter>();
		TorCoolTime.value = fs.coolTimeLeft / 10f; 
		if(stats.icon != null)Icon.sprite = stats.icon;
	}

	void onClick() => GameObject.Find("Main Camera").GetComponent<CameraController>().SetTarget(myShip);
}
