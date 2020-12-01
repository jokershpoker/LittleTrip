using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Target;

    Vector3 startDistance, moveVec;

    // Start is called before the first frame update
    void Start()
    {
        startDistance = transform.position - Target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        moveVec = Target.position + startDistance;
        //moveVec.y = startDistance.y;
        transform.position = moveVec;
    }
}
