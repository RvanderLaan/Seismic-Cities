using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityDestroyer : MonoBehaviour {

    public float maxRotation = 12;
    public float baseTime = 3;
    public float randomTime = 4;

    void Start () {
        for (int i = 0; i < transform.childCount; i++)
            StartCoroutine("rotateBuilding", i);
	}

    IEnumerator rotateBuilding(int child)
    {
        GameObject go = transform.GetChild (child).gameObject;

        float moveStart = Time.time;
        float moveLength = baseTime + Random.value * randomTime;

        Vector3 rotTarget = new Vector3(0, 0, Random.Range(-maxRotation, maxRotation));

        float t = (Time.time - moveStart) / moveLength;
        while (t < 1)
        {
            go.transform.rotation = Quaternion.Euler(Vector3.Slerp(Vector3.zero, rotTarget, t));
            t = (Time.time - moveStart) / moveLength;
            yield return null;
        }
    }
}
