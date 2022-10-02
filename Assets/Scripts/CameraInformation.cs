using UnityEngine;

[System.Serializable]
public class CameraInformation
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public Vector3 eulerAngles;
    [Range(0, 10f)]
    public float time = 5f;
}
