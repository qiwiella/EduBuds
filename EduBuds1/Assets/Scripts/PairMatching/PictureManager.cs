using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class PictureManager : MonoBehaviour
{
    public Picture PicturePrefab;
    public Transform PicSpawnPosition;
    public Vector2 StartPosition = new Vector2(-7f, 3f);
    public Vector2 Offset = new Vector2(2.5f, 2.5f);
    //public Vector2 _offsetFor15Pairs = new Vector2(1.08f, 1.22f);
    //public Vector2 _offsetFor20Pairs = new Vector2(1.08f, 1.0f);
    public Vector3 _mewScaleDown = new Vector3(0.9f, 0.9f, 0.001f);


    [HideInInspector]
    public List<Picture> PictureList;

    private List<Material> _materialList = new List<Material>();
    private List<string> _texturePathList = new List<string>();
    private Material _firstMaterial;
    private string _firstTexturePath;


    void Start()
    {
        LoadMaterials();

        if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E10Pairs)
        {

            // 5x4 bir resim grid'i oluþturmak üzere metod çaðrýlýyor ve gerekli parametreler veriliyor.
            SpawnPictureMesh(5, 2, StartPosition, Offset, false);

            // Oluþturulan resimleri hedef konumlarýna taþýmak için metod çaðrýlýyor.
            MovePicture(5, 2, StartPosition, Offset);

        }
        else if  (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E15Pairs)
        {

            // 5x4 bir resim grid'i oluþturmak üzere metod çaðrýlýyor ve gerekli parametreler veriliyor.
            SpawnPictureMesh(5, 3, StartPosition, Offset, false);

            // Oluþturulan resimleri hedef konumlarýna taþýmak için metod çaðrýlýyor.
            MovePicture(5, 3, StartPosition, Offset);

        }
        else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E20Pairs)
        {

            // 5x4 bir resim grid'i oluþturmak üzere metod çaðrýlýyor ve gerekli parametreler veriliyor.
            SpawnPictureMesh(5, 4, StartPosition, Offset, true);

            // Oluþturulan resimleri hedef konumlarýna taþýmak için metod çaðrýlýyor.
            MovePicture(5, 4, StartPosition, Offset);

        }
    }

    private void LoadMaterials()
    {
        var materialFilePath = GameSettings.Instance.GetMaterialDirectoryName();
        var textureFilePath = GameSettings.Instance.GetPuzzleCategoryTextureDirectoryName();
        var pairNumber = (int)GameSettings.Instance.GetPairNumber();
        const string matBaseName = "Pic";
        var firstMaterialName = "Back";

        for(var index = 1; index <= pairNumber; index++)
        {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            _materialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index; 
            _texturePathList.Add(currentTextureFilePath);
        }

        _firstTexturePath = textureFilePath + firstMaterialName;
        _firstMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;
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
                var tempPicture = (Picture)Instantiate(PicturePrefab, PicSpawnPosition.position, PicturePrefab.transform.rotation);

                if(scaleDown)
                {
                    tempPicture.transform.localScale = _mewScaleDown;
                }

                // Baþlangýç pozisyonlarý doðrudan burada ayarlanýyor.
                var targetPosition = new Vector3(StartPosition.x, StartPosition.y , 0.0f);

                tempPicture.transform.position = targetPosition;
                tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                PictureList.Add(tempPicture);
            }
        }

        ApplyTextures();
    }

    public void ApplyTextures()
    {
        var rndMatIndex = Random.Range(0, _materialList.Count);
        var AppliedTimes = new int[_materialList.Count];    

        for(int i = 0; i < _materialList.Count; i++)
        {
            AppliedTimes[i] = 0;
        }

        foreach(var o in PictureList)
        {
            var randPrevious = rndMatIndex;
            var counter = 0;
            var forceMat = false;

            while (AppliedTimes[rndMatIndex] >= 2 || ((randPrevious == rndMatIndex) && !forceMat))
            {
                rndMatIndex = Random.Range(0, _materialList.Count);
                counter++;
                if(counter > 100)
                {
                    for (var j = 0; j < _materialList.Count; j++) 
                    {
                        if (AppliedTimes[j] < 2)
                        {
                            rndMatIndex = j;
                            forceMat = true;
                        }
                    }

                    if (forceMat == false)
                        return;
                }
            }

            o.SetFirstMaterial(_firstMaterial, _firstTexturePath);
            o.ApplyFirstMaterial();
            o.SetSecondMaterial(_materialList[rndMatIndex], _texturePathList[rndMatIndex]);
            AppliedTimes[rndMatIndex] += 1;
            forceMat = false;

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
