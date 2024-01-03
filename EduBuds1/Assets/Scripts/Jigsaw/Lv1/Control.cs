using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    //Private de�i�keni aray�zde eri�im i�in 
    [SerializeField]
    private Transform[] _picture; //g�rsellerimizi ataca��m�z bir dizi olu�turduk.

    [SerializeField]
    private GameObject _WIN; //Text

    public static bool WIN; //static iken eri�ebilece�im

    private void Start()
    {
        _WIN.SetActive(false);
        WIN = false;
    }

    private void Update()
    {
        if (_picture[0].rotation.z == 0 &&
            _picture[1].rotation.z == 0 &&
            _picture[2].rotation.z == 0 &&
            _picture[3].rotation.z == 0 &&
            _picture[4].rotation.z == 0 &&
            _picture[5].rotation.z == 0 &&
            _picture[6].rotation.z == 0 &&
            _picture[7].rotation.z == 0 &&
            _picture[8].rotation.z == 0 
            )
        {
            _WIN.SetActive(true);
            WIN = true;
        }


    }


}
