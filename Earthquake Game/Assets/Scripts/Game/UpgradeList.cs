using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeList : MonoBehaviour {

    public GameObject upgradeItemContainer;
    public GameObject upgradeButton;
    public UpgradePlacer upgradePlacer;
    public List<UpgradeItem> upgradeItems;
    public GameObject tooltipPrefab;

    private List<Text> amountTexts = new List<Text>();

    // Use this for initialization
    void Start() {
        // Add buttons to GUI
        foreach (UpgradeItem ui in upgradeItems) {
            GameObject buttonInstance = GameObject.Instantiate(upgradeButton, upgradeItemContainer.transform);

            // Clicky stuff
            Button b = buttonInstance.GetComponentInChildren<Button>();
            b.GetComponent<Image>().sprite = ui.image;
            b.GetComponentInChildren<Text>().text = ui.name;

            Text amount = buttonInstance.transform.FindChild("Amount").GetComponentInChildren<Text>();
            amountTexts.Add(amount);
            amount.text = ui.amount + "";
            // Add onclick listener. The amount is changed in the upgrade placer
            b.onClick.AddListener(() => upgradePlacer.startPreview(ui.prefab, amount));

            // Tooltip
            GameObject tooltip = Instantiate(tooltipPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
            Text[] tooltipTexts = tooltip.GetComponentsInChildren<Text>();
            tooltipTexts[0].text = ui.name;
            tooltipTexts[1].text = ui.tooltipText;
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

    public bool finishedPlacing() {
        foreach (Text t in amountTexts) {
            if (t.text != "0")
                return false;
        }
        return true;
    }
}
