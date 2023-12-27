using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sugar_maker : MonoBehaviour
{
    public GameObject[] pre_Cands;
    public int width, height;
    //oyundaki þekerleri bu matrise kaydet, konumuna göre
    public static Cands[,] candies_in_the_game;


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
        GameObject random_candy_object = Create_Random_Candy();

        // rastgele þeker al, kordinat, bir rotasyon vermedik(0,0,0)
        GameObject new_cands = GameObject.Instantiate (random_candy_object, new Vector3 (x, y+10, 0), Quaternion.identity);

        // Her þekerin, þeker scriptini al, yeni konum oluþtur(creat_new_location) yap.

        Cands cand = new_cands.GetComponent<Cands> ();
        cand.colour = random_candy_object.name;
        cand.Enter_New_Location(x, y);
        candies_in_the_game[x, y] = cand;

        // ben y de 10 konumda duruyorum y dðerine gelmesi için yavaþ yavaþ düþmesi gerekecek
        StartCoroutine(MoveCandDown(cand));

    }
    IEnumerator MoveCandDown(Cands cand)
    {
        float duration = 0.3f; // Düþme süresi
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

        CheckAndFillEmptySpaces(); // Boþ alanlarý kontrol et ve doldur
    }

    void CheckAndFillEmptySpaces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 1; y < height; y++) // En üstteki satýrý kontrol etmeye gerek yok
            {
                if (candies_in_the_game[x, y] == null) // Eðer bu hücre boþsa
                {
                    // Üstteki hücredeki þekerin rengini alarak yeni bir þeker oluþtur
                    string colorOfCandAbove = candies_in_the_game[x, y - 1].colour;
                    GameObject randomCandyObject = Create_Random_Candy(colorOfCandAbove);
                    GameObject newCands = GameObject.Instantiate(randomCandyObject, new Vector3(x, y + 10, 0), Quaternion.identity);

                    Cands cand = newCands.GetComponent<Cands>();
                    cand.colour = randomCandyObject.name;
                    cand.Enter_New_Location(x, y);
                    candies_in_the_game[x, y] = cand;

                    StartCoroutine(MoveCandDown(cand)); // Düþme animasyonu baþlat
                }
            }
        }
    }
    public GameObject Create_Random_Candy(string excludedColor = "")   //hangi þekeri oluþturduysam onu belirteceðim
    {
        List<GameObject> availableCandies = new List<GameObject>();
        foreach (var candy in pre_Cands)
        {
            if (!candy.name.Equals(excludedColor)) // Ayný renkte þekerden bir daha oluþturmamak için kontrol
            {
                availableCandies.Add(candy);
            }
        }

        int rand = Random.Range(0, availableCandies.Count); //min 0, en yüksek þeker uzunluðu
        return availableCandies[rand]; // bunlarýn içinden rastgele bir sayý alacak
    }
    void Update()
    {
        
    }
}
