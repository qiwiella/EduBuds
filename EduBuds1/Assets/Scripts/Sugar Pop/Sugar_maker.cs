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

    

    private void Creat_Cands(int x, int y)
    {
        // Rastgele bir �eker objesi se�
        GameObject random_candy_object = Create_Random_Candy();

        // Yeni �eker objesini olu�tur ve konumunu ayarla
        GameObject new_cands = GameObject.Instantiate(random_candy_object, new Vector3(x, y + 10, 0), Quaternion.identity);

        // Yeni �ekerin Cands bile�enini al ve gerekli ayarlamalar� yap
        Cands cand = new_cands.GetComponent<Cands>();
        cand.colour = random_candy_object.name;
        cand.Enter_New_Location(x, y);
        candies_in_the_game[x, y] = cand;

        // Yava��a a�a�� d��mesi i�in coroutine ba�lat
        StartCoroutine(MoveCandDown(cand));

        // Olu�turulan �ekerin say�s� 2'den b�y�kse yok et
        //StartCoroutine(cand.Disappear());
    }
    IEnumerator MoveCandDown(Cands cand)
    {
        float duration = 0.3f; // D��me s�resi
        float elapsed = 0f;
        Vector3 start = cand.transform.position;
        Vector3 target = new Vector3(cand.transform.position.x, cand.transform.position.y - 1, 0);

        while (elapsed < duration)
        {
            cand.transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cand.transform.position = target;

        CheckAndFillEmptySpaces(); // Bo� alanlar� kontrol et ve doldur
    }

    void CheckAndFillEmptySpaces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 1; y < height; y++) // En �stteki sat�r� kontrol etmeye gerek yok
            {
                if (candies_in_the_game[x, y] == null) // E�er bu h�cre bo�sa
                {
                    // �stteki h�credeki �ekerin rengini alarak yeni bir �eker olu�tur
                    string colorOfCandAbove = candies_in_the_game[x, y - 1].colour;
                    GameObject randomCandyObject = Create_Random_Candy(colorOfCandAbove);
                    GameObject newCands = GameObject.Instantiate(randomCandyObject, new Vector3(x, y + 10, 0), Quaternion.identity);

                    Cands cand = newCands.GetComponent<Cands>();
                    cand.colour = randomCandyObject.name;
                    cand.Enter_New_Location(x, y);
                    candies_in_the_game[x, y] = cand;

                    // Yava��a a�a�� d��mesi i�in coroutine ba�lat
                    StartCoroutine(MoveCandDown(cand));

                    // Olu�turulan �ekerin say�s� 2'den b�y�kse yok et
                    //StartCoroutine(cand.Disappear());
                }
            }
        }
        CheckAndHandleMatches(); // Yeni �ekerler olu�turulduktan sonra e�le�meleri kontrol et

    }

    void CheckAndHandleMatches()
{
    // Bu metotu g�ncelleyerek, �� veya daha fazla ayn� renkte �ekerleri kontrol edebilir ve istedi�iniz i�lemi ger�ekle�tirebilirsiniz.
    // �rne�in, e�le�en �ekerleri yok etmek veya ba�ka bir i�lem ger�ekle�tirmek.

    // �lgili kontrol ve i�lemleri burada ger�ekle�tirin.
}
    public GameObject Create_Random_Candy(string excludedColor = "")   //hangi �ekeri olu�turduysam onu belirtece�im
    {
        List<GameObject> availableCandies = new List<GameObject>();
        foreach (var candy in pre_Cands)
        {
            if (!candy.name.Equals(excludedColor)) // Ayn� renkte �ekerden bir daha olu�turmamak i�in kontrol
            {

                availableCandies.Add(candy);
            }
        }

        int rand = Random.Range(0, availableCandies.Count); //min 0, en y�ksek �eker uzunlu�u
        return availableCandies[rand]; // bunlar�n i�inden rastgele bir say� alacak
    }
    
    

}
