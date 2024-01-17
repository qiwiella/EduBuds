using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton �rne�i
    public static GameManager Instance { get; private set; }

    // Oyun tahtas�n� temsil eden TileBoard nesnesi
    [SerializeField] private TileBoard board;

    // Oyun biti� ekran�
    [SerializeField] private CanvasGroup gameOver;

    // Skor ve en y�ksek skor metin alanlar�
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hiscoreText;

    // Oyuncu skoru
    private int score;

    // Oyuncu skorunu d��ar�dan okunabilir yapar
    public int Score => score;

    // Oyun y�neticisinin olu�turuldu�u anki i�lemler
    private void Awake()
    {
        // Singleton �rne�i kontrol�
        if (Instance != null)
        {
            // E�er ba�ka bir GameManager �rne�i varsa, bu �rne�i yok et
            DestroyImmediate(gameObject);
        }
        else
        {
            // Ba�ka bir �rnek yoksa, bu �rne�i atayarak yok edilmez yap
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Oyun ba�lad���nda �al��an metot
    private void Start()
    {
        // Yeni bir oyun ba�lat
        NewGame();
    }

    // Yeni bir oyun ba�latan metot
    public void NewGame()
    {
        // Skoru s�f�rla
        SetScore(0);

        // En y�ksek skoru y�kle ve g�ster
        hiscoreText.text = LoadHiscore().ToString();

        // Oyun biti� ekran�n� gizle
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        // Oyun tahtas�n� s�f�rla ve ba�lang�� i�in iki Tile olu�tur
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true; // Oyun tahtas�n� etkinle�tir
    }

    // Oyunun bitti�ini belirten metot
    public void GameOver()
    {
        // Oyun tahtas�n� devre d��� b�rak ve oyun biti� ekran�n� etkinle�tir
        board.enabled = false;
        gameOver.interactable = true;

        // Oyun biti� ekran�n� belirli bir s�rede g�steren coroutine ba�lat
        StartCoroutine(Fade(gameOver, 1f, 1f));
    }

    // CanvasGroup'un belirli bir s�rede belirli bir de�ere (alpha) fade yapmas�n� sa�layan coroutine
    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
    {
        // Belirli bir s�re bekle
        yield return new WaitForSeconds(delay);

        // Fade i�lemini ger�ekle�tir
        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Son de�eri garantilemek i�in ayarla
        canvasGroup.alpha = to;
    }

    // Skoru art�ran metot
    public void IncreaseScore(int points)
    {
        // Skoru g�ncelle
        SetScore(score + points);
    }

    // Skoru ayarlayan metot
    private void SetScore(int score)
    {
        // Skoru g�ncelle ve UI �zerinde g�ster
        this.score = score;
        scoreText.text = score.ToString();

        // En y�ksek skoru kaydet
        SaveHiscore();
    }

    // En y�ksek skoru kaydeden metot
    private void SaveHiscore()
    {
        // Kaydedilmi� en y�ksek skoru y�kle
        int hiscore = LoadHiscore();

        // E�er g�ncellenen skor, mevcut en y�ksek skordan b�y�kse, yeni skoru kaydet
        if (score > hiscore)
        {
            PlayerPrefs.SetInt("hiscore", score);
        }
    }

    // Kaydedilmi� en y�ksek skoru y�kle
    private int LoadHiscore()
    {
        return PlayerPrefs.GetInt("hiscore", 0);
    }
}
