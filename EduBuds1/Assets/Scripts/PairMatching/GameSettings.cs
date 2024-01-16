using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameSettings : MonoBehaviour
{
    private readonly Dictionary<EPuzzleCategories, string> _puzzleCatDirectory = new Dictionary<EPuzzleCategories, string>();

    // Oyun ayarlarýný temsil eden bir private integer deðiþken tanýmlanýr.
    private int _settings;

    // Ayar sayýsýný belirten sabit bir integer tanýmlanýr.
    private const int SettingsNumber = 2;
    private bool _muteFxPermanently = false;    

    // Eþleme sayýsýný belirten bir enum tanýmlanýr.
    public enum EPairNumber
    {
        NotSet = 0,
        E10Pairs = 10,
        E15Pairs = 15,
        E20Pairs = 20
    };

    // Bulmaca kategorisini belirten bir enum tanýmlanýr.
    public enum EPuzzleCategories
    {
        NotSet,
        Fruits,
        Vegetables,
    }

    // Ayarlarý tutan bir yapý (struct) tanýmlanýr.
    public struct Settings
    {
        public EPairNumber PairsNumber;
        public EPuzzleCategories PuzzleCategory;
    };

    // Oyun ayarlarýný tutan bir Settings yapýsý örneði tanýmlanýr.
    private Settings _gameSettings;

    // GameSettings sýnýfýnýn bir örneði oluþturulur ve bu örnek baþlangýçta null olarak tanýmlanýr.
    public static GameSettings Instance;

    // Awake metodu, nesnenin oluþturulduðu anda çaðrýlýr.
    private void Awake()
    {
        // Eðer Instance null ise, bu nesneyi yok etmeyi önleyip Instance'ý bu nesne olarak ayarlar.
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        // Eðer Instance zaten tanýmlý ise, bu nesneyi yok eder.
        else
        {
            Destroy(this);
        }
    }

    // Start metodu, nesnenin ilk çerçeve oluþturulduðunda çaðrýlýr.
    void Start()
    {
        // GameSettings yapýsý oluþturulur ve oyun ayarlarý sýfýrlanýr.
        _gameSettings = new Settings();
        ResetGameSettings();

        SetPuzzleCategoryDirectory();
    }

    private void SetPuzzleCategoryDirectory()
    {
        _puzzleCatDirectory.Add(EPuzzleCategories.Fruits, "Box1");
        _puzzleCatDirectory.Add(EPuzzleCategories.Vegetables, "Box2");
    }


    // Eþleme sayýsýný belirleyen bir metot.
    public void SetPairNumber(EPairNumber Number)
    {
        // Eðer eþleme sayýsý daha önce belirlenmemiþse, _settings deðiþkenini arttýrýr.
        if (_gameSettings.PairsNumber == EPairNumber.NotSet)
            _settings++;

        // Eþleme sayýsýný belirtilen deðere ayarlar.
        _gameSettings.PairsNumber = Number;
    }

    // Bulmaca kategorisini belirleyen bir metot.
    public void SetPuzzleCategories(EPuzzleCategories cat)
    {
        // Eðer bulmaca kategorisi daha önce belirlenmemiþse, _settings deðiþkenini arttýrýr.
        if (_gameSettings.PuzzleCategory == EPuzzleCategories.NotSet)
            _settings++;

        // Bulmaca kategorisini belirtilen deðere ayarlar.
        _gameSettings.PuzzleCategory = cat;
    }

    // Eþleme sayýsýný döndüren bir metot.
    public EPairNumber GetPairNumber()
    {
        return _gameSettings.PairsNumber;
    }

    // Bulmaca kategorisini döndüren bir metot.
    public EPuzzleCategories GetPuzzleCategory()
    {
        return _gameSettings.PuzzleCategory;
    }

    // Oyun ayarlarýný sýfýrlayan bir metot.
    public void ResetGameSettings()
    {
        _settings = 0;
        _gameSettings.PuzzleCategory = EPuzzleCategories.NotSet;
        _gameSettings.PairsNumber = EPairNumber.NotSet;
    }

    // Tüm ayarlar yapýlmýþ mý kontrol eden bir metot.
    public bool AllSettingsReady()
    {
        return _settings == SettingsNumber;
    }

    public string GetMaterialDirectoryName()
    {
        return "Materials/";
    }

    public string GetPuzzleCategoryTextureDirectoryName()
    {
        if(_puzzleCatDirectory.ContainsKey(_gameSettings.PuzzleCategory))
        {
            return "Graphics/PuzzleCat/" + _puzzleCatDirectory[_gameSettings.PuzzleCategory] + "/";
        }
        else
        {
            Debug.LogError("ERROR : CANNOT GET DIRECTORY NAME!");
            return "";
        }
    }


    public void MuteSoundEffectPermanently(bool muted)
    {
        _muteFxPermanently = muted;
    }

    public bool isSoundEffectMutedPermanently()
    {
        return _muteFxPermanently;
    }
}