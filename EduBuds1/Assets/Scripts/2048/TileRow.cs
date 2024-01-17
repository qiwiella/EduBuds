using UnityEngine;

public class TileRow : MonoBehaviour
{
    // Satýrdaki hücreleri temsil eden dizi
    public TileCell[] cells { get; private set; }

    // Awake metodu, sýnýf oluþturulduðunda çalýþýr
    private void Awake()
    {
        // Satýr içindeki tüm hücreleri al
        cells = GetComponentsInChildren<TileCell>();
    }
}
