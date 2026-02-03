using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 _startRotation;
    void Awake()
    {
        _startRotation = transform.rotation.eulerAngles;
    }

    void Update()
    {
       transform.forward = Camera.main.transform.forward;

        Vector3 newRotation = transform.rotation.eulerAngles;
        newRotation.y = _startRotation.y;
        newRotation.z = _startRotation.z;
        transform.rotation = Quaternion.Euler(newRotation);
    }
}
