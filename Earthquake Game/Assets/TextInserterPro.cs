using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextInserterPro : MonoBehaviour
{

    private string id;
    private TextMeshProUGUI text;
    private TextProvider textProvider;

    // Use this for initialization
    void Awake()
    {
        textProvider = GameObject.FindGameObjectWithTag("_GM").GetComponent<TextProvider>();
        text = GetComponent<TextMeshProUGUI>();
        reset(text.text);
    }

    public void reset(string newText)
    {
        id = newText;
        updateText();
    }

    public void updateText()
    {
        if (text == null || textProvider == null)
        {
            text = GetComponent<TextMeshProUGUI>();
            textProvider = GameObject.FindGameObjectWithTag("_GM").GetComponent<TextProvider>();
        }

        text.text = textProvider.getText(id);
    }
}
