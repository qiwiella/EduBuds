using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sugarmaker : MonoBehaviour
{
    public GameObject[] pre_Cands;
    public int width, height;
    //oyundaki þekerleri bu matrise kaydet, konumuna göre
    public Cands[,] candies_in_the_game;


    // döngüye alarak kaç tane oluþturacaðýmýza unity inspector kýsmýndan vermek istedim.
    void Start(){
        candies_in_the_game = new Cands[width, height];
        for ( int x = 0; x < width; x++)
        {
            for( int y = 0; y < height; y++)
            {
                Creat_Cands(x, y);
            }
        }

    }
    public void Creat_Cands(int x, int y) // konumu vermesini isteyerek þeker oluþtuacaðýz.
    {
        // rastgele þeker al, kordinat, bir rotasyon vermedik(0,0)
        
        GameObject new_cands = GameObject.Instantiate(Create_Random_Candy(), new Vector2(x, y+10), Quaternion.identity);

        // Her þekerin, þeker scriptini al, yeni konum oluþtur(creat_new_location) yap.

        Cands cand = new_cands.GetComponent<Cands> ();
        cand.Enter_New_Location(x, y);
        candies_in_the_game[x, y] = cand;

        // ben y de 10 konumda duruyorum y dðerine gelmesi için yavaþ yavaþ düþmesi gerekecek
    }

    public GameObject Create_Random_Candy()   //hangi þekeri oluþturduysam onu belirteceðim
    {
        int rand = Random.Range(0, pre_Cands.Length); //min 0, en yüksek þeker uzunluðu
        return pre_Cands[rand]; // bunlarýn içinden rastgele bir sayý alacak
    }
    
    

    
    void Update()
    {
        
    }
}
