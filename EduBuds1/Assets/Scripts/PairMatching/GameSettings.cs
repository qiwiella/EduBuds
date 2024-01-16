using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameSettings : MonoBehaviour
{
    private readonly Dictionary<EPuzzleCategories, string> _puzzleCatDirectory = new Dictionary<EPuzzleCategories, string>();

    // Oyun ayarlar�n� temsil eden bir private integer de�i�ken tan�mlan�r.
    private int _settings;

    // Ayar say�s�n� belirten sabit bir integer tan�mlan�r.
    private const int SettingsNumber = 2;
    private bool _muteFxPermanently = false;    

    // E�leme say�s�n� belirten bir enum tan�mlan�r.
    public enum EPairNumber
    {
        NotSet = 0,
        E10Pairs = 10,
        E15Pairs = 15,
        E20Pairs = 20
    };

    // Bulmaca kategorisini belirten bir enum tan�mlan�r.
    public enum EPuzzleCategories
    {
        NotSet,
        Fruits,
        Vegetables,
    }

    // Ayarlar� tutan bir yap� (struct) tan�mlan�r.
    public struct Settings
    {
        public EPairNumber PairsNumber;
        public EPuzzleCategories PuzzleCategory;
    };

    // Oyun ayarlar�n� tutan bir Settings yap�s� �rne�i tan�mlan�r.
    private Settings _gameSettings;

    // GameSettings s�n�f�n�n bir �rne�i olu�turulur ve bu �rnek ba�lang��ta null olarak tan�mlan�r.
    public static GameSettings Instance;

    // Awake metodu, nesnenin olu�turuldu�u anda �a�r�l�r.
    private void Awake()
    {
        // E�er Instance null ise, bu nesneyi yok etmeyi �nleyip Instance'� bu nesne olarak ayarlar.
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        // E�er Instance zaten tan�ml� ise, bu nesneyi yok eder.
        else
        {
            Destroy(this);
        }
    }

    // Start metodu, nesnenin ilk �er�eve olu�turuldu�unda �a�r�l�r.
    void Start()
    {
        // GameSettings yap�s� olu�turulur ve oyun ayarlar� s�f�rlan�r.
        _gameSettings = new Settings();
        ResetGameSettings();

        SetPuzzleCategoryDirectory();
    }

    private void SetPuzzleCategoryDirectory()
    {
        _puzzleCatDirectory.Add(EPuzzleCategories.Fruits, "Box1");
        _puzzleCatDirectory.Add(EPuzzleCategories.Vegetables, "Box2");
    }


    // E�leme say�s�n� belirleyen bir metot.
    public void SetPairNumber(EPairNumber Number)
    {
        // E�er e�leme say�s� daha �nce belirlenmemi�se, _settings de�i�kenini artt�r�r.
        if (_gameSettings.PairsNumber == EPairNumber.NotSet)
            _settings++;

        // E�leme say�s�n� belirtilen de�ere ayarlar.
        _gameSettings.PairsNumber = Number;
    }

    // Bulmaca kategorisini belirleyen bir metot.
    public void SetPuzzleCategories(EPuzzleCategories cat)
    {
        // E�er bulmaca kategorisi daha �nce belirlenmemi�se, _settings de�i�kenini artt�r�r.
        if (_gameSettings.PuzzleCategory == EPuzzleCategories.NotSet)
            _settings++;

        // Bulmaca kategorisini belirtilen de�ere ayarlar.
        _gameSettings.PuzzleCategory = cat;
    }

    // E�leme say�s�n� d�nd�ren bir metot.
    public EPairNumber GetPairNumber()
    {
        return _gameSettings.PairsNumber;
    }

    // Bulmaca kategorisini d�nd�ren bir metot.
    public EPuzzleCategories GetPuzzleCategory()
    {
        return _gameSettings.PuzzleCategory;
    }

    // Oyun ayarlar�n� s�f�rlayan bir metot.
    public void ResetGameSettings()
    {
        _settings = 0;
        _gameSettings.PuzzleCategory = EPuzzleCategories.NotSet;
        _gameSettings.PairsNumber = EPairNumber.NotSet;
    }

    // T�m ayarlar yap�lm�� m� kontrol eden bir metot.
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