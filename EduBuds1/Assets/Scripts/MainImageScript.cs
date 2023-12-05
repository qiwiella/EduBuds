using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainImageScript : MonoBehaviour
{
    [SerializeField] private GameObject imageUnknown; // imageUnknown bir GameObject'e dönüşüyor!

    public void OnMouseDown() //Mouse tıklandığında çalışacaktır!
    {
        if (imageUnknown.activeSelf) //Eğer imageUnknown aktif ise çalıştır!
        {
            imageUnknown.SetActive(false); // imageUnkown'u pasif hale getir!
        }
    }
    
    private int _spriteId; // Yerel spriteId tanımlaması!
    
    public int spriteId // Genel spriteId tanımlaması!
    {
        get {return _spriteId;} // Genel spriteId, yerel sprideId özelliklerini alır!
    }


    public void ChangeSprite(int id, Sprite image) // Fotoğrafı değiştirme metodu!
    {
        _spriteId = id; // sprideId, metoda girilen id'nin halini alır!
        GetComponent<SpriteRenderer>().sprite = image; // GetComponent, scriptin sprite'ı değiştirmesine izin verir, sprite'ı, metoda girilen image yapar. 
                                                       
    }
}
