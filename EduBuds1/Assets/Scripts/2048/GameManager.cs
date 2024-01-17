using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton örneði
    public static GameManager Instance { get; private set; }

    // Oyun tahtasýný temsil eden TileBoard nesnesi
    [SerializeField] private TileBoard board;

    // Oyun bitiþ ekraný
    [SerializeField] private CanvasGroup gameOver;

    // Skor ve en yüksek skor metin alanlarý
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hiscoreText;

    // Oyuncu skoru
    private int score;

    // Oyuncu skorunu dýþarýdan okunabilir yapar
    public int Score => score;

    // Oyun yöneticisinin oluþturulduðu anki iþlemler
    private void Awake()
    {
        // Singleton örneði kontrolü
        if (Instance != null)
        {
            // Eðer baþka bir GameManager örneði varsa, bu örneði yok et
            DestroyImmediate(gameObject);
        }
        else
        {
            // Baþka bir örnek yoksa, bu örneði atayarak yok edilmez yap
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Oyun baþladýðýnda çalýþan metot
    private void Start()
    {
        // Yeni bir oyun baþlat
        NewGame();
    }

    // Yeni bir oyun baþlatan metot
    public void NewGame()
    {
        // Skoru sýfýrla
        SetScore(0);

        // En yüksek skoru yükle ve göster
        hiscoreText.text = LoadHiscore().ToString();

        // Oyun bitiþ ekranýný gizle
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        // Oyun tahtasýný sýfýrla ve baþlangýç için iki Tile oluþtur
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true; // Oyun tahtasýný etkinleþtir
    }

    // Oyunun bittiðini belirten metot
    public void GameOver()
    {
        // Oyun tahtasýný devre dýþý býrak ve oyun bitiþ ekranýný etkinleþtir
        board.enabled = false;
        gameOver.interactable = true;

        // Oyun bitiþ ekranýný belirli bir sürede gösteren coroutine baþlat
        StartCoroutine(Fade(gameOver, 1f, 1f));
    }

    // CanvasGroup'un belirli bir sürede belirli bir deðere (alpha) fade yapmasýný saðlayan coroutine
    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
    {
        // Belirli bir süre bekle
        yield return new WaitForSeconds(delay);

        // Fade iþlemini gerçekleþtir
        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Son deðeri garantilemek için ayarla
        canvasGroup.alpha = to;
    }

    // Skoru artýran metot
    public void IncreaseScore(int points)
    {
        // Skoru güncelle
        SetScore(score + points);
    }

    // Skoru ayarlayan metot
    private void SetScore(int score)
    {
        // Skoru güncelle ve UI üzerinde göster
        this.score = score;
        scoreText.text = score.ToString();

        // En yüksek skoru kaydet
        SaveHiscore();
    }

    // En yüksek skoru kaydeden metot
    private void SaveHiscore()
    {
        // Kaydedilmiþ en yüksek skoru yükle
        int hiscore = LoadHiscore();

        // Eðer güncellenen skor, mevcut en yüksek skordan büyükse, yeni skoru kaydet
        if (score > hiscore)
        {
            PlayerPrefs.SetInt("hiscore", score);
        }
    }

    // Kaydedilmiþ en yüksek skoru yükle
    private int LoadHiscore()
    {
        return PlayerPrefs.GetInt("hiscore", 0);
    }
}
