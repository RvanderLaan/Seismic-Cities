using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]
public class SoilInfo : MonoBehaviour {

    public SoilInfoPanel soilInfoPanel;

    private Text ownText;
    private Button button;

    public enum SoilType {
        Marl, Limestone, Sand, Sandstone, Clay, Bedrock, Quicksand,
    }

    public SoilType soilType;

    // Use this for initialization
    void Start () {
        ownText = GetComponentInChildren<Text>();
        button = GetComponentInChildren<Button>();

        if (soilInfoPanel == null)
            soilInfoPanel = GameObject.Find("SoilInfoPanel").GetComponent<SoilInfoPanel>();
	}
	
	void Update () {
        // Update own name and onclick behaviour (-> show popup)
        string[] info = getInfo(soilType);
        ownText.text = info[0];
        gameObject.name = info[0];

        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => soilInfoPanel.show(info[0], info[1], info[2]));
    }

    string[] getInfo(SoilType type) {
        string[] info = new string[3];

        switch (type) {
            case SoilType.Bedrock: {
                    info[0] = "Bedrock";
                    info[1] = "Grondgesteente";
                    info[2] = "Bedrock description";
                    break;
                }
            case SoilType.Marl: {
                    info[0] = "Marl";
                    info[1] = "Mergel";
                    info[2] = @"
Marl consists mostly of <b>mud</b> or mudstone

<Effect on earthquakes> 
                    ";
                    break;
                }
            case SoilType.Limestone: {
                    info[0] = "Limestone";
                    info[1] = "Kalksteen";
                    info[2] = @"
Limestone is a <b>sedimentary</b> rock, which means that it is formed in water. ... shells, minerals, coral... calcium carbonate...

<Effect on earthquakes>";
                    break;
                }
            case SoilType.Sand: {
                    info[0] = "Sand";
                    info[1] = "Zand";
                    info[2] = @"

                    ";
                    break;
                }
            case SoilType.Sandstone: {
                    info[0] = "Sandstone";
                    info[1] = "Zandsteen";
                    info[2] = @"

                    ";
                    break;
                }
            case SoilType.Clay: {
                    info[0] = "Clay";
                    info[1] = "Klei";
                    info[2] = @"

                    ";
                    break;
                }
            case SoilType.Quicksand: {
                    info[0] = "Quicksand";
                    info[1] = "Drijfzand";
                    info[2] = @"

                    ";
                    break;
                }
            default: {
                    info[0] = "?";
                    info[1] = "?";
                    info[2] = "?";
                    break;
                }
        }


        return info;
    }
}


