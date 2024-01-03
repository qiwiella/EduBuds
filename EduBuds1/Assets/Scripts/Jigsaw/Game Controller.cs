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
        bool allPicturesAligned = true;

        // Dizinin boyutunu kontrol etmek i�in d�ng� kullanma
        for (int i = 0; i < _pictures.Length; i++)
        {
            if (_pictures[i].rotation.z != 0)
            {
                allPicturesAligned = false;
                break; // E�er bir tanesi dahi hizalanmam��sa d�ng�den ��k
            }
        }

        if (allPicturesAligned)
        {
            _WIN.SetActive(true);
            WIN = true;
        }
    }


}
