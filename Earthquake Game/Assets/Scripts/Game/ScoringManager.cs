using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringManager : MonoBehaviour {

    public Text scoreText;

    private GameObject[] buildings;

	// Use this for initialization
	void Start () {
        scoreText.canvasRenderer.SetAlpha(0);
	}

    public void displayScore()
    {
        float score = computeScore();
        //display in the UI
        scoreText.canvasRenderer.SetAlpha(1);
        scoreText.text = "Score: " + (int) score;
        scoreText.CrossFadeAlpha(0, 5f, true);
    }

    private float computeScore()
    {
        buildings = GameObject.FindGameObjectsWithTag("Building");
        float score = 0;
        foreach (GameObject building in buildings)
        {
            score += building.GetComponent<BuildingHealth>().getHealthPercentage();
            //Debug.Log("score: " + score + ", healthPercentage: " + building.GetComponent<BuildingHealth>().getHealthPercentage());
        }
        score *= 100;
        score += gameObject.GetComponent<BudgetManager>().getLeftMoney();
        return score;
    }
}
