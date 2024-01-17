using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    // Tile'ýn durumu
    public TileState state { get; private set; }

    // Tile'ýn baðlý olduðu hücre
    public TileCell cell { get; private set; }

    // Tile'ýn kilitli olup olmadýðý
    public bool locked { get; set; }

    // Tile'ýn arkaplaný ve metni için referanslar
    private Image background;
    private TextMeshProUGUI text;

    // Tile oluþturulduðunda çalýþan metot
    private void Awake()
    {
        // Tile'ýn arkaplanýný ve metnini temsil eden bileþenlere referanslarý ata
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Tile'ýn durumunu ayarlayan metot
    public void SetState(TileState state)
    {
        this.state = state;

        // Tile'ýn arkaplan rengi, metin rengi ve sayýsýný ayarla
        background.color = state.backgroundColor;
        text.color = state.textColor;
        text.text = state.number.ToString();
    }

    // Tile'ý bir hücreye yerleþtiren metot
    public void Spawn(TileCell cell)
    {
        // Eðer Tile zaten bir hücreye baðlýysa, baðlý olduðu hücrenin referansýný kaldýr
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        // Tile'ý yeni hücreye baðla
        this.cell = cell;
        this.cell.tile = this;

        // Tile'ý hücrenin konumuna yerleþtir
        transform.position = cell.transform.position;
    }

    // Tile'ý baþka bir hücreye taþýyan metot
    public void MoveTo(TileCell cell)
    {
        // Eðer Tile zaten bir hücreye baðlýysa, baðlý olduðu hücrenin referansýný kaldýr
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        // Tile'ý yeni hücreye baðla
        this.cell = cell;
        this.cell.tile = this;

        // Tile'ý belirtilen hücreye doðru animasyonlu bir þekilde taþý
        StartCoroutine(Animate(cell.transform.position, false));
    }

    // Tile'ý baþka bir Tile ile birleþtiren metot
    public void Merge(TileCell cell)
    {
        // Eðer Tile zaten bir hücreye baðlýysa, baðlý olduðu hücrenin referansýný kaldýr
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        // Bu Tile'ýn hücresini null olarak ayarla ve diðer Tile'ý kilitli hale getir
        this.cell = null;
        cell.tile.locked = true;

        // Tile'ý belirtilen hücreye doðru animasyonlu bir þekilde birleþtir
        StartCoroutine(Animate(cell.transform.position, true));
    }

    // Animasyonlu taþýma veya birleþtirme iþlemini gerçekleþtiren coroutine metot
    private IEnumerator Animate(Vector3 to, bool merging)
    {
        // Animasyon süresi ve geçen süre
        float elapsed = 0f;
        float duration = 0.1f;

        // Baþlangýç ve bitiþ konumlarý
        Vector3 from = transform.position;

        // Belirtilen süre boyunca animasyonu gerçekleþtir
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Animasyon bittiðinde Tile'ý doðrudan belirtilen konuma yerleþtir
        transform.position = to;

        // Eðer birleþtirme iþlemi yapýldýysa, bu Tile'ý yok et
        if (merging)
        {
            Destroy(gameObject);
        }
    }
}
