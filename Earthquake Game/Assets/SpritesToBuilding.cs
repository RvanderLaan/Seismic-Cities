using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Converts a texture with multiple sprites to a rigidbody in the same order as the sprites were defined
/// </summary>
[ExecuteInEditMode]
public class SpritesToBuilding : MonoBehaviour {

    public Texture2D texture;
    public GameObject blockPrefab;

    public bool update = false;

    // Use this for initialization
    
    void doIt () {
        Sprite[] sprites = Resources.LoadAll<Sprite>(texture.name);

        float xOffset = -texture.width / sprites[0].pixelsPerUnit / 2 + sprites[0].rect.width / sprites[0].pixelsPerUnit / 2;

        for (int i = 0; i < sprites.Length; i++) {
            GameObject s = GameObject.Instantiate(blockPrefab, transform);
            s.GetComponent<SpriteRenderer>().sprite = sprites[i];
            s.transform.localPosition = new Vector2(sprites[i].rect.x / sprites[i].pixelsPerUnit + xOffset, sprites[i].rect.y / sprites[i].pixelsPerUnit);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (update) {
            GameObject[] children = new GameObject[transform.childCount];
            int idx = 0;
            foreach (Transform child in transform)
                children[idx++] = child.gameObject;
            for (int i = 0; i < children.Length; i++)
                DestroyImmediate(children[i]);

            update = false;
            doIt();
        }
	}
}
