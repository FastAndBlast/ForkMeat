using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public float speed = 0.1f;

    public float turnSpeed = 0.1f;

    public Transform target;

    private Vector3 offset;

    private float originalY;

    private void Start()
    {
        //target = PlayerController.instance;
        //offset = target.InverseTransformVector(transform.position) - target.localPosition;
        offset = transform.position - target.position;
        originalY = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            return;
        }

        Vector3 targetPosition = target.position + offset;
        //Vector3 targetPosition = target.TransformVector(target.localPosition + offset);
        //targetPosition.y = originalY;

        float distance = Vector3.Distance(transform.position, targetPosition) / 2;

        Vector3 newPosition = Vector3.Slerp(transform.position, targetPosition, speed * (1 + distance / 10));

        //transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, turnSpeed);

        transform.forward = target.parent.position - transform.position;

        transform.position = newPosition;
    }
}
