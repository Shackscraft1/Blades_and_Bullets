using System;
using System.Collections;
using Game.Collectibles.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript Instance {get; private set;}
    [SerializeField] private Image abilityBarImage;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private RectTransform bombIconArea;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private TextMeshProUGUI gameOverText;
    public static EventHandler AbilityActiveStatus; 
    
    public static EventHandler OnPlayerDeath; 
    [SerializeField] private Slider hpSlider;
    private float _currentPlayerHp = 1f;
    private int _currentScore = 0;
    private int _HighScore = 0;
    
    public static EventHandler<OnHighScoreDataGatheredArgs> OnNewHighScoreChange;

    public class OnHighScoreDataGatheredArgs : EventArgs
    {
        public int newHighScore;
        public string nickName;
    }
    private enum HighScoreAchieved
    {
        NoHighScore,
        NewHighScore
    }
    private HighScoreAchieved _highSoreState = HighScoreAchieved.NoHighScore;


    private void Awake()
    {
        PlayerResourceInventory.OnSendPlayerData +=OnSendPlayerData;
    }

    private void OnSendPlayerData(object sender, PlayerResourceInventory.OnSendPlayerDataArgs e)
    {
        UpdateBomb(e.BombsRemaining);
        UpdateLives(e.LivesRemaining);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        Instance = this;
        abilityBarImage.type = Image.Type.Filled;
        abilityBarImage.fillMethod = Image.FillMethod.Vertical;
        abilityBarImage.fillAmount = 0f;
        Player.ModifyAbilityCooldown +=ModifyAbilityCooldown;
        // Player.OnPlayerGetsHit += PlayerGetsHit;
        SlashScript.OnSlashingSomething += OnSlashingSomething;
        SavedDataJSON.OnHighScoreDataGathered +=OnHighScoreDataGathered;
        

    }
    
    private void Update()
    {
        
    }


    private void OnHighScoreDataGathered(object sender, SavedDataJSON.OnHighScoreDataGatheredArgs e)
    {
        _HighScore = e.highScore;
        highScoreText.text =  ($"HighScore: {_HighScore.ToString("D6")}");
    }

    private void OnSlashingSomething(object sender, EventArgs e)
    {
        _currentScore += 100;
        ScoreChange(_currentScore);
    }

    void OnDestroy()
    {
        Player.ModifyAbilityCooldown -=ModifyAbilityCooldown;
        // Player.OnPlayerGetsHit -= PlayerGetsHit;
        SlashScript.OnSlashingSomething -= OnSlashingSomething;
        SavedDataJSON.OnHighScoreDataGathered -=OnHighScoreDataGathered;
        PlayerResourceInventory.OnSendPlayerData -=OnSendPlayerData;
    }

    // private void PlayerGetsHit(object sender, EventArgs e)
    // {
    //     _currentPlayerHp -= .15f;
    //     hpSlider.value = _currentPlayerHp;
    // if (_currentPlayerHp < .25f) HpDropsToZero();
    // }

    private void ScoreChange(int scoreChange)
    {
        currentScoreText.text =  ($"Score: {scoreChange.ToString("D6")}");
        if (scoreChange >= _HighScore)
        {
            _HighScore = scoreChange;
            highScoreText.text =  ($"HighScore: {_HighScore.ToString("D6")}");
            _highSoreState =  HighScoreAchieved.NewHighScore;
        }
    }

    private void ModifyAbilityCooldown(object sender, Player.ModifyAbilityCooldownArgs e)
    {
        if (e.changeAmount > 0f)
        {
            abilityBarImage.fillAmount += e.changeAmount;
            if (abilityBarImage.fillAmount >= 1f) AbilityActiveStatus?.Invoke(this, EventArgs.Empty);
            
        }
        else abilityBarImage.fillAmount = e.changeAmount;
    }

    private void HpDropsToZero()
    { 
        //When there's a new highscore a window will popup to ask for your nickname so you can replace the nickname field for that new string
        if(_highSoreState.Equals(HighScoreAchieved.NewHighScore)) OnNewHighScoreChange?.Invoke(this, new OnHighScoreDataGatheredArgs{nickName = "Player", newHighScore = _HighScore});
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        GameOverEvent();
    }

    private void GameOverEvent()
    {
        gameOverText.gameObject.SetActive(true);
        StartCoroutine(FinishGameScene());
    }
    
    public void LoadMainMenu()
    {
        StartCoroutine(_LoadCredits());
        

        IEnumerator _LoadCredits()
        {
            yield return new WaitForSeconds(5f);
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync("MainMenu");
            while(!loadOperation!.isDone) yield return null;
        }
    }
    
    private IEnumerator FinishGameScene()
    {
        yield return new WaitForSeconds(3f);
        //go back to main menu scene
        LoadMainMenu();
    }
    
    
    private void UpdateBomb(int bombsRemaining)
    {
        int currentBombs = bombIconArea.childCount;
        int maxCount = Mathf.Max(currentBombs, bombsRemaining);

        for (int i = 0; i < maxCount; i++)
        {
            if (i >= bombsRemaining && i < currentBombs)
            {
                Destroy(bombIconArea.GetChild(i).gameObject);
            }
            else if (i >= currentBombs && i < bombsRemaining)
            {
                Instantiate(bombPrefab, bombIconArea);
            }
        }
    }

    private void UpdateLives(int livesRemaining)
    {
        float normalized = (float)livesRemaining / 6.0f;
        _currentPlayerHp = 0.1f + normalized * (1f - 0.1f);
        hpSlider.value = _currentPlayerHp;
        if (_currentPlayerHp < .20)
        {
            HpDropsToZero();
        }
    }

    public float GetPlayerHP()
    {
        return _currentPlayerHp;
    }

}
