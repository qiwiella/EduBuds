using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    //Private değişkeni arayüzde erişim için 
    [SerializeField]
    private Transform[] _pictures; //görsellerimizi atacağımız bir dizi oluşturduk.

    [SerializeField]
    private GameObject _WIN; //Text

    public static bool WIN; //sttic iken erişebileceğim

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
            _pictures[5].rotation.z == 0 )
        {
            _WIN.SetActive(true);
            WIN = true;
        }
    }
}
