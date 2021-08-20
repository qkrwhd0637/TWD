using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard2 : MonoBehaviour
{
    void Start()
    {
        Quaternion lookRotaion = Quaternion.LookRotation(-(transform.position - Camera.main.transform.position));
        transform.rotation = lookRotaion;
    }
}
