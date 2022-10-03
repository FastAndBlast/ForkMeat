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

    [HideInInspector]
    public int weightSum = 0;

    private int[] savedWeights = new int[6] { 0, 0, 0, 0, 0, 0 };

    //float startTime;

    public static BoxManager instance;

    public Conveyer targetConveyer;

    public string firstFewBoxes;

    int cur = 0;

    [HideInInspector]
    public Queue<int> boxPrioQueue = new Queue<int>();

    // 1 - straight away
    // 2 - 3rd box
    // 3 - 6th box
    // 4 - 10th box
    // 5 - 15th box
    // 6 - 22nd box

    int currentBoxWave;

    [HideInInspector]
    public int availableBoxTypes = 1;

    public int boxNum = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        availableBoxTypes = 1;
        currentBoxWave = 0;

        weights.CopyTo(savedWeights, 0);

        for (int i = 1; i < weights.Length; i++)
        {
            weights[i] = 0;
        }

        weightSum = weights[0];

        for (int i = 0; i < boxNames.Count; i++)
        {
            valueDict.Add(boxNames[i], boxValues[i]);
        }
    }

    private void Update()
    {
        //int boxNum = 1; //Mathf.Clamp((int)(Time.time - startTime) / 60, 1, 10);

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
        currentBoxWave += 1;
        if (currentBoxWave == 3)
        {
            weights[1] = savedWeights[1];
            weightSum += weights[1];
            availableBoxTypes++;
        }
        else if (currentBoxWave == 6)
        {
            weights[2] = savedWeights[2];
            weightSum += weights[2];
            availableBoxTypes++;
        }
        else if (currentBoxWave == 10)
        {
            weights[3] = savedWeights[3];
            weightSum += weights[3];
            availableBoxTypes++;
        }
        else if (currentBoxWave == 15)
        {
            weights[4] = savedWeights[4];
            weightSum += weights[4];
            availableBoxTypes++;
        }
        else if (currentBoxWave == 22)
        {
            weights[5] = savedWeights[5];
            weightSum += weights[5];
            availableBoxTypes++;
        }

        for (int i = 0; i < num; i++)
        {
            /*
            if (targetConveyer.boxes.Count == 10)
            {
                GameManager.instance.EndGame();
            }
            */

            int boxIndex;

            if (boxPrioQueue.Count > 0)
            {
                boxIndex = boxPrioQueue.Dequeue();
            }
            else if (cur >= firstFewBoxes.Length)
            {
                int rng = Random.Range(0, weightSum);
                for (boxIndex = 0; boxIndex < weights.Length; boxIndex++)
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
                cur++;
            }

            boxIndex = Mathf.Clamp(boxIndex, 0, weights.Length - 1);

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
