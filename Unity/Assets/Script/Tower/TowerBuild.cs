using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuild : MonoBehaviour
{
    public GameObject toTower;
    public Slider slider;
    public float buildTime;

    Slider bar;
    float second;

    void Start()
    {
        second = 0;
        InvokeRepeating("MoveMoles", 1.0f, 1);
    }

    void MoveMoles()
    {
        second++;
        slider.value = second / buildTime;
        SoundManager.sm.BuildAudio();

        if (slider.value >= 1)
        {
            GameObject obj = Instantiate(toTower, GameObject.Find("Tower").transform);
            obj.transform.position = transform.position;

            CancelInvoke("MoveMoles");
            Destroy(gameObject);
        }
    }
}
