using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior for moving camera to game elements affected by an earthquake
/// and providing feedback or explanations
/// </summary>
public class DamageInspection : MonoBehaviour {

    public Transform[] targets;
    public int targetIndex = 0;
    public Vector2 cameraOffset;
    public float zoom;

    private CamMovement cam;
    private UserFeedback userFeedback;

	// Use this for initialization
	void Start () {
        cam = Camera.main.transform.parent.GetComponent<CamMovement>();
	}

    public void ShowTarget()
    {
        Transform target = targets[targetIndex];
        cam.moveTo(cameraOffset + (Vector2) target.position);
        cam.zoomTo(zoom);

        BuildingZone bz = target.gameObject.GetComponent<BuildingZone>();
        Seismograph seis = target.gameObject.GetComponent<Seismograph>();
        if (bz != null)
        {
            string textId = SolutionChecker.getFeedback(bz);
            userFeedback.setText(textId);

        } else if (seis != null)
        {

        }
    }

    public void NextTarget()
    {
        if (targetIndex >= -1 && targetIndex + 1 < targets.Length)
        {
            targetIndex++;
            ShowTarget();
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
