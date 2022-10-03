using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public float spawnTime;

    public List<GameObject> boxPrefabs;

    public List<string> boxNames;

    public List<int> boxValues;

    public Dictionary<string, int> valueDict = new Dictionary<string, int>();

    public Transform spawnPoint;

    public int[] weights = new int[6] { 0, 0, 0, 0, 0, 0 };

    int weightSum = 0;

    //int boxNum = 1;

    float startTime;

    public static BoxManager instance;

    public Conveyer targetConveyer;

    public string firstFewBoxes;

    int cur = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            weightSum += weights[i];
        }

        for (int i = 0; i < boxNames.Count; i++)
        {
            valueDict.Add(boxNames[i], boxValues[i]);
        }

        startTime = Time.time;
    }

    private void Update()
    {
        int boxNum = 1; //Mathf.Clamp((int)(Time.time - startTime) / 60, 1, 10);

        if (spawnTime > 0)
        {
            spawnTime -= Time.deltaTime;
        }
        else
        {
            if (!GameManager.gameOver)
            {
                StartCoroutine(SpawnBoxes(boxNum));
                spawnTime = 10;
            }
        }

        if (Input.GetKeyDown("1"))
        {
            SpawnBox(0);
        }
        if (Input.GetKeyDown("2"))
        {
            SpawnBox(1);
        }
        if (Input.GetKeyDown("3"))
        {
            SpawnBox(2);
        }
        if (Input.GetKeyDown("4"))
        {
            SpawnBox(3);
        }
        if (Input.GetKeyDown("5"))
        {
            SpawnBox(4);
        }
        if (Input.GetKeyDown("6"))
        {
            SpawnBox(5);
        }
    }

    IEnumerator SpawnBoxes(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (targetConveyer.boxes.Count == 10)
            {
                GameManager.instance.EndGame();
            }

            int boxIndex;

            if (cur >= firstFewBoxes.Length)
            {
                int rng = Random.Range(0, weightSum);
                for (boxIndex = 0; boxIndex < weights.Count(); boxIndex++)
                {
                    rng -= weights[boxIndex];
                    if (rng <= 0)
                    {
                        break;
                    }
                }
            }
            else
            {
                boxIndex = (int)firstFewBoxes[cur] - 48;
                print(firstFewBoxes[cur]);
                cur++;
                print(boxIndex);
            }

            boxIndex = Mathf.Clamp(boxIndex, 0, weights.Count() - 1);

            GameObject newBox = Instantiate(boxPrefabs[boxIndex]);
            newBox.transform.position = spawnPoint.transform.position;
            newBox.name = boxNames[boxIndex];
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }

    public void SpawnBox(int index)
    {
        GameObject newBox = Instantiate(boxPrefabs[index]);
        newBox.transform.position = spawnPoint.transform.position;
        newBox.name = boxNames[index];
    }


}
