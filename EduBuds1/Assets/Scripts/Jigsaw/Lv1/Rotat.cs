using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotat : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!Control.WIN)
            transform.Rotate(0f, 0f, 90f);
    }
}
