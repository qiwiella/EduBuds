using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private int _settings;
    private const int SettingsNumber = 2;

    public enum EPairNumber
    {
        NotSet = 0,
        E10Pairs = 10,
        E15Pairs = 15,
        E20Pairs = 20
    };

    public enum EPuzzleCategories
    {
        NotSet,
        Box1,
        Box2
    }

    public struct Settings
    {
        public EPairNumber PairsNumber;
        public EPuzzleCategories PuzzleCategory;

    };

    private Settings _gameSettings;

    public static GameSettings Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        _gameSettings = new Settings();
        ResetGameSettings();
    }

    public void SetPairNumber(EPairNumber Number)
    {
        if (_gameSettings.PairsNumber == EPairNumber.NotSet)
            _settings++;

        _gameSettings.PairsNumber = Number;
    }

    public void SetPuzzleCategories(EPuzzleCategories cat)
    {
        if (_gameSettings.PuzzleCategory == EPuzzleCategories.NotSet)
            _settings++;

        _gameSettings.PuzzleCategory = cat;
    }

    public EPairNumber GetPairNumber()
    {
        return _gameSettings.PairsNumber;
    }

    public EPuzzleCategories GetPuzzleCategory()
    {
        return _gameSettings.PuzzleCategory;
    }

    public void ResetGameSettings()
    {
        _settings = 0;
        _gameSettings.PuzzleCategory = EPuzzleCategories.NotSet;
        _gameSettings.PairsNumber = EPairNumber.NotSet;
    }

    public bool AllSettingsReady()
    {
        return _settings == SettingsNumber;
    }

}
