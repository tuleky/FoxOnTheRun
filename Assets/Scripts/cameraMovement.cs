using UnityEngine;

public class cameraMovement : MonoBehaviour {

    public Transform target;
    public float smoothTime = 0f;
    private Vector3 velocity = Vector3.zero;

	
	void LateUpdate () {

        Vector3 targetPosition = target.TransformPoint(new Vector3(4, 1, -10));

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
	}
}
