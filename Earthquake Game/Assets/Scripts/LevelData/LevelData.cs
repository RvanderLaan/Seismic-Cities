using UnityEngine;

using System.Collections.Generic;

/// <summary>
/// Extension of LevelDataScript - Using the ScriptableObject does not work in WebGL.
/// The resource will be null for some reason, but extending the class is a fix.
/// </summary>
[CreateAssetMenu(fileName = "New Level", menuName = "SeismicCities/Level")]
public class LevelData : LevelDataScript
{

}