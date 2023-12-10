using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game2Sugarmaker : MonoBehaviour
{
    public GameObject[] Cands;
    public int width, height;

    // d�ng�ye alarak ka� tane olu�turaca��m�za unity inspector k�sm�ndan vermek istedim.
    void Start(){
        for( int x = 0; x < width; x++ )
        {
            for( int y = 0; y < height; y++)
            {
                Creat_Cands(x, y);
            }
        }

    }
    public void Creat_Cands(int x, int y) // konumu vermesini isteyerek �eker olu�tuaca��z.
    {
        // rastgele �eker al, kordinat, bir rotasyon vermedik(0,0)
        GameObject new_cands = GameObject.Instantiate(Create_Random_Candy(), new Vector2(x, y+10), Quaternion.identity);
        // Her �ekerin, �eker scriptini al, yeni konum olu�tur(creat_new_location) yap.
        new_cands.GetComponent<game2_Cands>().Enter_New_Location(x, y);
        // ben y de 10 konumda duruyorum y d�erine gelmesi i�in yava� yava� d��mesi gerekecek
    }

    public GameObject Create_Random_Candy()   //hangi �ekeri olu�turduysam onu belirtece�im
    {
        int rand = Random.Range(0, Cands.Length); //min 0, en y�ksek �eker uzunlu�u
        return Cands[rand]; // bunlar�n i�inden rastgele bir say� alacak
    }
    
    

    
    void Update()
    {
        
    }
}
