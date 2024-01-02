using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotation : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!GameController.WIN)
            transform.Rotate(0f, 0f, 90f);
    }
}
