using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{    
    public float mouseSensitivity = 3.5f;
    public Transform target;
    public float dstFromTarget = 4f;
    public Vector2 pitchMinMax = new Vector2(-5, 60);    

    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;    

    float x;
    float y;

    void LateUpdate()
    {
        x += Input.GetAxis("Mouse X") * mouseSensitivity;
        y -= Input.GetAxis("Mouse Y") * mouseSensitivity;        

        CorrectCameraClipping();

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(y, x), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * dstFromTarget;
        
    }
    private void CorrectCameraClipping()
    {
        Vector3 clipCheckOffset = new Vector3(0, -.5f, 0);

        RaycastHit hit;

        if (Physics.Linecast(target.position, (transform.position + clipCheckOffset), out hit))
        {
            if (hit.collider.CompareTag("Region"))
                return;
            dstFromTarget = Mathf.Clamp(hit.distance + .3f, 1f, 3f);
            pitchMinMax = new Vector2(-60f, 60f);
            y = Mathf.Clamp(y, pitchMinMax.x + 2f, pitchMinMax.y);
        }
        else
        {
            dstFromTarget = Mathf.Clamp(.05f, 3f, 1f);
            pitchMinMax = new Vector2(-60f, 60f);
            y = Mathf.Clamp(y, pitchMinMax.x + 1f, pitchMinMax.y);
        }
        transform.localPosition = Vector3.Lerp(transform.position, target.position, Time.deltaTime * .1f);
    }
}