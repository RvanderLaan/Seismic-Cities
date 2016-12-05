using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountrySelection : MonoBehaviour {

    public Canvas hoverDialog;
    public Canvas levelDetails;


    private Renderer renderer;

    void Start() {
        renderer = GetComponent<Renderer>();
    }

    void Update() {
        hoverDialog.transform.rotation = Quaternion.identity;
    }

    private void OnMouseEnter()
    {
        // hoverDialog.gameObject.SetActive(true);
        renderer.material.color = Color.green;

    }

    private void OnMouseExit()
    {
        // hoverDialog.gameObject.SetActive(false);
        renderer.material.color = Color.yellow;
    }

    private void OnMouseDown()
    {
        levelDetails.gameObject.SetActive(true);
    }
}
