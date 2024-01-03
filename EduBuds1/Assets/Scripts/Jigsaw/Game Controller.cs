using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Private deðiþkeni arayüzde eriþim için 
    [SerializeField]
    private Transform[] _pictures; //görsellerimizi atacaðýmýz bir dizi oluþturduk.

    [SerializeField]
    private GameObject _WIN; //Text

    public static bool WIN; //static iken eriþebileceðim

    private void Start()
    {
        _WIN.SetActive(false);
        WIN = false;
    }

    private void Update()
    {
        bool allPicturesAligned = true;

        // Dizinin boyutunu kontrol etmek için döngü kullanma
        for (int i = 0; i < _pictures.Length; i++)
        {
            if (_pictures[i].rotation.z != 0)
            {
                allPicturesAligned = false;
                break; // Eðer bir tanesi dahi hizalanmamýþsa döngüden çýk
            }
        }

        if (allPicturesAligned)
        {
            _WIN.SetActive(true);
            WIN = true;
        }
    }


}
