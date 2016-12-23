using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BudgetManager : MonoBehaviour {

    public int startingMoney;
    public int targetPopulation;

    public Text moneyText;
    public Text populationText;
    public Text notEnoughMoneyText;

    public Color redColor;

    private int actualMoney;
    private int actualPopulation;

	// Use this for initialization
	void Start () {
        actualMoney = startingMoney;
        actualPopulation = 0;
        //updateGUI();

        notEnoughMoneyText.canvasRenderer.SetAlpha(0);
    }

    public void newBuilding(int cost, int people)
    {
        if (enoughMoney(cost))
        {
            actualMoney -= cost;
            actualPopulation += people;
            //updateGUI();
        }
    }

    public bool enoughMoney(int cost)
    {
        return actualMoney >= cost;
    }

    public bool enoughPopulation()
    {
        return actualPopulation >= targetPopulation;
    }

    private void updateGUI()
    {
        moneyText.text = "Money = " + actualMoney + "/" + startingMoney;
        populationText.text = "Population = " + actualPopulation + "/" + targetPopulation;
    }

    public void notEnoughMoneyMessage()
    {
        //Debug.Log("notEnoughMoneyMessage()");
        notEnoughMoneyText.canvasRenderer.SetAlpha(1);
        notEnoughMoneyText.CrossFadeAlpha(0, 2, true);
    }

    public int getLeftMoney()
    {
        return actualMoney;
    }
}
