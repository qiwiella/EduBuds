using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Private de�i�keni aray�zde eri�im i�in 
    [SerializeField]
    private Transform[] _pictures; //g�rsellerimizi ataca��m�z bir dizi olu�turduk.

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
        if (_pictures[0].rotation.z == 0 &&
            _pictures[1].rotation.z == 0 &&
            _pictures[2].rotation.z == 0 &&
            _pictures[3].rotation.z == 0 &&
            _pictures[4].rotation.z == 0 &&
            _pictures[5].rotation.z == 0 &&
            _pictures[6].rotation.z == 0 &&
            _pictures[7].rotation.z == 0 &&
            _pictures[8].rotation.z == 0 &&
            _pictures[9].rotation.z == 0 &&
            _pictures[10].rotation.z == 0 &&
            _pictures[11].rotation.z == 0 &&
            _pictures[12].rotation.z == 0 &&
            _pictures[13].rotation.z == 0 &&
            _pictures[14].rotation.z == 0 &&
            _pictures[15].rotation.z == 0 )
        {
            _WIN.SetActive(true);
            WIN = true;
        }

        
    }
    
   
}
