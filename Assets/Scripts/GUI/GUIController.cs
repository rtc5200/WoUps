using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GUIController : MonoBehaviour {

	[SerializeField]
	private GameObject MainMenu;
	[SerializeField]
	private GameObject PlayButton;
	[SerializeField]
	private GameObject DemoButton;
	[SerializeField]
	private GameObject AISelectMenu;

	public static List<GameObject> loadedShipsList;
	private List<GameObject> secAreaIconList;
	[SerializeField]
	private Sprite defaultIcon;
	
	public static List<GameObject> selectedShipsList;
	private List<GameObject> queueListElementList;

	public static bool isDemoMode = false;
	

	void Start(){
		loadedShipsList = Resources.LoadAll<GameObject>("Ships").ToList();
		secAreaIconList = new List<GameObject>();
		selectedShipsList = new List<GameObject>();
		queueListElementList = new List<GameObject>();
		for(int i = 0; i < loadedShipsList.Count && i < 12; i++){
			secAreaIconList.Add(AISelectMenu.transform.Find("SelectArea").Find("Icon" + i).gameObject);
			secAreaIconList[i].GetComponent<Image>().sprite = loadedShipsList[i].GetComponent<ShipStats>().icon ?? defaultIcon;
		}
		for(int i = 0; i < 5; i++){
			queueListElementList.Add(AISelectMenu.transform.Find("QueueArea").Find("QueueElement" + i).gameObject);
		}

		AISelectMenu.SetActive(false);
	}
	
	public void onPlayButtonClick(){
		MainMenu.SetActive(false);
		AISelectMenu.SetActive(true);
		selectedShipsList.Clear();
		foreach(var e in queueListElementList){
			e.transform.Find("Icon").GetComponent<Image>().sprite = null;
			e.transform.Find("Name").GetComponent<Text>().text = "None";
		}
		
	}
	public void onDemoButtonClick(){
		int maxnum = loadedShipsList.Count > 5 ? 5 : loadedShipsList.Count;
		int num = Random.Range(2,maxnum);
		selectedShipsList.Clear();
		for(int i = 0; i < num;i++){
			var e = getRandomListElement(loadedShipsList);
			while(e.GetComponent<ShipStats>().shipName == "Player" ||
				e.GetComponent<ShipStats>().shipName == "StuckAI"){
				e = getRandomListElement(loadedShipsList);
			}
			selectedShipsList.Add(e);
		}
		isDemoMode = true;
		SceneManager.LoadScene("Main");
	}

	public static T getRandomListElement<T>(List<T> list){
		return list[ UnityEngine.Random.Range(0, list.Count) ];
	}
	public void onExitButtonClick(){
		Application.Quit();
	}
	public void onSecAreaImageClick(int index){
		if(loadedShipsList.Count > index){
			if(selectedShipsList.Count < 5){
				var ship = loadedShipsList[index];
				selectedShipsList.Add(ship);
				var elem = queueListElementList[selectedShipsList.Count - 1];
				elem.transform.Find("Icon").GetComponent<Image>().sprite = loadedShipsList[index].GetComponent<ShipStats>().icon ?? defaultIcon;
				elem.transform.Find("Name").GetComponent<Text>().text = loadedShipsList[index].GetComponent<ShipStats>().shipName ?? "unknownName";
			}
		}
	}
	public void onSecAreaDeleteClick(){
		if(selectedShipsList.Count > 0){
			selectedShipsList.RemoveAt(selectedShipsList.Count - 1);
			var elem = queueListElementList[selectedShipsList.Count];
			elem.transform.Find("Icon").GetComponent<Image>().sprite = null;
			elem.transform.Find("Name").GetComponent<Text>().text = "None";
		}
	}
	public void onBackClick(){
		AISelectMenu.SetActive(false);
		MainMenu.SetActive(true);
	}

	public void onStartClick(){
		if(selectedShipsList.Count > 1){
			isDemoMode = false;
			SceneManager.LoadScene("Main");
		}
	}
}
