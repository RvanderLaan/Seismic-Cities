using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour {

    public GameObject[] soilBlocks;
    public Material[] materials;

    public GameObject soilBlock;
    public Material soilMaterial;

    private GameObject previewBlock;

    private Plane centerPlane;

	// Use this for initialization
	void Start () {
        centerPlane = new Plane(Vector3.back, Vector3.right);

        previewBlock = GameObject.Instantiate(soilBlocks[0]);
	}

    public void setSoilBlock(int i) {
        soilBlock = soilBlocks[i];

        GameObject.Destroy(previewBlock);
        previewBlock = GameObject.Instantiate(soilBlocks[i]);
        previewBlock.GetComponent<MeshRenderer>().material = soilMaterial;
    }

    public void setSoilType(int i) {
        soilMaterial = materials[i];

        previewBlock.GetComponent<MeshRenderer>().material = materials[i];
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetMouseButtonDown(1)) {

            GameObject newBlock = GameObject.Instantiate(soilBlock);
            newBlock.GetComponent<MeshRenderer>().material = soilMaterial;
            newBlock.transform.position = getCursorWorldPos();
            
        } else {
            // Preview
            if (previewBlock != null) {
                previewBlock.transform.position = getCursorWorldPos();
            }
        }
	}

    private Vector3 getCursorWorldPos() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (centerPlane.Raycast(ray, out rayDistance)) {
            Vector3 point = ray.GetPoint(rayDistance);
            point += new Vector3(8, 8, 0);
            point.x = Mathf.Round(point.x / 8) * 8;
            point.y = Mathf.Round(point.y / 8) * 8;
            point.z = Mathf.Round(point.z / 8) * 8;

            return point;
        }

        return Vector3.zero;
    }
}
