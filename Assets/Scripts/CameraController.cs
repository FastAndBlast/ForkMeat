using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public float speed = 0.1f;

    public float turnSpeed = 0.1f;

    public Transform target;

    public float targetForwardShift;

    public Vector2 xBound = new Vector2(-10000, 10000);
    public Vector2 zBound = new Vector2(-10000, 10000);

    //private Vector3 offset;

    private float originalY;

    private void Start()
    {
        //target = PlayerController.instance;
        //offset = target.InverseTransformVector(transform.position) - target.localPosition;
        //offset = transform.position - target.position;
        originalY = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            return;
        }

        Vector3 horizontalForward = new Vector3(target.forward.x, 0, target.forward.z);

        Vector3 targetPosition = target.position; // + offset;
        //Vector3 targetPosition = target.TransformVector(target.localPosition + offset);
        //targetPosition.y = originalY;

        float distance = Vector3.Distance(transform.position, targetPosition) / 2;

        Vector3 newPosition = Vector3.Slerp(transform.position, targetPosition, speed * (1 + distance / 10));

        newPosition = new Vector3(Mathf.Clamp(newPosition.x, xBound.x, xBound.y), newPosition.y, Mathf.Clamp(newPosition.z, zBound.x, zBound.y));

        //transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, turnSpeed);

        transform.forward = (target.parent.position + horizontalForward * targetForwardShift) - transform.position;

        transform.position = newPosition;
    }
}
