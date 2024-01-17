using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    // Olu�turulacak Tile prefab'�
    [SerializeField] private Tile tilePrefab;

    // Tile'lar�n durumlar�n� tutan dizi
    [SerializeField] private TileState[] tileStates;

    // Tile tahtas�n� temsil eden �zgara
    private TileGrid grid;

    // Oyundaki t�m Tile'lar� tutan liste
    private List<Tile> tiles;

    // Tile'lar�n hareketi i�in bekleme durumu
    private bool waiting;

    // Awake metodu, s�n�f olu�turuldu�unda �al���r
    private void Awake()
    {
        // Tile �zgaras�n� al
        grid = GetComponentInChildren<TileGrid>();

        // Tile'lar� tutan listeyi olu�tur
        tiles = new List<Tile>(16);
    }

    // Oyun tahtas�n� temizleyen metot
    public void ClearBoard()
    {
        // Her h�creyi bo�alt
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }

        // Oyundaki t�m Tile'lar� yok et
        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        // Tile listesini temizle
        tiles.Clear();
    }

    // Yeni bir Tile olu�turan metot
    public void CreateTile()
    {
        // TilePrefab'� klonla
        Tile tile = Instantiate(tilePrefab, grid.transform);

        // Tile'�n durumunu ayarla ve rastgele bo� bir h�creye yerle�tir
        tile.SetState(tileStates[0]);
        tile.Spawn(grid.GetRandomEmptyCell());

        // Tile'� listeye ekle
        tiles.Add(tile);
    }

    // Her frame'de �al��an metot
    private void Update()
    {
        // Hareket tu�lar�na tepki verme, beklenen durumda de�ilse
        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(Vector2Int.down, 0, 1, grid.Height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(Vector2Int.right, grid.Width - 2, -1, 0, 1);
            }
        }
    }

    // Tile'lar� belirtilen y�nde hareket ettiren metot
    private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        // Hareket ger�ekle�ti mi kontrol�
        bool changed = false;

        // �ki boyutlu d�ng� ile t�m h�creleri kontrol et
        for (int x = startX; x >= 0 && x < grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.Height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                // E�er h�cre doluysa, Tile'� belirtilen y�nde hareket ettir
                if (cell.Occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        // Hareket ger�ekle�tiyse, de�i�iklikleri beklemek i�in coroutine ba�lat
        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    // Tile'� belirtilen y�nde hareket ettiren metot
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        // Yeni h�cre ve kom�u h�creleri temsil eden de�i�kenler
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        // Kom�u h�cre null olana kadar d�ng�y� s�rd�r
        while (adjacent != null)
        {
            // E�er kom�u h�cre doluysa
            if (adjacent.Occupied)
            {
                // Tile'lar birle�tirilebiliyorsa birle�tir ve true d�n
                if (CanMerge(tile, adjacent.tile))
                {
                    MergeTiles(tile, adjacent.tile);
                    return true;
                }

                break;
            }

            // E�er kom�u h�cre bo�sa, yeni h�cre olarak ayarla
            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        // E�er yeni bir h�cre bulunduysa, Tile'� o h�creye ta�� ve true d�n
        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        // Hareket ger�ekle�mediyse false d�n
        return false;
    }

    // �ki Tile'�n birle�tirilebilir olup olmad���n� kontrol eden metot
    private bool CanMerge(Tile a, Tile b)
    {
        return a.state == b.state && !b.locked;
    }

    // �ki Tile'� birle�tiren metot
    private void MergeTiles(Tile a, Tile b)
    {
        // Liste i�erisinden Tile'� ��kar ve birle�tirme i�lemini ger�ekle�tir
        tiles.Remove(a);
        a.Merge(b.cell);

        // Yeni bir Tile durumu al ve di�er Tile'�n durumunu bir �st seviyeye ��kar
        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        TileState newState = tileStates[index];

        b.SetState(newState);

        // Oyun y�neticisine puan� art�rma iste�i g�nder
        GameManager.Instance.IncreaseScore(newState.number);
    }

    // Bir Tile durumunun indeksini bulan metot
    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i])
            {
                return i;
            }
        }

        return -1;
    }

    // Yap�lan de�i�iklikleri beklemek i�in coroutine ba�latan metot
    private IEnumerator WaitForChanges()
    {
        // Bekleme durumunu true yap
        waiting = true;

        // 0.1 saniye bekle
        yield return new WaitForSeconds(0.1f);

        // Bekleme durumunu false yap
        waiting = false;

        // T�m Tile'lar�n kilidini kald�r
        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        // E�er Tile say�s� �zgara boyutuna e�it de�ilse, yeni bir Tile olu�tur
        if (tiles.Count != grid.Size)
        {
            CreateTile();
        }

        // Oyunun bitip bitmedi�ini kontrol et
        if (CheckForGameOver())
        {
            // Oyun bitti�inde oyun y�neticisine bildir
            GameManager.Instance.GameOver();
        }
    }

    // Oyunun bitip bitmedi�ini kontrol eden metot
    public bool CheckForGameOver()
    {
        // E�er Tile say�s� �zgara boyutuna e�it de�ilse, oyun bitmemi�tir
        if (tiles.Count != grid.Size)
        {
            return false;
        }

        // Her bir Tile i�in, yukar�, a�a��, sola ve sa�a kom�u h�crelere bak
        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            // E�er birle�tirilebilecek bir kom�u bulunursa, oyun bitmemi�tir
            if (up != null && CanMerge(tile, up.tile))
            {
                return false;
            }

            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }

            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }

            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }
        }

        // Yukar�daki ko�ullar sa�lanmazsa, oyun bitmi�tir
        return true;
    }
}
