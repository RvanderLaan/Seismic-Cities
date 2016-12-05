using UnityEngine;
using System.Collections;

public class TargetController : MonoBehaviour {

    public WaveGenerator instance;
    public StrengthController sc;

    private Vector3 clickPosition;

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}

    public bool mouseMoved() {
        return (clickPosition - Input.mousePosition).sqrMagnitude >= 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            clickPosition = Input.mousePosition;
        }

        // Place marker
        if (Input.GetMouseButtonUp(0) && !mouseMoved()) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
            targetPos.z = -0.01f;
            transform.position = targetPos;

        }

        // Create wave
        if (Input.GetKeyDown(KeyCode.Space)) {
            WaveGenerator wg = (WaveGenerator) GameObject.Instantiate(instance, transform.position, Quaternion.identity);
            wg.startSpeed = sc.getTimingScore() * wg.startSpeed * .8f + wg.startSpeed * .2f;
            // wg.transform.SetParent(transform);

            audioSource.pitch = Random.Range(0.5f, 1f);
            audioSource.Play();
         }
    }


}
