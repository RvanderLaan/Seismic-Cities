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
        Marl, Limestone, Sand, Sandstone, Clay, Bedrock, Quicksand, RockSalt,
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
                    info[2] = @"
Bedrock is the hardest layer, that normally lies underneath the more softer soils. It has been <b>compressed</b> into one by the pressure of the layers above. 
This is a kind of stiffest soil where an earthquake can easily pass through. 
";
                    break;
                }
            case SoilType.Marl: {
                    info[0] = "Marl";
                    info[1] = "Mergel";
                    info[2] = @"
Marl consists mostly of <b>mud</b> or mudstone, which contains variable amounts of clays and silt. 
This is a kind of stiff soil where an earthquake can easily pass through, with little amplification effects .
                    ";
                    break;
                }
            case SoilType.Limestone: {
                    info[0] = "Limestone";
                    info[1] = "Kalksteen";
                    info[2] = @"
Limestone is a <b>sedimentary</b> rock, which means that it is formed in water. It consists of shells, minerals and coral. 
This is a soft rock layer, which slows down the waves but increases intensity."
;
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
Sandstone is a bit more porous compared to rocksalt. Therefore gas has been able to develop in between the layer. 
The gas is contained by a denser soil layer on top of it. This layer therefore intensifies the earthquake, as the gas releases, the soil becomes more unstable. 
                    ";
                    break;
                }
            case SoilType.Clay: {
                    info[0] = "Clay";
                    info[1] = "Klei";
                    info[2] = @"
Clay is a <b>fine-grained</b> natural soil material. Combined with the groundwater it acts elastic. 
This means that the earthquake will be amplified when it passes through. 
                    ";
                    break;
                }
            case SoilType.Quicksand: {
                    info[0] = "Quicksand";
                    info[1] = "Drijfzand";
                    info[2] = @"
Quicksand is a <b>water saturated</b> layer. A special effect occurs here when an earthquake hits, called liquefaction. 
Liquefaction means that the ground will start to act like a liquid, that will cause heavy objects like buildings and cars to sink. 
                    ";
                    break;
                }
            case SoilType.RockSalt: {
                    info[0] = "Rock Salt";
                    info[1] = "Zoutsteen";
                    info[2] = @"
Rock salt occurs in vast beds of sedimentary evaporite minerals that result from the drying up of enclosed lakes, playas, and seas. 
It is a stiff layer, and has a little damping effect on the earthquake.
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


