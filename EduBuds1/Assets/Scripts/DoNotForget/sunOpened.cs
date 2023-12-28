using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunOpened : MonoBehaviour
{
    [SerializeField] private GameObject pinkcard; 
    
    public void OnMouseDown() 
    {
        if(pinkcard.activeSelf)
        {
            pinkcard.SetActive(false);
        }
    }
}
