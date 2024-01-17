using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public class Config 
{
#if UNITY_EDITOR 
    private static readonly string Dir = Directory.GetCurrentDirectory();
#elif UNITY_ANDROID
    private static readonly string Dir = Application.persistentDataPath;
#else
    private static readonly string Dir = Directory.GetCurrentDirectory();
#endif 

    static readonly string File = @"\PairMatcing.ini";
    static readonly string Path = Dir + File;


    private const int NumberOfScoreRecords = 3;

    public static float[] ScoreTimeList10Pairs = new float[NumberOfScoreRecords];
    public static string[] PairNumberList10Pairs = new string[NumberOfScoreRecords];

    public static float[] ScoreTimeList15Pairs = new float[NumberOfScoreRecords];
    public static string[] PairNumberList15Pairs = new string[NumberOfScoreRecords];

    public static float[] ScoreTimeList20Pairs = new float[NumberOfScoreRecords];
    public static string[] PairNumberList20Pairs = new string[NumberOfScoreRecords];

    private static bool _bestScore = false;
    
    public static void CreateScoreFile()
    {
        if(System.IO.File.Exists(Path) == false)
        {
            CreateFile();
        }
        UpdateScoreList();
    }
    
    public static void UpdateScoreList()
    {
        var file = new StreamReader(Path);
        UpdateScoreList(file, ScoreTimeList10Pairs, PairNumberList10Pairs);
        UpdateScoreList(file, ScoreTimeList15Pairs, PairNumberList15Pairs);
        UpdateScoreList(file, ScoreTimeList20Pairs, PairNumberList20Pairs);

        file.Close();
    }

    private static void UpdateScoreList(StreamReader file, float[] scoreTimeList, string[] pairNumberList)
    {
        if (file == null) return;

        var line = file.ReadLine();

        while(line != null && line[0] == '(')
        {
            line = file.ReadLine(); 
        }

        for(int i = 1; i <= NumberOfScoreRecords; i++)
        {
            var word = line.Split('#');

            if (word[0] == i.ToString())
            {
                string[] substring = Regex.Split(word[1], "D");
                if (float.TryParse(substring[0], out var scoreOnPosition))
                {
                    scoreTimeList[i - 1] = scoreOnPosition;

                    if (scoreTimeList[i-1] > 0)
                    {
                        var dataTime = Regex.Split(substring[1], "T");
                        pairNumberList[i - 1] = dataTime[0] + "T" + dataTime[1];
                    }
                    else
                    {
                        pairNumberList[i - 1] = " ";
                    }
                }
                else
                {
                    scoreTimeList[i - 1] = 0;
                    pairNumberList[i - 1] = " ";
                }

            line = file.ReadLine(); 

            }
        }
    }

    //(10 Pairs)
    //1#51.33241D05/31/2024T07:12
    //2#OD
    // saat olmazsa alttaki gibi de�er d�nd�rmez d den sonra olursa �stteki gibi alg�lar
    
    public static void PlaceScoreOnBoard(float time)
    {
        UpdateScoreList();
        _bestScore = false;

        switch (GameSettings.Instance.GetPairNumber())
        {
            case GameSettings.EPairNumber.E10Pairs:
                PlaceScoreOnBoard(time, ScoreTimeList10Pairs, PairNumberList10Pairs);
                break;
            case GameSettings.EPairNumber.E15Pairs:
                PlaceScoreOnBoard(time, ScoreTimeList15Pairs, PairNumberList15Pairs);
                break;
            case GameSettings.EPairNumber.E20Pairs:
                PlaceScoreOnBoard(time, ScoreTimeList20Pairs, PairNumberList20Pairs);
                break;
        }

        SaveScoreList();
    }

    private static void PlaceScoreOnBoard(float time, float[] scoreTimeList, string[] pairNumberList)
    {
        var theTime = System.DateTime.Now.ToString("hh:mm");
        var theData = System.DateTime.Now.ToString("MM/dd/yyyy");
        var currentDate = theData + "T" + theTime;

        for(int i = 0; i < NumberOfScoreRecords; i++)
        {
            if (scoreTimeList[i] > time || scoreTimeList[i] == 0.0f)
            {
                if (i == 0)
                    _bestScore = true;

                for(var moveDownFrom = (NumberOfScoreRecords - 1); moveDownFrom > i; moveDownFrom--)
                {
                    scoreTimeList[moveDownFrom] = scoreTimeList[moveDownFrom - 1];
                    pairNumberList[moveDownFrom] = pairNumberList[moveDownFrom - 1];
                }

                scoreTimeList[i] = time;
                pairNumberList[i] = currentDate;
                break;
            }
        }

    }

    public static bool IsBestScore()
    {
        return _bestScore;
    }

    public static void CreateFile()
    {
        SaveScoreList();
    }

    public static void SaveScoreList()
    {
        System.IO.File.WriteAllText(Path, string.Empty);

        var writer = new StreamWriter(Path, false);

        writer.WriteLine("(10PAIRS)");
        for(var i = 1; i <= NumberOfScoreRecords; i++)
        {
            var x = ScoreTimeList10Pairs[i-1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + PairNumberList10Pairs[i - 1]);
        }

        writer.WriteLine("(15PAIRS)");
        for (var i = 1; i <= NumberOfScoreRecords; i++)
        {
            var x = ScoreTimeList15Pairs[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + PairNumberList15Pairs[i - 1]);
        }

        writer.WriteLine("(20PAIRS)");
        for (var i = 1; i <= NumberOfScoreRecords; i++)
        {
            var x = ScoreTimeList20Pairs[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + PairNumberList20Pairs[i - 1]);
        }

        writer.Close();
    }

}
