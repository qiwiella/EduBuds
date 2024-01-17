using UnityEngine;

// ScriptableObject'�n yarat�labilir men�de g�r�nebilmesi i�in gerekli �zellikler
[CreateAssetMenu(menuName = "Tile State")]
public class TileState : ScriptableObject
{
    // Tile'�n �zerindeki say�
    public int number;

    // Tile'�n arkaplan rengi
    public Color backgroundColor;

    // Tile'�n metin rengi
    public Color textColor;
}
