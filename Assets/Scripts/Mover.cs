using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
	[SerializeField]
	public float speed;
	[SerializeField]
	public GameObject parentShip;
	[SerializeField]
	public GameObject explosionParticle;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().velocity = transform.forward * speed; // Bullet 6.4 /call Torpedo 0.008 /call
	}
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Bullet" || other.gameObject.tag == "Torpedo"){
			var expl = Instantiate(explosionParticle,other.gameObject.transform.position, Quaternion.identity);
			Destroy(this.gameObject);
			Destroy(expl,2f);
		}
	}
}
