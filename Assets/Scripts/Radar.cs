using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour {
	/// <summary>
	/// 他の船のデータ(ShipData)のList
	/// </summary>
	[HideInInspector]
	public List<ShipData> OtherShipsData;
	/// <summary>
	/// 砲弾のデータ(BulletData)のList
	/// </summary>
	[HideInInspector]
	public List<BulletData> BulletsData;
	/// <summary>
	/// 魚雷のデータ(TorpedoData)のList
	/// </summary>
	[HideInInspector]
	public List<TorpedoData> TorpedosData;

	public void UpdateData() {
		GameObject.Find("GameController").GetComponent<GameController>().getOtherShipsData(this);
		if(BulletsData == null || BulletsData.Count == 0){
			BulletsData = new List<BulletData>();
		}
		for(int i = 0; i < BulletsData.Count;i++){
			if(BulletsData[i].isDestroyed()){
				BulletsData.RemoveAt(i);
			}
		}
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Bullet")){
			bool found = false;
			foreach(BulletData b in BulletsData){
				if(b.Equals(g)){
					b.UpdateData();
					found = true;
					break;
				}
			}
			if(!found){
				BulletsData.Add(new BulletData(g));
			}
		}
		if(TorpedosData == null || TorpedosData.Count == 0){
			TorpedosData = new List<TorpedoData>();
		}
		for(int i = 0; i < TorpedosData.Count;i++){
			if(TorpedosData[i].isDestroyed()){
				TorpedosData.RemoveAt(i);
			}
		}
		if(TorpedosData == null || TorpedosData.Count == 0){
			TorpedosData = new List<TorpedoData>();
		}
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Torpedo")){
			bool found = false;
			foreach(TorpedoData b in TorpedosData){
				if(b.Equals(g)){
					b.UpdateData();
					found = true;
					break;
				}
			}
			if(!found){
				TorpedosData.Add(new TorpedoData(g));
			}
		}
	}
}
/// <summary>
/// 船が取得可能な発射された魚雷のデータ
/// </summary>
public class TorpedoData : BulletData
{
	public TorpedoData(GameObject torpedo) : base(torpedo){}
}
/// <summary>
/// 取得可能な船のデータの集まり
/// </summary>
public class ShipData{
	public readonly int id;
	public  int hp{
		get;private set;
	}
	public  Vector3 position{
		get;private set;
	}
	public  Vector3 forward{
		get;private set;
	}
	public  Quaternion quaternion{
		get;private set;
	}
	private readonly GameObject ship;
	public ShipData(GameObject ship){
		this.ship = ship;
		ShipStats stats = ship.GetComponent<ShipStats>();
		id = stats.ID;
		hp = stats.HP;
		position = ship.transform.position;
		forward = ship.transform.forward;
		quaternion = ship.transform.rotation;
	}
	public void UpdateData(){
		ShipStats stats = ship.GetComponent<ShipStats>();
		hp = stats.HP;
		position = ship.transform.position;
		forward = ship.transform.forward;
		quaternion = ship.transform.rotation;
	}
}
/// <summary>
/// 船が取得可能な発射された砲弾のデータ
/// </summary>
public class BulletData
{
	public Vector3 position{
		get;private set;
	}
	public Vector3 forward{
		get;private set;
	}
	public Quaternion quaternion{
		get;private set;
	}
	public readonly int shipid;
	protected readonly GameObject obj;

	public BulletData(GameObject bullet){
		this.obj = bullet;
		this.position = bullet.transform.position;
		this.forward = bullet.transform.forward;
		this.quaternion = bullet.transform.rotation;
		this.shipid = bullet.GetComponent<Mover>().parentShip.GetComponent<ShipStats>().ID;
	}
	public void UpdateData(){
		this.position = obj.transform.position;
		this.forward = obj.transform.forward;
		this.quaternion = obj.transform.rotation;
	}
	public override bool Equals(object obj){
		if(obj is GameObject){
			var b = obj as GameObject;
			
			if(b.Equals(this.obj))return true;
		}
		return false;
	}
	public bool isDestroyed(){
		//Debug.Log(obj == null);
		return obj == null || !obj.activeSelf;
	}
}
