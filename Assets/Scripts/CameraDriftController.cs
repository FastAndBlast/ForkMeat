using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDriftController : MonoBehaviour
{
    public List<CameraInformation> information;

    public int currentIndex = 0;
    public float currentTime;

    private void LateUpdate()
    {
        CameraInformation currentInfo = information[currentIndex];

        transform.eulerAngles = currentInfo.eulerAngles;
        transform.position = Vector3.Lerp(currentInfo.startPosition, currentInfo.endPosition, currentTime / currentInfo.time);

        currentTime = Mathf.MoveTowards(currentTime, currentInfo.time, Time.deltaTime);
        
        if (currentTime == currentInfo.time)
        {
            currentIndex++;
            if (currentIndex >= information.Count)
            {
                currentIndex = 0;
            }
            currentTime = 0;
        }

        
        
    }



}
