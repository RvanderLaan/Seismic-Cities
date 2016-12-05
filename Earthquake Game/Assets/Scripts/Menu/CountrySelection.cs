using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountrySelection : MonoBehaviour {

    public Canvas hoverDialog;
    public Canvas levelDetails;

    private void OnMouseEnter()
    {
        hoverDialog.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        hoverDialog.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        levelDetails.gameObject.SetActive(true);
    }
}
