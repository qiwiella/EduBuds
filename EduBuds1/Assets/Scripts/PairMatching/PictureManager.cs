using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour
{
    public Picture PicturePrefab;
    public Transform PicSpawnPosition;
    public Vector2 StartPosition = new Vector2(-7f, 3f);
    public Vector2 StartPositionFift = new Vector2(-7.7f, 2.5f);
    public Vector2 StartPositionTwent = new Vector2(-6.7f, 3f);

    [Space]
    [Header("End Game Screen")]
    public GameObject EndGamePanel;

    public GameObject catIconImage;

    public GameObject NewBestScoreText;
    public GameObject YourScoreText;
    public GameObject EndTimeText;

    public enum GameState 
    {   NoAction, 
        MovingOnPositions, 
        DeletingPuzzles, 
        FlipBack, 
        Checking, 
        GameEnd 
    };

    public enum PuzzleState
    {
        PuzzleRotating,
        CanRotate
    };

    public enum RevealedState
    {
        NoRevealed, 
        OneRevealed,
        TwoRevealed
    };

    [HideInInspector]
    public GameState CurrentGameState;
    [HideInInspector]
    public PuzzleState CurrentPuzzleState;
    [HideInInspector]
    public RevealedState PuzzleRevealedNumber;


    public Vector2 Offset = new Vector2(2.5f, 2.5f);
    public Vector2 OffsetFift = new Vector2(2.3f, 2.5f);
    public Vector2 OffsetTwent = new Vector2(2.3f, 2.5f);

    public Vector3 _mewScaleDown = new Vector3(0.9f, 0.9f, 0.001f);


    [HideInInspector]
    public List<Picture> PictureList;

    private List<Material> _materialList = new List<Material>();
    private List<string> _texturePathList = new List<string>();
    private Material _firstMaterial;
    private string _firstTexturePath;

    private int _firstRevealedPic;
    private int _secondRevealedPic;
    private int _revealedPicNumber = 0;
    private int _picToDestroy1;
    private int _picToDestroy2;

    private bool _coroutineStarted = false;

    private int _pairNumbers;
    private int _removedPairs;
    private Timer _gameTimer;



    void Start()
    {

        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        _revealedPicNumber = 0;
        _firstRevealedPic = -1;
        _secondRevealedPic = -1;

        _removedPairs = 0;
        _pairNumbers = (int)GameSettings.Instance.GetPairNumber();

        _gameTimer = GameObject.Find("Main Camera").GetComponent<Timer>();

        catIconImage = GameObject.Find("catIconImage");

        LoadMaterials(); 

        if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E10Pairs)
        {
            CurrentGameState = GameState.MovingOnPositions;

            // 5x4 bir resim grid'i olu�turmak �zere metod �a�r�l�yor ve gerekli parametreler veriliyor.
            SpawnPictureMesh(5, 4, StartPosition, Offset, false);

            // Olu�turulan resimleri hedef konumlar�na ta��mak i�in metod �a�r�l�yor.
            MovePicture(5, 4, StartPosition, Offset);

        }
        else if  (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E15Pairs)
        {
            CurrentGameState = GameState.MovingOnPositions;

            // 5x4 bir resim grid'i olu�turmak �zere metod �a�r�l�yor ve gerekli parametreler veriliyor.
            SpawnPictureMesh(10, 3, StartPositionFift, Offset, false);

            // Olu�turulan resimleri hedef konumlar�na ta��mak i�in metod �a�r�l�yor.
            MovePictureFift(10, 3, StartPositionFift, OffsetFift);

        }
        else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E20Pairs)
        {
            CurrentGameState = GameState.MovingOnPositions;

            // 5x4 bir resim grid'i olu�turmak �zere metod �a�r�l�yor ve gerekli parametreler veriliyor.
            SpawnPictureMesh(8, 5, StartPositionTwent, Offset, true);

            // Olu�turulan resimleri hedef konumlar�na ta��mak i�in metod �a�r�l�yor.
            MovePictureTwent (8, 5, StartPositionTwent, OffsetTwent);

        }
    }

    public void CheckPicture()
    {
        CurrentGameState = GameState.Checking;
        _revealedPicNumber = 0;

        for (int id = 0; id < PictureList.Count; id++)
        {
            if (PictureList[id].Revealed && _revealedPicNumber < 2)
            {
                if (_revealedPicNumber == 0)
                {
                    _firstRevealedPic = id;
                    _revealedPicNumber++;
                }
                else if (_revealedPicNumber == 1)
                {
                    _secondRevealedPic = id;
                    _revealedPicNumber++;
                }
            }
        }
    
        if ( _revealedPicNumber == 2)
        {
            if (PictureList[_firstRevealedPic].GetIndex() == PictureList[_secondRevealedPic].GetIndex() && _firstRevealedPic != _secondRevealedPic)
            {
                CurrentGameState = GameState.DeletingPuzzles;
                _picToDestroy1 = _firstRevealedPic;
                _picToDestroy2 = _secondRevealedPic;
            }
            else
            {
                CurrentGameState = GameState.FlipBack;
            }
        }

        CurrentPuzzleState = PictureManager.PuzzleState.CanRotate;

        if(CurrentGameState == GameState.Checking)
        {
            CurrentGameState = GameState.NoAction;
        }
    }

    private void DestroyPicture()
    {
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        PictureList[_picToDestroy1].Deactivate();
        PictureList[_picToDestroy2].Deactivate();
        _revealedPicNumber = 0;
        _removedPairs++;
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
    }


    private IEnumerator FlipBack()
    {
        _coroutineStarted = true;
        yield return new WaitForSeconds(0.5f);

        PictureList[_firstRevealedPic].FlipBack();
        PictureList[_secondRevealedPic].FlipBack();

        PictureList[_firstRevealedPic].Revealed = false;
        PictureList[_secondRevealedPic].Revealed = false;

        PuzzleRevealedNumber = RevealedState.NoRevealed;
        CurrentGameState = GameState.NoAction;

        _coroutineStarted = false;
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
        if(CurrentGameState == GameState.DeletingPuzzles)
        {
            if(CurrentPuzzleState == PuzzleState.CanRotate )
            {
                DestroyPicture();
                CheckGameEnd();
            }
        }

        if(CurrentGameState == GameState.FlipBack)
        {
            if(CurrentPuzzleState == PuzzleState.CanRotate && _coroutineStarted == false)
            {
               StartCoroutine( FlipBack());
            }
        }

        if(CurrentGameState == GameState.GameEnd)
        {
            if (PictureList[_firstRevealedPic].gameObject.activeSelf == false && PictureList[_secondRevealedPic].gameObject.activeSelf == false && EndGamePanel.activeSelf == false )
            {
                ShowEndGameInformation();
            }
        }
    }



    private bool CheckGameEnd()
    {
        if(_removedPairs == _pairNumbers && CurrentGameState != GameState.GameEnd)
        {
            CurrentGameState = GameState.GameEnd;
            _gameTimer.StopTimer();
            Config.PlaceScoreOnBoard(_gameTimer.GetCurrentTime());  
        }

        return (CurrentGameState == GameState.GameEnd);
    }

    private void ShowEndGameInformation()
    {
        EndGamePanel.SetActive(true);
        catIconImage.SetActive(false);


        if(Config.IsBestScore())
        {
            NewBestScoreText.SetActive(true);
            YourScoreText.SetActive(false);
        }
        else
        {
            NewBestScoreText.SetActive(false);
            YourScoreText.SetActive(true);
        }

        var timer = _gameTimer.GetCurrentTime();
        var minutes = Mathf.Floor(timer / 60);
        var seconds = Mathf.RoundToInt(timer % 60);
        var newText = minutes.ToString("00") + ":" + seconds.ToString("00");
        EndTimeText.GetComponent<Text>().text = newText; 

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

                if (columns == 4)
                {

                    // Ba�lang�� pozisyonlar� do�rudan burada ayarlan�yor.
                    var targetPosition = new Vector3(StartPosition.x, StartPosition.y, 0.0f);

                    tempPicture.transform.position = targetPosition;
                    tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                    PictureList.Add(tempPicture);

                }
                else if (columns == 3)
                {

                    // Ba�lang�� pozisyonlar� do�rudan burada ayarlan�yor.
                    var targetPosition = new Vector3(StartPositionFift.x, StartPositionFift.y, 0.0f);

                    tempPicture.transform.position = targetPosition;
                    tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                    PictureList.Add(tempPicture);

                }
                else if (columns == 5)
                {

                    // Ba�lang�� pozisyonlar� do�rudan burada ayarlan�yor.
                    var targetPosition = new Vector3(StartPositionTwent.x, StartPositionTwent.y, 0.0f);

                    tempPicture.transform.position = targetPosition;
                    tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                    PictureList.Add(tempPicture);

                }
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
            o.SetIndex(rndMatIndex);
            o.Revealed = false;
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
                // Hedef konum, resimlerin x ve y pozisyonlar�na dayal� olarak belirleniyor.
                var targetPosition = new Vector3(((pos.x + 1) + (( offset.x - 1 ) * row) + 2 ), ((pos.y - 1) - ((offset.y - 1) * col)), 0.0f);

                // Resimleri hedef konumlar�na ta��mak i�in Coroutine kullan�larak MoveToPosition metoduna ba�lat�l�yor.
                StartCoroutine(MoveToPosition(targetPosition, PictureList[index]));

                // �ndeks art�r�l�yor.
                index++;
            }
        }
    }

    private void MovePictureFift(int rows, int columns, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for (var col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                // Hedef konum, resimlerin x ve y pozisyonlar�na dayal� olarak belirleniyor.
                var targetPosition = new Vector3(((pos.x - 1) + ((offset.x - 1) * row) + 2), ((pos.y) - ((offset.y - 1) * col)), 0.0f);

                // Resimleri hedef konumlar�na ta��mak i�in Coroutine kullan�larak MoveToPosition metoduna ba�lat�l�yor.
                StartCoroutine(MoveToPosition(targetPosition, PictureList[index]));

                // �ndeks art�r�l�yor.
                index++;
            }
        }
    }

    private void MovePictureTwent(int rows, int columns, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for (var col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                // Hedef konum, resimlerin x ve y pozisyonlar�na dayal� olarak belirleniyor.
                var targetPosition = new Vector3(((pos.x - 1) + ((offset.x - 1) * row) + 2), ((pos.y) - ((offset.y - 1) * col)), 0.0f);

                // Resimleri hedef konumlar�na ta��mak i�in Coroutine kullan�larak MoveToPosition metoduna ba�lat�l�yor.
                StartCoroutine(MoveToPosition(targetPosition, PictureList[index]));

                // �ndeks art�r�l�yor.
                index++;
            }
        }
    }



    private IEnumerator MoveToPosition(Vector3 target, Picture obj)
    {
        // Resmin hedef konuma ula�mas� i�in kullan�lacak olan rastgele bir mesafe.
        var randomDis = 7;

        // Resmin hedef konuma ula�ana kadar d�ng� devam eder.
        while (obj.transform.position != target)
        {
            // Resmin mevcut konumu ile hedef konumu aras�ndaki mesafeyi kapat�r.
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);

            // Bir sonraki frame'i bekler.
            yield return null;
        }
    }
}
