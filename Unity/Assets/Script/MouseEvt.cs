using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseEvt : MonoBehaviour
{
    [Header("EventColor")]
    public Color hoverColor;
    public Color startColor;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    //마우스와 일치할 시
    void OnMouseEnter()
    {
        //Debug.Log("OnMouseEnter");
        rend.material.color = hoverColor;
    }

    //마우스와 일치하지 않을 시
    void OnMouseExit()
    {
        //Debug.Log("OnMouseExit");
        rend.material.color = startColor;
    }
}
