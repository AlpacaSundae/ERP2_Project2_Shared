// Solution to mirror a camera's output in Unity sourced from Unity forum post below
// http://answers.unity.com/answers/1300369/view.html
//
// Used in project for binding the hand display model to always use the left
// hand of the model, and just flip the image for a right hand input

using UnityEngine;
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class MirrorFlipCamera : MonoBehaviour {
    new Camera camera;
    public bool flipHorizontal;
    void Awake () {
        camera = GetComponent<Camera>();
    }
    void OnPreCull() {
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        Vector3 scale = new Vector3(flipHorizontal ? -1 : 1, 1, 1);
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
    }
    void OnPreRender () {
        GL.invertCulling = flipHorizontal;
    }

    void OnPostRender () {
        GL.invertCulling = false;
    }
}