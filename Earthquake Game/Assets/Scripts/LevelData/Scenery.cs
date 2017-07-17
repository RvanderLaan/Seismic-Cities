using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scenery", menuName = "SeismicCities/Scenery")]
public class Scenery : ScriptableObject {
    [SerializeField]
    public GameObject[] prefabs;
    public float spawnChance = 0.5f;
    public float randomScale = 0.2f;
}
