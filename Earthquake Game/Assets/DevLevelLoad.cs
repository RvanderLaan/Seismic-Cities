using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class DevLevelLoad : MonoBehaviour
{
    LevelManager lm;

    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    // Use this for initialization
    void Start()
    {
        lm = GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                int numberPressed = i + 1;
                if (numberPressed <= lm.levels.Count)
                {
                    lm.levelIndex = i;
                    lm.Restart();
                }
            }
        }
    }
}
