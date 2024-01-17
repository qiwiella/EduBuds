using UnityEngine;

public class TileCell : MonoBehaviour
{
    // H�crenin koordinatlar� (x, y)
    public Vector2Int coordinates { get; set; }

    // H�crede bulunan Tile nesnesi
    public Tile tile { get; set; }

    // H�cre bo� mu kontrol�
    public bool Empty => tile == null;

    // H�cre dolu mu kontrol�
    public bool Occupied => tile != null;
}
