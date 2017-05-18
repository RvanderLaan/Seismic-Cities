using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilBlock : MonoBehaviour {

   
    public enum BlockShape { Square, TriangleLeftBottom, TriangleRightBottom, TriangleLeftTop, TriangleTopRight };
    
    public Soil.SoilType type = Soil.SoilType.Bedrock;


    private Mesh mesh;
    private Material material;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
