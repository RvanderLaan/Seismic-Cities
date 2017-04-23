using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class TextProvider : MonoBehaviour {

    public enum Language { English = 0, Nederlands = 1 };

    private Dictionary<string, string>[] dictionaries;

    void Awake() {
        // Should be extended to all languages you need
        dictionaries = new Dictionary<string, string>[] {
            new Dictionary<string, string>(),
            new Dictionary<string, string>()
        };


        TextAsset translations = (TextAsset) Resources.Load("Translations");
        StringReader reader = new StringReader(translations.text);
        XmlReader xml = XmlReader.Create(reader);

        // Skip headers
        xml.ReadToDescendant("Row");
        // xml.ReadToNextSibling("Row");

        // Better: Dynamically add language types defined in xml instead of only reading specific languages

        // Read rows
        string currentId = "";
        while (xml.Read()) {

            // Debug.Log(xml.Name + ": " + xml.Value + " - " + xml.NodeType.ToString());

            if (xml.NodeType == XmlNodeType.Element) {
                // For some reason, the Name node is empty, and the next node without name contains the value
                string name = xml.Name;
                xml.Read();

                if (name == "ID") {
                    currentId = xml.Value;
                } else if (name == "English") {
                    insertTranslation(currentId, xml.Value, (int)Language.English);
                } else if (name == "Nederlands") {
                    insertTranslation(currentId, xml.Value, (int)Language.Nederlands);
                }
            }
        }
    }

    private void insertTranslation(string id, string val, int language) {
        if (!dictionaries[language].ContainsKey(id)) {
            dictionaries[language].Add(id, val);
        } else {
            Debug.LogWarning("Duplicate language key: '" + id + "' in language " + language + " of value '" + val + "'");
        }
    }

    public string getText(string id) {
        int language = PlayerPrefs.GetInt("Language");

        if (language > dictionaries.Length) {
            Debug.LogWarning("No language found for language id " + language);
            return id;
        }

        if (!dictionaries[language].ContainsKey(id)) {
            Debug.LogWarning("No translation found for id " + id + " in language " + language);
            return id;
        }
        return dictionaries[language][id];
    }

}
