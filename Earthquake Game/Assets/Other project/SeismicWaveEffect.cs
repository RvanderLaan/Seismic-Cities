using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SeismicWaveEffect : MonoBehaviour {
    public Material material;
    public GameObject target;

    private float pressTime = float.MaxValue;

    void Start() {
        reset();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            reset();
    }

    public void reset() {
        material.SetFloat("_StartTime", Time.time);
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination) {

        Vector3 relTargetPos = Camera.main.WorldToScreenPoint(target.transform.position);
        relTargetPos.y = Screen.height - relTargetPos.y;

        material.SetVector("_Center", relTargetPos);

        material.SetFloat("_Width", 5 * -10 / Camera.main.transform.position.z);
        Graphics.Blit(source, destination, material);
    }
}
