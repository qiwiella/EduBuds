using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cands : MonoBehaviour
{
    float x, y;
    bool should_it_fall = true; //düþsün mü? evet
    GameObject choice_tool;

    public static Cands first_choice_cand;
    public static Cands second_choice_cand;


    void Start(){
        //seçim tagýndaki objeyi bu ve al
        choice_tool = GameObject.FindGameObjectWithTag("choice");
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
            //Eðer þekerimin konumum, hedef konumumdan 0.2f den küçükse 
            //düþme learpünü kapatsýn.en son posizyonu hedef konuma eþitlesin.
            if( transform.position.y-y<0.05f)
            {
                should_it_fall = false;
                transform.position = new Vector3(x, y, 0);
            }
            // iki deðer arasýnda yavaþca düþsün
            //hangi hýzda inecek
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), Time.deltaTime * 3f);
        }
    }

    //Fareye týklandýðý zaman seçim aracý buraya gelsin
    void OnMouseDown()
    {
        choice_tool.transform.position = transform.position;
        Cands_Control();
        
    }

    void Cands_Control() 
    {
        if (first_choice_cand == null)
        {
            first_choice_cand = this;
        }
        else
        {
            second_choice_cand = this;
            if (first_choice_cand != second_choice_cand)
            {
                //Mutlak deðer içerisinde x ve y farkýný bul.
                float difference_x = Mathf.Abs(first_choice_cand.x - second_choice_cand.y);
                float difference_y = Mathf.Abs(first_choice_cand.y - second_choice_cand.y);
                if (difference_x + difference_y == 1)
                {
                    Debug.Log("Let Them Change! ");
                    first_choice_cand = null;
                    second_choice_cand = null;
                }
                else
                {
                    first_choice_cand = second_choice_cand;
                    second_choice_cand = null;
                }
            }
            else
            {
                second_choice_cand = null;
            }
        }

    }
}
