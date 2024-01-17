using UnityEngine;

public class TileGrid : MonoBehaviour
{
    // Sat�rlar� temsil eden dizi
    public TileRow[] rows { get; private set; }

    // H�creleri temsil eden dizi
    public TileCell[] cells { get; private set; }

    // Grid'in toplam h�cre say�s�
    public int Size => cells.Length;

    // Grid'in y�ksekli�i (sat�r say�s�)
    public int Height => rows.Length;

    // Grid'in geni�li�i (s�tun say�s�)
    public int Width => Size / Height;

    // Awake metodu, s�n�f olu�turuldu�unda �al���r
    private void Awake()
    {
        // Grid i�indeki t�m sat�rlar� al�r
        rows = GetComponentsInChildren<TileRow>();

        // Grid i�indeki t�m h�creleri al�r
        cells = GetComponentsInChildren<TileCell>();

        // H�crelere koordinatlar�n� atama
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].coordinates = new Vector2Int(i % Width, i / Width);
        }
    }

    // Verilen koordinatlardaki h�creyi d�nd�ren metot
    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }

    // Verilen x ve y koordinatlar�ndaki h�creyi d�nd�ren metot
    public TileCell GetCell(int x, int y)
    {
        // Koordinatlar grid s�n�rlar� i�indeyse ilgili h�creyi d�nd�r, de�ilse null d�nd�r
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
    }

    // Verilen h�crenin belirtilen y�nde kom�u h�cresini d�nd�ren metot
    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        // H�crenin koordinatlar�n� al
        Vector2Int coordinates = cell.coordinates;

        // Belirtilen y�nde koordinatlar� g�ncelle
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        // Yeni koordinatlara sahip h�creyi d�nd�r
        return GetCell(coordinates);
    }

    // Rastgele bo� bir h�creyi d�nd�ren metot
    public TileCell GetRandomEmptyCell()
    {
        // Rastgele bir h�cre se�
        int index = Random.Range(0, cells.Length);

        // Ba�lang�� indeksi kaydedilir
        int startingIndex = index;

        // H�cre dolu oldu�u s�rece d�ng�y� devam ettir
        while (cells[index].Occupied)
        {
            index++;

            // Dizi sonuna gelindi�inde ba�a d�n
            if (index >= cells.Length)
            {
                index = 0;
            }

            // E�er t�m h�creler dolu ise null d�nd�r
            if (index == startingIndex)
            {
                return null;
            }
        }

        // Bo� h�creyi d�nd�r
        return cells[index];
    }
}
