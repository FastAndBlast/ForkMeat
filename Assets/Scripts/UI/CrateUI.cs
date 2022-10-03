using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrateUI : MonoBehaviour
{
    public List<string> crateNames;
    public List<Sprite> crateSprites;

    public static Dictionary<string, Sprite> dict = new Dictionary<string, Sprite>();

    public Conveyer targetConveyer;

    /*
    public static CrateUI instance;

    private void Awake()
    {
        instance = this;
    }
    */

    private void Start()
    {
        dict = new Dictionary<string, Sprite>();
        for (int i = 0; i < crateNames.Count; i++)
        {
            dict.Add(crateNames[i], crateSprites[i]);
        }
    }

    public void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            Transform imageTransform = transform.GetChild(i);
            if (i < targetConveyer.boxes.Count)
            {
                GameObject box = targetConveyer.boxes[i];

                imageTransform.GetComponent<Image>().sprite = dict[box.name];
                float pos = targetConveyer.GetPositionOfBox(i);
                imageTransform.transform.position = transform.Find("End").position * pos +
                                                    transform.Find("Start").position * (1 - pos);
                imageTransform.gameObject.SetActive(true);
            }
            else
            {
                imageTransform.gameObject.SetActive(false);
            }
        }
    }
}
