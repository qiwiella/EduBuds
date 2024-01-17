using UnityEngine;

public class TileRow : MonoBehaviour
{
    // Sat�rdaki h�creleri temsil eden dizi
    public TileCell[] cells { get; private set; }

    // Awake metodu, s�n�f olu�turuldu�unda �al���r
    private void Awake()
    {
        // Sat�r i�indeki t�m h�creleri al
        cells = GetComponentsInChildren<TileCell>();
    }
}
