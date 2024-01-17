using UnityEngine;

// ScriptableObject'ýn yaratýlabilir menüde görünebilmesi için gerekli özellikler
[CreateAssetMenu(menuName = "Tile State")]
public class TileState : ScriptableObject
{
    // Tile'ýn üzerindeki sayý
    public int number;

    // Tile'ýn arkaplan rengi
    public Color backgroundColor;

    // Tile'ýn metin rengi
    public Color textColor;
}
