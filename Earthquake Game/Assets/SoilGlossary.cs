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
            string soilId = item.type + "";
            soilId = char.ToLowerInvariant(soilId[0]) + soilId.Substring(1);
            GameObject go = Instantiate(prefab, transform);
            go.GetComponentsInChildren<Image>()[1].sprite = item.sprite;
            go.GetComponentInChildren<TextInserter>().reset(soilId);
            go.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                soilInfoWindow.SetActive(false);
                soilInfoWindow.SetActive(true);
                soilInfoWindow.GetComponentInChildren<TextInserterPro>().reset(soilId + "Description");
            });
        }

    }
}
