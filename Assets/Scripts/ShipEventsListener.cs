using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEventsListener : MonoBehaviour {
	[SerializeField]
	public GameObject smokePrefab;
	[SerializeField]
	public GameObject firePrefab;
	[SerializeField]
	public GameObject BigExplosionPrefab;
	[SerializeField]
	public GameObject explosionParticle;

	[SerializeField]
	private AudioClip damageExpSound;

	private bool smokeEnabled = false;
	private bool fireEnabled = false;
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag == "Player"){
			//Debug.Log("CE");
		} 
	}
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Bullet" 
		&& !other.gameObject.GetComponent<Mover>().parentShip.Equals(gameObject)){
			var expl = Instantiate(explosionParticle,other.gameObject.transform.position,Quaternion.identity);
			Destroy(other.gameObject);
			Destroy(expl,2f);
			GetComponent<AudioSource>().PlayOneShot(damageExpSound);
			gameObject.GetComponent<ShipStats>().HP -= 10;
		}else if(other.gameObject.tag == "Torpedo" 
		&& !other.gameObject.GetComponent<Mover>().parentShip.Equals(gameObject)){
			var expl = Instantiate(explosionParticle,other.gameObject.transform.position,Quaternion.identity);
			Destroy(other.gameObject);
			Destroy(expl,2f);
			GetComponent<AudioSource>().PlayOneShot(damageExpSound);
			gameObject.GetComponent<ShipStats>().HP -= 50;
		}
		GameObject.Find("GameController").GetComponent<GameController>().checkEnd();
	}

	void Update(){
		if(gameObject.GetComponent<ShipStats>().HP <= 0){
			gameObject.SetActive(false);
			var BigExpl = GameObject.Instantiate(BigExplosionPrefab,gameObject.transform.position,Quaternion.identity);
			BigExpl.transform.localScale = new Vector3(30,30,30);
			Destroy(BigExpl,3f);
		}else if(gameObject.GetComponent<ShipStats>().HP <= 30 && !fireEnabled){
			var fire1 = GameObject.Instantiate(firePrefab,gameObject.transform.GetChild(1).transform);
			//GameObject fire2 = GameObject.Instantiate(firePrefab,gameObject.transform.GetChild(7).transform);
			fire1.transform.localScale = new Vector3(20,20,20);
			//fire2.transform.localScale = new Vector3(25,25,25);
			fireEnabled = true;
		}else if(gameObject.GetComponent<ShipStats>().HP <= 60 && !smokeEnabled){
			var smoke1 = GameObject.Instantiate(smokePrefab,gameObject.transform.GetChild(1).transform);
			var smoke2 = GameObject.Instantiate(smokePrefab,gameObject.transform.GetChild(7).transform);
			smoke1.transform.localScale = new Vector3(25,25,25);
			smoke2.transform.localScale = new Vector3(25,25,25);
			smokeEnabled = true;
		}
	}
}
