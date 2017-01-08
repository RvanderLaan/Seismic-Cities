using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoilInfoPanel : MonoBehaviour {

    public Text title, dutch, description;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void show(string title, string dutch, string description) {
        this.title.text = title;
        this.dutch.text = dutch;
        this.description.text = description;

        animator.SetBool("active", true);
    }

    public void hide() {
        animator.SetBool("active", false);
    }
}
