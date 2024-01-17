using UnityEngine;

public class TileGrid : MonoBehaviour
{
    // Satýrlarý temsil eden dizi
    public TileRow[] rows { get; private set; }

    // Hücreleri temsil eden dizi
    public TileCell[] cells { get; private set; }

    // Grid'in toplam hücre sayýsý
    public int Size => cells.Length;

    // Grid'in yüksekliði (satýr sayýsý)
    public int Height => rows.Length;

    // Grid'in geniþliði (sütun sayýsý)
    public int Width => Size / Height;

    // Awake metodu, sýnýf oluþturulduðunda çalýþýr
    private void Awake()
    {
        // Grid içindeki tüm satýrlarý alýr
        rows = GetComponentsInChildren<TileRow>();

        // Grid içindeki tüm hücreleri alýr
        cells = GetComponentsInChildren<TileCell>();

        // Hücrelere koordinatlarýný atama
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].coordinates = new Vector2Int(i % Width, i / Width);
        }
    }

    // Verilen koordinatlardaki hücreyi döndüren metot
    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }

    // Verilen x ve y koordinatlarýndaki hücreyi döndüren metot
    public TileCell GetCell(int x, int y)
    {
        // Koordinatlar grid sýnýrlarý içindeyse ilgili hücreyi döndür, deðilse null döndür
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
    }

    // Verilen hücrenin belirtilen yönde komþu hücresini döndüren metot
    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        // Hücrenin koordinatlarýný al
        Vector2Int coordinates = cell.coordinates;

        // Belirtilen yönde koordinatlarý güncelle
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        // Yeni koordinatlara sahip hücreyi döndür
        return GetCell(coordinates);
    }

    // Rastgele boþ bir hücreyi döndüren metot
    public TileCell GetRandomEmptyCell()
    {
        // Rastgele bir hücre seç
        int index = Random.Range(0, cells.Length);

        // Baþlangýç indeksi kaydedilir
        int startingIndex = index;

        // Hücre dolu olduðu sürece döngüyü devam ettir
        while (cells[index].Occupied)
        {
            index++;

            // Dizi sonuna gelindiðinde baþa dön
            if (index >= cells.Length)
            {
                index = 0;
            }

            // Eðer tüm hücreler dolu ise null döndür
            if (index == startingIndex)
            {
                return null;
            }
        }

        // Boþ hücreyi döndür
        return cells[index];
    }
}
