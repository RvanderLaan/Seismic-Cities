using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoilInfoPanel : MonoBehaviour {

    public Text title, dutch, description;
    private Animator animator;
    private string englishDescription, dutchDescription;
    private Color initialTitleColor;

    void Start() {
        animator = GetComponent<Animator>();

        initialTitleColor = title.color;
    }

    public void show(string title, string dutch, string description, string dutchDescription) {
        this.title.text = title;
        this.dutch.text = dutch;
        this.description.text = description;

        englishDescription = description;
        this.dutchDescription = dutchDescription;

        animator.SetBool("active", true);
    }

    public void hide() {
        animator.SetBool("active", false);
    }

    public void setDutchDescription()
    {
        description.text = dutchDescription;
    }

    public void setEnglishDescription()
    {
        description.text = englishDescription;
    }

    public void OnEnterEnglish()
    {
        title.color = Color.blue;
    }

    public void OnEnterDutch()
    {
        dutch.color = Color.blue;
    }

    public void OnLeaveEnglish()
    {
        title.color = initialTitleColor;
    }

    public void OnLeaveDutch()
    {
        dutch.color = initialTitleColor;
    }
}
