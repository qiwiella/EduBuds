using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    // Oluþturulacak Tile prefab'ý
    [SerializeField] private Tile tilePrefab;

    // Tile'larýn durumlarýný tutan dizi
    [SerializeField] private TileState[] tileStates;

    // Tile tahtasýný temsil eden ýzgara
    private TileGrid grid;

    // Oyundaki tüm Tile'larý tutan liste
    private List<Tile> tiles;

    // Tile'larýn hareketi için bekleme durumu
    private bool waiting;

    // Awake metodu, sýnýf oluþturulduðunda çalýþýr
    private void Awake()
    {
        // Tile ýzgarasýný al
        grid = GetComponentInChildren<TileGrid>();

        // Tile'larý tutan listeyi oluþtur
        tiles = new List<Tile>(16);
    }

    // Oyun tahtasýný temizleyen metot
    public void ClearBoard()
    {
        // Her hücreyi boþalt
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }

        // Oyundaki tüm Tile'larý yok et
        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        // Tile listesini temizle
        tiles.Clear();
    }

    // Yeni bir Tile oluþturan metot
    public void CreateTile()
    {
        // TilePrefab'ý klonla
        Tile tile = Instantiate(tilePrefab, grid.transform);

        // Tile'ýn durumunu ayarla ve rastgele boþ bir hücreye yerleþtir
        tile.SetState(tileStates[0]);
        tile.Spawn(grid.GetRandomEmptyCell());

        // Tile'ý listeye ekle
        tiles.Add(tile);
    }

    // Her frame'de çalýþan metot
    private void Update()
    {
        // Hareket tuþlarýna tepki verme, beklenen durumda deðilse
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

    // Tile'larý belirtilen yönde hareket ettiren metot
    private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        // Hareket gerçekleþti mi kontrolü
        bool changed = false;

        // Ýki boyutlu döngü ile tüm hücreleri kontrol et
        for (int x = startX; x >= 0 && x < grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.Height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                // Eðer hücre doluysa, Tile'ý belirtilen yönde hareket ettir
                if (cell.Occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        // Hareket gerçekleþtiyse, deðiþiklikleri beklemek için coroutine baþlat
        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    // Tile'ý belirtilen yönde hareket ettiren metot
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        // Yeni hücre ve komþu hücreleri temsil eden deðiþkenler
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        // Komþu hücre null olana kadar döngüyü sürdür
        while (adjacent != null)
        {
            // Eðer komþu hücre doluysa
            if (adjacent.Occupied)
            {
                // Tile'lar birleþtirilebiliyorsa birleþtir ve true dön
                if (CanMerge(tile, adjacent.tile))
                {
                    MergeTiles(tile, adjacent.tile);
                    return true;
                }

                break;
            }

            // Eðer komþu hücre boþsa, yeni hücre olarak ayarla
            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        // Eðer yeni bir hücre bulunduysa, Tile'ý o hücreye taþý ve true dön
        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        // Hareket gerçekleþmediyse false dön
        return false;
    }

    // Ýki Tile'ýn birleþtirilebilir olup olmadýðýný kontrol eden metot
    private bool CanMerge(Tile a, Tile b)
    {
        return a.state == b.state && !b.locked;
    }

    // Ýki Tile'ý birleþtiren metot
    private void MergeTiles(Tile a, Tile b)
    {
        // Liste içerisinden Tile'ý çýkar ve birleþtirme iþlemini gerçekleþtir
        tiles.Remove(a);
        a.Merge(b.cell);

        // Yeni bir Tile durumu al ve diðer Tile'ýn durumunu bir üst seviyeye çýkar
        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        TileState newState = tileStates[index];

        b.SetState(newState);

        // Oyun yöneticisine puaný artýrma isteði gönder
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

    // Yapýlan deðiþiklikleri beklemek için coroutine baþlatan metot
    private IEnumerator WaitForChanges()
    {
        // Bekleme durumunu true yap
        waiting = true;

        // 0.1 saniye bekle
        yield return new WaitForSeconds(0.1f);

        // Bekleme durumunu false yap
        waiting = false;

        // Tüm Tile'larýn kilidini kaldýr
        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        // Eðer Tile sayýsý ýzgara boyutuna eþit deðilse, yeni bir Tile oluþtur
        if (tiles.Count != grid.Size)
        {
            CreateTile();
        }

        // Oyunun bitip bitmediðini kontrol et
        if (CheckForGameOver())
        {
            // Oyun bittiðinde oyun yöneticisine bildir
            GameManager.Instance.GameOver();
        }
    }

    // Oyunun bitip bitmediðini kontrol eden metot
    public bool CheckForGameOver()
    {
        // Eðer Tile sayýsý ýzgara boyutuna eþit deðilse, oyun bitmemiþtir
        if (tiles.Count != grid.Size)
        {
            return false;
        }

        // Her bir Tile için, yukarý, aþaðý, sola ve saða komþu hücrelere bak
        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            // Eðer birleþtirilebilecek bir komþu bulunursa, oyun bitmemiþtir
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

        // Yukarýdaki koþullar saðlanmazsa, oyun bitmiþtir
        return true;
    }
}
