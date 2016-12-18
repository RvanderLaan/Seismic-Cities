using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetController : MonoBehaviour {

    public WaveGenerator instance;
    public int initialWaveGenerators = 3;

    public float waitTime = 0.5f;
    private float lastWave = float.MinValue;

    public StrengthController sc;

    private Vector3 clickPosition;

    private AudioSource audioSource;

    private Queue<WaveGenerator> waveGenerators;

    public ModeManager modeManager;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        waveGenerators = new Queue<WaveGenerator>();

        // Create a few wave generators: More load time but smoother run time
        for (int i = 0; i < initialWaveGenerators; i++) {
            WaveGenerator wg = (WaveGenerator)GameObject.Instantiate(instance, transform.position, Quaternion.identity);
            wg.gameObject.SetActive(false);
            waveGenerators.Enqueue(wg);
        }
            
	}

    public bool mouseMoved() {
        return (clickPosition - Input.mousePosition).sqrMagnitude >= 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (modeManager.isInBuildingMode())
            return;

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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > (lastWave + waitTime)) {
            lastWave = Time.time;
            createWave(sc.getTimingScore());
        }

        
    }

    public void createWave(float intensity) {
        WaveGenerator wg;

        // If the last wave generator is done, reuse it
        if (waveGenerators.Peek().isDone()) {
            wg = waveGenerators.Dequeue();
            wg.reset();
        } else {
            wg = (WaveGenerator)GameObject.Instantiate(instance, transform.position, Quaternion.identity);
        }
        wg.intensity = intensity;
        waveGenerators.Enqueue(wg);
        wg.startWave();
        wg.transform.position = transform.position;

        audioSource.pitch = Random.Range(0.5f, 1f);
        audioSource.Play();
    }

}
