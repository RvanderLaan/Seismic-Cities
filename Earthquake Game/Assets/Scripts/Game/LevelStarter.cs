using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStarter : MonoBehaviour {

    public GameObject buildingItemContainer;
    public GameObject buildingButton;
    public BuildingPlacer buildingPlacer;
    public List<BuildingItem> buildingItems;
    
	// Use this for initialization
	void Start () {
		// Add buttons to GUI
        foreach(BuildingItem bi in buildingItems) {
            GameObject buttonInstance = GameObject.Instantiate(buildingButton);
            buttonInstance.transform.SetParent(buildingItemContainer.transform);

            Button b = buttonInstance.GetComponentInChildren<Button>();
            b.GetComponent<Image>().sprite = bi.image;

            b.GetComponentInChildren<Text>().text = bi.name;

            Text amount = buttonInstance.transform.FindChild("Amount").GetComponentInChildren<Text>();
            amount.text = bi.amount + "";
            // Todo: Update amount when clicking

            b.onClick.AddListener(() => buildingPlacer.startPreview(bi.prefab, amount));
        }
	}
}
