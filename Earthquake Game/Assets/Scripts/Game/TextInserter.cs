using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
public class TextInserter : MonoBehaviour {

    private string id;
    private Text text;
    private TextProvider textProvider;

	// Use this for initialization
	void Awake () {
        text = GetComponent<Text>();
        id = text.text;

        textProvider = GameObject.FindGameObjectWithTag("_GM").GetComponent<TextProvider>();
        updateText();
	}

    public void updateText() {
        text.text = textProvider.getText(id);
    }
}
