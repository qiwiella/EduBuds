using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class PictureManager : MonoBehaviour
{
    public Picture PicturePrefab;
    public Transform PicSpawnPosition;
    public Vector2 StartPosition = new Vector2(-7f, 3f);
    public Vector2 Offset = new Vector2(2.5f, 2.5f);

    [HideInInspector]
    public List<Picture> PictureList;

    void Start()
    {
        // 5x4 bir resim grid'i oluþturmak üzere metod çaðrýlýyor ve gerekli parametreler veriliyor.
        SpawnPictureMesh(5, 4, StartPosition, Offset, false);

        // Oluþturulan resimleri hedef konumlarýna taþýmak için metod çaðrýlýyor.
        MovePicture(5, 4, StartPosition, Offset);
    }

    void Update()
    {
        // Gerekirse güncelleme (update) lojikleri eklenir.
    }

    private void SpawnPictureMesh(int rows, int columns, Vector2 pos, Vector2 offset, bool scaleDown)
    {
        PictureList = new List<Picture>();

        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var tempPicture = (Picture)Instantiate(PicturePrefab, PicSpawnPosition.position, PicSpawnPosition.transform.rotation);

                // Baþlangýç pozisyonlarý doðrudan burada ayarlanýyor.
                var targetPosition = new Vector3(StartPosition.x, StartPosition.y , 0.0f);

                tempPicture.transform.position = targetPosition;
                tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                PictureList.Add(tempPicture);
            }
        }
    }

    private void MovePicture(int rows, int columns, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for (var col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                // Hedef konum, resimlerin x ve y pozisyonlarýna dayalý olarak belirleniyor.
                var targetPosition = new Vector3(((pos.x + 1) + (( offset.x - 1 ) * row) + 2 ), ((pos.y - 1) - ((offset.y - 1) * col)), 0.0f);

                // Resimleri hedef konumlarýna taþýmak için Coroutine kullanýlarak MoveToPosition metoduna baþlatýlýyor.
                StartCoroutine(MoveToPosition(targetPosition, PictureList[index]));

                // Ýndeks artýrýlýyor.
                index++;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 target, Picture obj)
    {
        // Resmin hedef konuma ulaþmasý için kullanýlacak olan rastgele bir mesafe.
        var randomDis = 7;

        // Resmin hedef konuma ulaþana kadar döngü devam eder.
        while (obj.transform.position != target)
        {
            // Resmin mevcut konumu ile hedef konumu arasýndaki mesafeyi kapatýr.
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);

            // Bir sonraki frame'i bekler.
            yield return null;
        }
    }
}
