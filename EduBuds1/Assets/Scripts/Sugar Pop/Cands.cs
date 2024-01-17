using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cands : MonoBehaviour
{
    public float x, y;
    bool should_it_fall = true; //düþsün mü? evet
    GameObject choice_tool;

    public static Cands first_choice_cand;
    public static Cands second_choice_cand;

    public Vector3 target_location; // þekerimizin yeni gitmek istediði hedef konum
    public bool change_location = false;

    // þekerleri listede x ve y eksenindekiler olarak ayrý ayrý tutuyoruz.
    public List <Cands> sugar_x_axis;
    public List<Cands> sugar_y_axis;

    public string colour;

    Animator animator;
    
    


    void Start(){
        //seçim tagýndaki objeyi bu ve al
        choice_tool = GameObject.FindGameObjectWithTag("choice");
        animator = GetComponent<Animator>();
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
            if( transform.position.y-y < 0.05f)
            {
                should_it_fall = false;
                transform.position = new Vector3(x, y, 0);
            }
            // iki deðer arasýnda yavaþca düþsün
            //hangi hýzda inecek
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), Time.deltaTime * 3f);
        }
        if(change_location)
        {
            Change_Places();
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
                float difference_x = Mathf.Abs(first_choice_cand.x - second_choice_cand.x);
                float difference_y = Mathf.Abs(first_choice_cand.y - second_choice_cand.y);

                if ((difference_x == 1 && difference_y == 0) || (difference_x == 0 && difference_y == 1))
                {
                    if(first_choice_cand.colour != second_choice_cand.colour)
                    {
                        Debug.Log("Let Them Change! ");
                        first_choice_cand.target_location = second_choice_cand.transform.position;
                        second_choice_cand.target_location = first_choice_cand.transform.position;
                        first_choice_cand.change_location = true;
                        second_choice_cand.change_location = true;

                        Change_Variables();
                        first_choice_cand.Check_X_Axis();
                        first_choice_cand.Check_Y_Axis();
                        second_choice_cand.Check_X_Axis();
                        second_choice_cand.Check_Y_Axis();

                        StartCoroutine(first_choice_cand.Disappear());
                        StartCoroutine(second_choice_cand.Disappear());

                        first_choice_cand = null;
                        second_choice_cand = null;
                    }
                }
                else
                {
                    first_choice_cand = second_choice_cand;
                }
            }
                second_choice_cand = null;  
        }

    }
    void Change_Variables()
    {
        Sugar_maker.candies_in_the_game[(int) first_choice_cand.x, (int) first_choice_cand.y] = second_choice_cand;
        Sugar_maker.candies_in_the_game[(int) second_choice_cand.x, (int) second_choice_cand.y] = first_choice_cand;


        //ilk seçilen
        float first_choice_X = first_choice_cand.x; // 4 5  
        float first_choice_Y = first_choice_cand.y; // 5 5

        first_choice_cand.x = second_choice_cand.x; // 5 5
        first_choice_cand.y = second_choice_cand.y; // 5 5

        second_choice_cand.x = first_choice_X;
        second_choice_cand.y = first_choice_Y;

    }
    void Change_Places()
    {
        transform.position = Vector3.Lerp(transform.position, target_location, Time.deltaTime * 5f); // Örnek bir hýz deðeri, ihtiyaca göre ayarlayabilirsiniz.
        if (Vector3.Distance(transform.position, target_location) < 0.01f)
        {
            transform.position = target_location;
            change_location = false;
        }
    }

    public void Check_X_Axis()
    {
        //saðýndaki nesnelerin kontrolü
        for (int i = (int) x + 1; i < Sugar_maker.candies_in_the_game.GetLength(0); i++)
        {
            Cands candy_on_the_right = Sugar_maker.candies_in_the_game[i, (int) y];
            if( colour == candy_on_the_right.colour)
            {
                sugar_x_axis.Add(candy_on_the_right);
            }
            else
            {
                break;
            }
        }
        for (int i = (int) x-1; i >= 0; i--)
        {
            Cands candy_on_the_right = Sugar_maker.candies_in_the_game[i, (int) y];
            if( colour == candy_on_the_right.colour)
            {
                sugar_x_axis.Add(candy_on_the_right);
            }
            else
            {
                break;
            }
        }
    }
    public void Check_Y_Axis()
    {
        //saðýndaki nesnelerin kontrolü
        for (int i = (int) y + 1; i < Sugar_maker.candies_in_the_game.GetLength(1); i++)
        {
            Cands candy_on_the_right = Sugar_maker.candies_in_the_game[(int) x, i];
            if (colour == candy_on_the_right.colour)
            {
                sugar_y_axis.Add(candy_on_the_right);
            }
            else
            {
                break;
            }
        }
        for (int i = (int) y - 1; i >= 0; i--)
        {
            Cands candy_on_the_right = Sugar_maker.candies_in_the_game[(int) x, i];
            if (colour == candy_on_the_right.colour)
            {
                sugar_y_axis.Add(candy_on_the_right);
            }
            else
            {
                break;
            }
        }
    }
    //oynayýcýnýn iç þekeri ayný hizaya geldiðini görmesi için enumerator kullanýyorum.
    //direktr yok etmesini istemiyorum. Disappear = yok olamak, yok etmek
    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(0.3f);
        //þekerlerinn sayýsý 2 den büyük ise
        if(sugar_x_axis.Count >= 2 || sugar_y_axis.Count >= 2)
        {
            Destroy(gameObject);
            if(sugar_x_axis.Count >= 2)
            {
                //buradaki elamanlarý tek tek al
                foreach( var item in sugar_x_axis)
                {
                    //þeker objesinin oyun objesinde yok et þeker companentiyle birlikte
                    Destroy(item.gameObject);
                }
            }
            else
            {
                foreach (var item in sugar_y_axis)
                {
                    Destroy(item.gameObject);
                }
            }
        }
        
    }

    
}



