using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantsGen : MonoBehaviour
{
    public GameObject[] plants;
    public int plantsAmount = 0;
    private void Start()
    {
        int rand = Random.Range(0, plants.Length);
        for (int i = 0; i < plantsAmount; i++)
        {
            GameObject plant = Instantiate(this.plants[0]);
            plant.transform.parent = GameObject.Find("Ground").transform;
            Vector2 pos = new Vector2(Random.Range(-50, 399), Random.Range(-50, 199));
            plant.transform.position = pos;
        }
    }

}