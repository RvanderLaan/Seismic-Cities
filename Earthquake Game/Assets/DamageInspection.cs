using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Behavior for moving camera to game elements affected by an earthquake
/// and providing feedback or explanations
/// </summary>
public class DamageInspection : MonoBehaviour {

    public Transform[] targets;
    public int targetIndex = 0;
    public Vector2 cameraOffset;
    public float zoom;

    public UnityEvent onFinish; 

    private CamMovement cam;
    private TextInserterPro text;

    

	// Use this for initialization
	void Start () {
        cam = Camera.main.transform.parent.GetComponent<CamMovement>();
        text = GetComponentInChildren<TextInserterPro>();

        cam.detectClicks = false;

        if (targets.Length > 0)
            ShowTarget();
        else
            Debug.LogWarning("Damage Inspection started without any targets");
	}

    public void ShowTarget()
    {
        Transform target = targets[targetIndex];
        cam.moveTo(cameraOffset + (Vector2) target.position);
        cam.zoomTo(zoom, true);

        BuildingZone bz = target.gameObject.GetComponent<BuildingZone>();
        Seismograph seis = target.gameObject.GetComponent<Seismograph>();
        if (bz != null)
        {
            string textId = "correctPlacement";
            Debug.Log(bz.isCorrect() + ", " + bz.soilType + ", " + bz.building.type);
            if (!bz.isCorrect())
                textId = SolutionChecker.getFeedback(bz);
            text.reset(textId);

        } else if (seis != null)
        {
            text.reset("seismographLevel-1");
        }
    }

    public void NextTarget()
    {
        if (targetIndex >= -1 && targetIndex + 1 < targets.Length)
        {
            targetIndex++;
            ShowTarget();
        } else
        {
            cam.detectClicks = true;
            if (onFinish != null)
                onFinish.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        targetIndex = 0;
    }

    public bool isFinished()
    {
        return targetIndex == targets.Length - 1;
    }
}
