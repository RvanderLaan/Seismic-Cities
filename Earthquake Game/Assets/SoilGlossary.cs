using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoilGlossary : MonoBehaviour {

    public GameObject prefab;
    public GameObject soilInfoWindow;

	public void init(List<SoilGlossaryItem> items)
    {
        // Clear list elements
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        // Add list elements
        foreach (SoilGlossaryItem item in items)
        {
            GameObject go = Instantiate(prefab, transform);
            go.GetComponentsInChildren<Image>()[1].sprite = item.sprite;
            go.GetComponentInChildren<Text>().text = item.type + "";
            go.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                soilInfoWindow.SetActive(false);
                soilInfoWindow.SetActive(true);
                string descriptionId = item.type + "Description";
                descriptionId = char.ToLowerInvariant(descriptionId[0]) + descriptionId.Substring(1);
                soilInfoWindow.GetComponentInChildren<TextInserterPro>().reset(descriptionId);
                
            });
        }

    }
}
