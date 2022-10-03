using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class MapUpgradeManager : MonoBehaviour
{
    public List<Transform> arrowParentParents;

    public List<GameObject> conveyerScriptObjects;

    public static MapUpgradeManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (Transform arrowParent in arrowParentParents)
        {
            for (int i = 0; i < arrowParent.childCount; i++)
            {
                Transform child = arrowParent.GetChild(i);
                for (int j = 0; j < child.childCount; j++)
                {
                    child.GetChild(j).gameObject.SetActive(false);
                }
            }
            /*
            foreach (Transform child in arrowParent)
            {
                foreach (Transform arrow in arrowParent)
                {
                    print(arrow.name);
                    arrow.gameObject.SetActive(false);
                }
            }
            */
        }

        foreach (GameObject conveyerObject in conveyerScriptObjects)
        {
            conveyerObject.SetActive(false);
        }
    }

    public void Upgrade()
    {
        foreach (Transform arrowParent in arrowParentParents)
        {
            for (int i = 0; i < arrowParent.childCount; i++)
            {
                Transform child = arrowParent.GetChild(i);
                for (int j = 0; j < child.childCount; j++)
                {
                    child.GetChild(j).gameObject.SetActive(true);
                }
            }
        }

        foreach (GameObject conveyerObject in conveyerScriptObjects)
        {
            conveyerObject.SetActive(true);
        }
    }



}
