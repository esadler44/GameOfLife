using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MouseMovement : MonoBehaviour {

    private Camera gameCamera;
    private Vector3 mouseDragOrigin;

    private void Awake() {
        gameCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update () {
        gameCamera.orthographicSize += -Input.mouseScrollDelta.y;
        gameCamera.orthographicSize = Mathf.Max(gameCamera.orthographicSize, 1.0f);
        if (Input.GetMouseButtonDown(1)) {
            mouseDragOrigin = Input.mousePosition;
        } else if (Input.GetMouseButton(1)) {
            Vector3 mouseMovement = mouseDragOrigin - Input.mousePosition;
            mouseDragOrigin = Input.mousePosition;
            gameCamera.transform.Translate(mouseMovement * 0.1f);
        }
	}
}
