using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cands : MonoBehaviour
{
    float x, y;
    bool should_it_fall = true; //d��s�n m�? evet
    GameObject choice_tool;

    public static Cands first_choice_cand;
    public static Cands second_choice_cand;


    void Start(){
        //se�im tag�ndaki objeyi bu ve al
        choice_tool = GameObject.FindGameObjectWithTag("choice");
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
            //E�er �ekerimin konumum, hedef konumumdan 0.2f den k���kse 
            //d��me learp�n� kapats�n.en son posizyonu hedef konuma e�itlesin.
            if( transform.position.y-y<0.05f)
            {
                should_it_fall = false;
                transform.position = new Vector3(x, y, 0);
            }
            // iki de�er aras�nda yava�ca d��s�n
            //hangi h�zda inecek
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), Time.deltaTime * 3f);
        }
    }

    //Fareye t�kland��� zaman se�im arac� buraya gelsin
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
                //Mutlak de�er i�erisinde x ve y fark�n� bul.
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
