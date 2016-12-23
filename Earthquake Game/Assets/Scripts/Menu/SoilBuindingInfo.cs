using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilBuindingInfo : MonoBehaviour {

    public GameObject marlPanel;
    public GameObject limestonePanel;

    public GameObject housePanel;
    public GameObject skyscraperPanel;
	
    public void showMarlInfo()
    {
        marlPanel.SetActive(true);
    }

    public void hideMarlInfo()
    {
        marlPanel.SetActive(false);
    }

    public void showLimestoneInfo()
    {
        limestonePanel.SetActive(true);
    }

    public void hideLimestoneInfo()
    {
        limestonePanel.SetActive(false);
    }

    public void showBuildingInfoPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void hideBuildingInfoPanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
