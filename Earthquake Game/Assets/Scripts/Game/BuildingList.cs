using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingList : MonoBehaviour {

    public GameObject buildingItemContainer;
    public GameObject buildingButton;
    public BuildingPlacer buildingPlacer;
    public List<BuildingItem> buildingItems;
    public GameObject tooltipPrefab;
    
	// Use this for initialization
	void Start () {
		// Add buttons to GUI
        foreach(BuildingItem bi in buildingItems) {
            GameObject buttonInstance = GameObject.Instantiate(buildingButton, buildingItemContainer.transform);

            // Clicky stuff
            Button b = buttonInstance.GetComponentInChildren<Button>();
            b.GetComponent<Image>().sprite = bi.image;
            b.GetComponentInChildren<Text>().text = bi.name;

            Text amount = buttonInstance.transform.FindChild("Amount").GetComponentInChildren<Text>();
            amount.text = bi.amount + "";
            // Add onclick listener. The amount is changed in the building placer
            b.onClick.AddListener(() => buildingPlacer.startPreview(bi.prefab, amount));

            // Tooltip
            GameObject tooltip = Instantiate(tooltipPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
            Text[] tooltipTexts = tooltip.GetComponentsInChildren<Text>();
            tooltipTexts[0].text = bi.name;
            tooltipTexts[1].text = bi.tooltipText;
            tooltip.SetActive(false); // disabled by default

            // Only show on hover
            EventTrigger buttonTrigger = b.GetComponentInChildren<EventTrigger>();

            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((e) => tooltip.SetActive(true));
            buttonTrigger.triggers.Add(enterEntry);

            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((e) => tooltip.SetActive(false));
            buttonTrigger.triggers.Add(exitEntry);

            // Follow position of button
            tooltip.GetComponent<Follow2D>().target = buttonInstance.GetComponent<RectTransform>();
        }
	}
}
