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

        string[] info = getInfo(soilType);
        ownText.text = info[0];
        gameObject.name = info[0];


        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => soilInfoPanel.show(info[0], info[1], info[2], info[3]));

        if (soilInfoPanel == null)
            soilInfoPanel = GameObject.Find("SoilInfoPanel").GetComponent<SoilInfoPanel>();
	}
	
	void Update () {
        // Update own name and onclick behaviour (-> show popup)
        
    }

    string[] getInfo(SoilType type) {
        string[] info = new string[4];

        switch (type) {
            case SoilType.Bedrock: {
                    info[0] = "Bedrock";
                    info[1] = "Grondgesteente";
                    info[2] = @"
Bedrock is the hardest layer, that normally lies underneath the more softer soils. It has been <b>compressed</b> into one by the pressure of the layers above. 
This is a kind of stiffest soil where an earthquake can easily pass through. 
";
                    info[3] = @"
Grondgesteente is de hardste laag, die normaal gesproken onder de zachtere lagen ligt. Het is in elkaar gedrukt door de druk van de lagen die er bovenop liggen. De aardbeving kan makkelijker door deze grondlaag heen bewegen.";
                    break;
                }
            case SoilType.Marl: {
                    info[0] = "Marl";
                    info[1] = "Mergel";
                    info[2] = @"
Marl consists mostly of <b>mud</b> or mudstone, which contains variable amounts of clays and silt. 
This is a kind of stiff soil where an earthquake can easily pass through, with little amplification effects.
                    ";
                    info[3] = @"
Mergel​ ​bestaat​ ​voor​ ​het​ ​grootste​ ​deel​ ​uit​ ​<b>modder</b>,​ ​wat​ ​bestaat​ ​uit​ ​verschillende​ ​lagen van​ ​klei​ ​en​ ​silt.​ 
​Dit​ ​is​ ​een​ ​stevige​ ​bodemlaag​ ​waar​ ​een​ ​aardbeving​ ​makkelijk​ ​doorheen beweegt,​ ​met​ ​weinig​ ​versterkende​ ​effecten.";
                    break;
                }
            case SoilType.Limestone: {
                    info[0] = "Limestone";
                    info[1] = "Kalksteen";
                    info[2] = @"
Limestone is a <b>sedimentary</b> rock, which means that it is formed in water. It consists of shells, minerals and coral. 
This is a soft rock layer, which slows down the waves but increases intensity."
;
                    info[3] = @"
Kalksteen is een <b>rotslaag</b> die is opgebouwd van natuurlijke stoffen die zijn gevormd in het water. Het bestaat uit schelpen, mineralen en koraal.
Deze deeltjes zijn zo hard op elkaar gedrukt dat ze een gesteente zijn geworden. Het is een zachte soort rotslaag, waar de snelheid van de aardbeving door wordt afgenomen, maar die de intensiteit nog hoger maakt.";
                    break;
                }
            case SoilType.Sand: {
                    info[0] = "Sand";
                    info[1] = "Zand";
                    info[2] = @"

                    ";
                    info[3] = "";
                    break;
                }
            case SoilType.Sandstone: {
                    info[0] = "Sandstone";
                    info[1] = "Zandsteen";
                    info[2] = @"
Sandstone is a bit more porous compared to rocksalt. Therefore gas has been able to develop in between the layer. 
The gas is contained by a denser soil layer on top of it. This layer therefore intensifies the earthquake, as the gas releases, the soil becomes more unstable. 
                    ";
                    info[3] = @"
Zandsteen is wat poreuzer dan zoutsteen. Daardoor kan het gas dat zich heeft ontwikkelt in de laag gevangen blijven, omdat het niet kan ontsnappen door de zoutsteenlaag. De gassen die hier zijn gewonnen laten ruimtes over in de grondlaag. Als er een aardbeving doorheen gaat, zullen deze instorten, waardoor de intensiteit van de aardbeving wordt versterkt.";
                    break;
                }
            case SoilType.Clay: {
                    info[0] = "Clay";
                    info[1] = "Klei";
                    info[2] = @"
Clay is a <b>fine-grained</b> natural soil material. Combined with the groundwater it acts elastic. 
This means that the earthquake will be amplified when it passes through. 
                    ";
                    info[3] = @"
Klei​ ​is​ ​een​ ​natuurlijke​ ​grondlaag​ ​​ ​met​ ​een <b>fijnkorrelige​ ​structuur​</b>.​ ​Gecombineerd​ ​met het​ ​grondwater,​ ​zorgt​ ​dit​ ​ervoor​ ​dat​ ​het​ ​elastisch​ ​wordt​ ​als​ ​er​ ​een​ ​aardbeving​ ​is​ ​veroorzaakt. 
Dit​ ​betekent​ ​dat​ ​het​ ​effect​ ​van​ ​de​ ​aardbeving​ ​wordt​ ​versterkt​ ​als​ ​het​ ​door​ ​de​ ​kleilaag​ ​heen beweegt.";
                    break;
                }
            case SoilType.Quicksand: {
                    info[0] = "Quicksand";
                    info[1] = "Drijfzand";
                    info[2] = @"
Quicksand is a <b>water saturated</b> layer. A special effect occurs here when an earthquake hits, called liquefaction. 
Liquefaction means that the ground will start to act like a liquid, that will cause heavy objects like buildings and cars to sink. 
                    ";
                    info[3] = @"
Drijfzand​ ​is​ ​een​ ​<b>​ ​water​ ​verzadigde​ ​</b>​ ​grondlaag.​ ​Als​ ​er​ ​een​ ​aardbeving​ ​wordt veroorzaakt,​ ​heeft​ ​deze​ ​grond​ ​een​ ​speciaal​ ​effect,​ ​wat​ ​bodemvervloeiing​ ​heet.​ 
​Dit​ ​betekent dat​ ​de​ ​grond​ ​zich​ ​als​ ​een​ ​vloeistof​ ​zal​ ​gaan​ ​gedragen,​ ​waardoor​ ​zwaardere​ ​objecten​ ​als huizen​ ​en​ ​auto’s​ ​in​ ​de​ ​grond​ ​zullen​ ​zinken.";
                    break;
                }
            case SoilType.RockSalt: {
                    info[0] = "Rock Salt";
                    info[1] = "Zoutsteen";
                    info[2] = @"
Rock salt occurs in vast beds of sedimentary evaporite minerals that result from the drying up of enclosed lakes, playas, and seas. 
It is a stiff layer, and has a little damping effect on the earthquake.
                    ";
                    info[3] = @"
Zoutsteen komt voor in plaatsen waar grote hoeveelheden mineralen zijn neergedaald, nadat een meer of zee is verdampt. Het is een harde laag die weinig effect heeft op de aardbeving.";
                    break;
                }
            default: {
                    info[0] = "?";
                    info[1] = "?";
                    info[2] = "?";
                    info[3] = "?";
                    break;
                }
        }


        return info;
    }
}


