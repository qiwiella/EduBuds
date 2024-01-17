using UnityEngine;

public class TileCell : MonoBehaviour
{
    // Hücrenin koordinatlarý (x, y)
    public Vector2Int coordinates { get; set; }

    // Hücrede bulunan Tile nesnesi
    public Tile tile { get; set; }

    // Hücre boþ mu kontrolü
    public bool Empty => tile == null;

    // Hücre dolu mu kontrolü
    public bool Occupied => tile != null;
}
