using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    // Tile'�n durumu
    public TileState state { get; private set; }

    // Tile'�n ba�l� oldu�u h�cre
    public TileCell cell { get; private set; }

    // Tile'�n kilitli olup olmad���
    public bool locked { get; set; }

    // Tile'�n arkaplan� ve metni i�in referanslar
    private Image background;
    private TextMeshProUGUI text;

    // Tile olu�turuldu�unda �al��an metot
    private void Awake()
    {
        // Tile'�n arkaplan�n� ve metnini temsil eden bile�enlere referanslar� ata
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Tile'�n durumunu ayarlayan metot
    public void SetState(TileState state)
    {
        this.state = state;

        // Tile'�n arkaplan rengi, metin rengi ve say�s�n� ayarla
        background.color = state.backgroundColor;
        text.color = state.textColor;
        text.text = state.number.ToString();
    }

    // Tile'� bir h�creye yerle�tiren metot
    public void Spawn(TileCell cell)
    {
        // E�er Tile zaten bir h�creye ba�l�ysa, ba�l� oldu�u h�crenin referans�n� kald�r
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        // Tile'� yeni h�creye ba�la
        this.cell = cell;
        this.cell.tile = this;

        // Tile'� h�crenin konumuna yerle�tir
        transform.position = cell.transform.position;
    }

    // Tile'� ba�ka bir h�creye ta��yan metot
    public void MoveTo(TileCell cell)
    {
        // E�er Tile zaten bir h�creye ba�l�ysa, ba�l� oldu�u h�crenin referans�n� kald�r
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        // Tile'� yeni h�creye ba�la
        this.cell = cell;
        this.cell.tile = this;

        // Tile'� belirtilen h�creye do�ru animasyonlu bir �ekilde ta��
        StartCoroutine(Animate(cell.transform.position, false));
    }

    // Tile'� ba�ka bir Tile ile birle�tiren metot
    public void Merge(TileCell cell)
    {
        // E�er Tile zaten bir h�creye ba�l�ysa, ba�l� oldu�u h�crenin referans�n� kald�r
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        // Bu Tile'�n h�cresini null olarak ayarla ve di�er Tile'� kilitli hale getir
        this.cell = null;
        cell.tile.locked = true;

        // Tile'� belirtilen h�creye do�ru animasyonlu bir �ekilde birle�tir
        StartCoroutine(Animate(cell.transform.position, true));
    }

    // Animasyonlu ta��ma veya birle�tirme i�lemini ger�ekle�tiren coroutine metot
    private IEnumerator Animate(Vector3 to, bool merging)
    {
        // Animasyon s�resi ve ge�en s�re
        float elapsed = 0f;
        float duration = 0.1f;

        // Ba�lang�� ve biti� konumlar�
        Vector3 from = transform.position;

        // Belirtilen s�re boyunca animasyonu ger�ekle�tir
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Animasyon bitti�inde Tile'� do�rudan belirtilen konuma yerle�tir
        transform.position = to;

        // E�er birle�tirme i�lemi yap�ld�ysa, bu Tile'� yok et
        if (merging)
        {
            Destroy(gameObject);
        }
    }
}
