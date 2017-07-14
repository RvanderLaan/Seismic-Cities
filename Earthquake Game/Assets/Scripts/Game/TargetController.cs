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

    public float lifeTime = 10;

	// Use this for initialization
	void Awake () {
        audioSource = GetComponent<AudioSource>();
        waveGenerators = new Queue<WaveGenerator>();

        // Create a few wave generators: More load time but smoother run time
        for (int i = 0; i < initialWaveGenerators; i++) {
            WaveGenerator wg = (WaveGenerator)GameObject.Instantiate(instance, transform.position, Quaternion.identity);
            wg.lifeTime = lifeTime;
            wg.gameObject.SetActive(false);
            waveGenerators.Enqueue(wg);
        }
	}

    public void createWave(float intensity, GameObject[] targets) {
        WaveGenerator wg;

        // If the last wave generator is done, reuse it
        if (waveGenerators.Peek().isDone()) {
            wg = waveGenerators.Dequeue();
            wg.reset();
        } else {
            wg = (WaveGenerator)GameObject.Instantiate(instance, transform.position, Quaternion.identity);
        }
        wg.intensity = intensity * 5000;
        waveGenerators.Enqueue(wg);
        
        wg.startWave(targets);
        wg.transform.position = transform.position;

        //audioSource.pitch = Random.Range(0.5f, 1f);
        //audioSource.Play();
    }

    public void stopWaves() {
        if (waveGenerators != null)
            foreach (WaveGenerator wg in waveGenerators)
                wg.gameObject.SetActive(false);
    }

}
