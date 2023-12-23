using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sugar_maker : MonoBehaviour
{
    public GameObject[] pre_Cands;
    public int width, height;
    //oyundaki �ekerleri bu matrise kaydet, konumuna g�re
    public static Cands[,] candies_in_the_game;


    // d�ng�ye alarak ka� tane olu�turaca��m�za unity inspector k�sm�ndan vermek istedim.
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
    public void Creat_Cands(int x, int y) // konumu vermesini isteyerek �eker olu�tuaca��z.
    {
        GameObject random_candy_object = Create_Random_Candy();

        // rastgele �eker al, kordinat, bir rotasyon vermedik(0,0,0)
        GameObject new_cands = GameObject.Instantiate (random_candy_object, new Vector3 (x, y+10, 0), Quaternion.identity);

        // Her �ekerin, �eker scriptini al, yeni konum olu�tur(creat_new_location) yap.

        Cands cand = new_cands.GetComponent<Cands> ();
        cand.colour = random_candy_object.name;
        cand.Enter_New_Location(x, y);
        candies_in_the_game[x, y] = cand;

        // ben y de 10 konumda duruyorum y d�erine gelmesi i�in yava� yava� d��mesi gerekecek
        
    }

    public GameObject Create_Random_Candy()   //hangi �ekeri olu�turduysam onu belirtece�im
    {
        int rand = Random.Range(0, pre_Cands.Length); //min 0, en y�ksek �eker uzunlu�u
        return pre_Cands[rand]; // bunlar�n i�inden rastgele bir say� alacak
    }
    
    

    
    void Update()
    {
        
    }
}
