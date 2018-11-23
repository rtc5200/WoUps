using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	[SerializeField]
	public ScrollRect scl;
	[SerializeField]
	public List<GameObject> shipsPrefabList;
	private List<GameObject> shipsList;
	[SerializeField]
	private GameObject UIShipsListElement;


	[SerializeField]
	private GameObject EndUI;

	private bool isPausing = false;
	public static float gameStartedTime;

	void Awake(){
		Time.timeScale = 1f;
		gameStartedTime = Time.time;
		shipsList = new List<GameObject>();
	}
	void Start () {
		if(shipsPrefabList == null || shipsPrefabList.Count == 0)shipsPrefabList = GUIController.selectedShipsList;
		int id = 1;
		foreach(var obj in shipsPrefabList){
			var pos = new Vector3(Random.Range(-500,500f),0f,Random.Range(-500f,500f));
			var spawned = GameObject.FindGameObjectsWithTag("Player");
			if(spawned != null || spawned.Length == 0){
				bool blocked = false;
				while(true){
					foreach(var spawnedShip in spawned){
						if(Vector3.Distance(spawnedShip.transform.position,pos) > 30f)continue;
						blocked = true;
						break;
					}
					if(!blocked)break;
					pos = new Vector3(Random.Range(-500,500),0f,Random.Range(-500,500));
				}
			}
			var instance = Instantiate(obj,pos,Quaternion.identity);
			instance.SetActive(true);
			instance.transform.LookAt(Vector3.zero);
			var stats = instance.GetComponent<ShipStats>();
			stats.ID = id;
			//if(stats.shipName == null || stats.shipName.Length == 0)stats.shipName = "AI" + id;
			stats.name = stats.name ?? "AI" + id;
			instance.name = stats.shipName;
			shipsList.Add(instance);

			var item  = Instantiate(UIShipsListElement);
			item.GetComponent<UIShipsElementUpdater>().myShip = instance;
			item.transform.SetParent(scl.content.transform,false);
			id++;
		}
		GameObject.Find("Canvas").transform.Find("DemoModeText").gameObject.SetActive(GUIController.isDemoMode);
		gameStartedTime = Time.time;	
	}
	void Update(){
		if(!isPausing){
			checkEnd();
			GameObject.Find("Time").GetComponent<Text>().text = string.Format("Time : {0:0.0}",Time.time -gameStartedTime);
		}
		if(Input.GetKey(KeyCode.Escape)){
			onBacktoMenuClick();
		}
	}

	public void checkEnd(){
		int liveCount = 0;
		foreach(var ship in shipsList){
			if(ship.activeSelf){
				liveCount++;
			}
			if(liveCount > 1)return;
		}
		if(!isPausing){
			EndGame();
		}
	}

	private void EndGame(){
		isPausing = true;
		Time.timeScale = 0;
		var winner = GameObject.FindWithTag("Player");
		if(winner is GameObject){
			EndUI.SetActive(true);
			if(GUIController.isDemoMode){
				EndUI.transform.Find("Button").gameObject.SetActive(false);
			}
			var stats = winner.GetComponent<ShipStats>();
			EndUI.transform.Find("Winner").Find("Name").GetComponent<Text>().text = stats.shipName;
			if(stats.icon != null)EndUI.transform.Find("Winner").Find("Icon").GetComponent<Image>().sprite = stats.icon;
		}
		if(GUIController.isDemoMode){
			StartCoroutine("DemoModeRestart");
		}
	}

	private IEnumerator DemoModeRestart(){
		yield return new WaitForSecondsRealtime(3f);
		int maxnum = GUIController.loadedShipsList.Count > 5 ? 5 : GUIController.loadedShipsList.Count;
		int num = Random.Range(2,maxnum);
		GUIController.selectedShipsList.Clear();
		for(int i = 0; i < num;i++){
			var e = GUIController.getRandomListElement(GUIController.loadedShipsList);
			while(e.GetComponent<ShipStats>().name.Equals("PlayerShip") ||
				e.GetComponent<ShipStats>().name.Equals("StuckAI")){
				e = GUIController.getRandomListElement(GUIController.loadedShipsList);
			}
			GUIController.selectedShipsList.Add(e);
		}
		GUIController.isDemoMode = true;
		SceneManager.LoadScene("Main");
	}

	public void getOtherShipsData(Radar r){
		if(r.OtherShipsData == null || r.OtherShipsData.Count == 0){
			r.OtherShipsData = new List<ShipData>();
			foreach(var ship in shipsList){
				if(!ship.Equals(r.gameObject) && ship.activeSelf){
					r.OtherShipsData.Add(new ShipData(ship));
				}
			}
		}
		for(int i =0;i < r.OtherShipsData.Count;i++){
			ShipData sd = r.OtherShipsData[i];
			if(sd.hp == 0){
				r.OtherShipsData.Remove(sd);
			}else{
				sd.UpdateData();
			}
		}
	}
	
	public void onBacktoMenuClick(){
		SceneManager.LoadScene("Menu");
	}
}


