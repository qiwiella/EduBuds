using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game2_Cands : MonoBehaviour
{
    float x, y;
    bool should_it_fall = true; //d��s�n m�? evet

    void Start(){
        
    }

    // herbir �ekerin hangi konumda oldu�unu tutmam�z gerekiyor
    // d��me i�lemini kontrol etmemiz geekiyor
    public void Enter_New_Location(float _x, float _y)
    {
        //start da girdi�imiz de�erle buradaki de�erlere e�it olacak
        x = _x;
        y = _y;
    }


    
    void Update()
    {
        
        if(should_it_fall)
        {
            // iki de�er aras�nda ava�ca d��s�n
            //hangi h�zda inecek
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), Time.deltaTime * 3f);
        }
    }
}
