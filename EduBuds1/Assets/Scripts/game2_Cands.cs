using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game2_Cands : MonoBehaviour
{
    float x, y;
    bool should_it_fall = true; //düþsün mü? evet

    void Start(){
        
    }

    // herbir þekerin hangi konumda olduðunu tutmamýz gerekiyor
    // düþme iþlemini kontrol etmemiz geekiyor
    public void Enter_New_Location(float _x, float _y)
    {
        //start da girdiðimiz deðerle buradaki deðerlere eþit olacak
        x = _x;
        y = _y;
    }


    
    void Update()
    {
        
        if(should_it_fall)
        {
            // iki deðer arasýnda avaþca düþsün
            //hangi hýzda inecek
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), Time.deltaTime * 3f);
        }
    }
}
