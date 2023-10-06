using System;
using UnityEngine;
using Com.Durbol.Utility;
using System.Collections;
using TMPro;

[DefaultExecutionOrder(ConstantManager.KolpoManagerOrder)]
public class DurbolManager : MonoBehaviour, IHierarchyIcon
{
    public static Action OnAddKolpoBehaviour;
    public static Action<DurbolManager> OnSelfDistributionAction;
    public static Action<GameState> OnChangeGameState;
    public static Action OnNone;
    public static Action OnGameDataLoad;
    public static Action OnGameInitialize;
    public static Action OnGameStart;
    public static Action OnGameOver;
    public static Action OnCompleteTask;
    public static Action OnIncompleteTask;
    public static Action OnPauseGamePlay;
    public static Action OnUnPauseGamePlay;

    public static Action<DL_TappingType, Vector3> OnTap;
    public static Action<Vector3> OnDrag;
    public static Action<DL_SwippingType> OnSwip;
    public static Action<float> OnZoom;

    public GameplayData gameplayData = new GameplayData();
    public GameState gameState;
    public ParticleSystem gameOverEffect;
    public Animator gameStartingUI, gamePlayUI, gameSuccessUI, gameFaildUI, gameCustomUI;
    public TextMeshProUGUI levelText;
    public AudioClip gameWinAudioClip, gameLoseFailAudioClip;

    //[HideInInspector]
    public int totalGivenTask = 0, totalCompletedTask = 0, totalIncompleteTask = 0;
    [HideInInspector]
    public DurbolTools KolpoTools;
    public bool debugModeOn;

    GameState previousState;
    AudioSource runtimeAudioSource;

    private void OnEnable()
    {
        OnAddKolpoBehaviour += AddKolpoBehaviour;
    }

    private void OnDisable()
    {
        OnAddKolpoBehaviour -= AddKolpoBehaviour;
    }

    public virtual void Awake()
    {
        KolpoTools = DurbolTools.Instance;
        runtimeAudioSource = gameObject.AddComponent<AudioSource>();
        runtimeAudioSource.playOnAwake = false;
    }

    public virtual void Start()
    {
    }

    public void AddKolpoBehaviour()
    {
        //Debug.Log("New KolpoBehaviour Added Successfully.");
        OnSelfDistributionAction?.Invoke(this);
    }

    public virtual void ProcessTapping(DL_TappingType tappingType, Vector3 tapOnWorldSpace)
    {
        OnTap?.Invoke(tappingType, tapOnWorldSpace);
    }
    public virtual void ProcessDragging(Vector3 dragAmount)
    {
        OnDrag?.Invoke(dragAmount);
    }
    public virtual void ProcessSwipping(DL_SwippingType swippingType)
    {
        OnSwip?.Invoke(swippingType);
    }
    public virtual void ProcessZooming(float zoom)
    {
        OnZoom?.Invoke(zoom);
    }

    public virtual void OnCompleteATask()
    {
        totalCompletedTask++;
        OnCompleteTask?.Invoke();

        if (totalCompletedTask.Equals(totalGivenTask))
        {
            gameplayData.gameoverSuccess = true;
            gameplayData.gameEndTime = Time.time;

            ChangeGameState(GameState.GAME_PLAY_ENDED);
        }
    }

    public virtual void OnIncompleteATask()
    {
        totalIncompleteTask++;
        OnIncompleteTask?.Invoke();
    }

    public int GetModedLevelNumber()
    {
        //My Settings
        //return gameplayData.currentLevelNumber % ConstantManager.TOTAL_GAME_LEVELS;

        //Homa Settings
        if (gameplayData.currentLevelNumber >= ConstantManager.TOTAL_GAME_LEVELS)
            return ConstantManager.TOTAL_GAME_LEVELS - 1;
        return gameplayData.currentLevelNumber % ConstantManager.TOTAL_GAME_LEVELS;
    }

    public int GetValidLevelIndex()
    {
        //My Settings
        //int sceneIndex = GetModedLevelNumber() % (UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1);
        //return (sceneIndex < 1 ? 0 : sceneIndex) + 1;

        //Homa Settings
        int sceneIndex = GetModedLevelNumber();
        return (sceneIndex < 1 ? 0 : sceneIndex) + 2;
    }

    public int GetLevelMultiplayer()
    {
        return gameplayData.currentLevelNumber / ConstantManager.TOTAL_GAME_LEVELS;
    }

    public virtual void NextLevel()
    {
        KolpoTools.sceneManager.LoadNextLevel();
    }

    public virtual void ReloadLevel()
    {
        KolpoTools.sceneManager.ReLoadLevel();
    }

    public virtual void ChangeGameState(GameState gameState)
    {
        StartCoroutine(ChangeState(gameState));
    }

    IEnumerator ChangeState(GameState gameState)
    {
        yield return null;
        switch (gameState)
        {
            case GameState.NONE:
                None();
                break;
            case GameState.GAME_DATA_LOADED:
                GameDataLoad();
                break;
            case GameState.GAME_INITIALIZED:
                GameInitialize();
                break;
            case GameState.GAME_PLAY_STARTED:
                GameStart();
                break;
            case GameState.GAME_PLAY_ENDED:
                GameOver();
                break;
            case GameState.GAME_PLAY_PAUSED:
                PauseGamePlay();
                break;
            case GameState.GAME_PLAY_UNPAUSED:
                gameState = UnPauseGame(gameState);
                break;
        }

        Debug.Log("Executing: " + gameState.ToString());
        this.gameState = gameState;
        OnChangeGameState?.Invoke(gameState);
    }

    public virtual void None()
    {
        OnNone?.Invoke();
    }

    public virtual void GameDataLoad()
    {
        gameplayData = KolpoTools.savefileManager.LoadGameData();
        OnSelfDistributionAction?.Invoke(this);
        OnGameDataLoad?.Invoke();
    }

    public virtual void GameInitialize()
    {
        OnGameInitialize?.Invoke();
        levelText.text = "Level " + (gameplayData.currentLevelNumber + 1);
    }

    public virtual void GameStart()
    {
        gameplayData.gameStartTime = Time.time;
        OnGameStart?.Invoke();
    }

    public virtual void GameOver()
    {
        gameplayData.gameEndTime = Time.time;
        OnGameOver?.Invoke();

        if (gameplayData.gameoverSuccess)
        {
            gameplayData.currentLevelNumber++;
            //if(gameOverEffect != null)
            //    gameOverEffect.Play();
            PlayThisSoundEffect(gameWinAudioClip);
        }
        else
        {
            PlayThisSoundEffect(gameLoseFailAudioClip);
        }

        SaveGame();
    }

    public virtual void PlayThisSoundEffect(AudioClip audioClip)
    {
        runtimeAudioSource.clip = audioClip;
        runtimeAudioSource.Play();
    }

    public virtual void SaveGame()
    {
        KolpoTools.savefileManager.SaveGameData(gameplayData);
    }

    public virtual void PauseGamePlay()
    {
        previousState = gameState;
        OnPauseGamePlay?.Invoke();
    }

    public virtual GameState UnPauseGame(GameState gameState)
    {
        gameState = previousState;
        OnUnPauseGamePlay?.Invoke();
        return gameState;
    }

    public virtual void DistributeKolpoManager()
    {
        OnSelfDistributionAction?.Invoke(this);
    }

    public string EditorIconPath { get { return "KolpoManagerIcon"; } }
}
