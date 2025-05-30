using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum GameState { Title, Pregame, Gameplay, Endgame };

public class GameManager : Singleton<GameManager>
{

    [Header("Spawning")]
    [SerializeField]
    Spawner Spawner;
    [SerializeField]
    int SpawnsPerWave;

    [Header("Times")]
    [SerializeField]
    [Tooltip("How long the pre-game phase lasts, in seconds.")]
    float CountdownTime = 3.0f;
    [SerializeField]
    [Tooltip("How long the gameplay phase lasts, in seconds.")]
    float GameDuration = 60.0f;
    [SerializeField]
    [Tooltip("The amount of seconds it takes for an endgame element to show up.")]
    float TimeBetweenElements;

    [Header("Scoring")]
    [SerializeField]
    int ScorePerTarget;
    [SerializeField]
    int ScorePerWave;


    //Stats
    int _iScore = 0;
    int _iShotsFired = 0;
    int _iTargetsHit = 0;
    int _iTargetsRemaining;

    //Countdowns
    float _fGameplayCountdown;
    float _fPregameCountdown;
    float _fEndgameCountdown;

    //State tracking
    GameState _currGameState;
    bool _bCanRestart = false;


    private void Start()
    {
        Cursor.visible = false;
        ChangeScreen(GameState.Title);
        _fPregameCountdown = CountdownTime;
        _fGameplayCountdown = GameDuration;
        _fEndgameCountdown = TimeBetweenElements;
    }

    private void Update()
    {
        switch(_currGameState)
        {
            case GameState.Pregame:
                _fPregameCountdown -= Time.deltaTime;
                if (_fPregameCountdown <= 0)
                {
                    ChangeScreen(GameState.Gameplay);
                }
                break;
            case GameState.Gameplay:
                _fGameplayCountdown -= Time.deltaTime;
                if (_fGameplayCountdown <= 0)
                {
                    ChangeScreen(GameState.Endgame);
                }
                break;
            case GameState.Endgame:
                _fEndgameCountdown -= Time.deltaTime;
                if(_fEndgameCountdown <= 0 && !_bCanRestart)
                {
                    _fEndgameCountdown = TimeBetweenElements;
                    UIManager.Instance.ShowNextEndgameElement();
                }
                break;
            default:
                break;
        }

    }

    #region Game loop

    private void SpawnWave()
    {
        for (int i = 0; i < SpawnsPerWave; i++)
        {
            Spawner.Spawn();
        }
        _iTargetsRemaining = SpawnsPerWave;
    }

    private void ChangeScreen(GameState newState)
    {
        _currGameState = newState;
        UIManager.Instance.SetScreen(newState);

        if (newState == GameState.Gameplay)
        {
            StartGame();
        }
        else
        {
            Player.Instance.ChangeCamera(true);
            Player.Instance.SetVisiblity(false);
        }
    }
    private void StartGame()
    {
        SpawnWave();
        _fGameplayCountdown = GameDuration;
        Player.Instance.SetVisiblity(true);
    }
    #endregion

    #region Responding to other classes

    public void TargetHit()
    {
        _iScore += ScorePerTarget;
        _iTargetsHit++;
        _iTargetsRemaining--;

        //When every target has been destroyed
        if (_iTargetsRemaining <= 0)
        {
            _iScore += ScorePerWave;
            SpawnWave();
        }
    }

    public void PlayerFired()
    {
        _iShotsFired++;
    }

    #endregion

    #region Player actions
    //context.performed makes sure that the action only happens once per input
    //Player inputs should only function during the gameplay state


    public void Shoot(InputAction.CallbackContext context)
    {
        if (Player.Instance != null && context.performed && _currGameState == GameState.Gameplay)
        {
            Player.Instance.Shoot();
        }
    }

    public void AdjustPlayerYaw(InputAction.CallbackContext context)
    {
        if (Player.Instance != null && context.performed && _currGameState == GameState.Gameplay)
        {
            Player.Instance.AdjustYaw(context.ReadValue<float>());
        }

    }

    public void AdjustCannonPitch(InputAction.CallbackContext context)
    {
        if (Player.Instance != null && context.performed && _currGameState == GameState.Gameplay)
        {
            Player.Instance.AdjustCannonPitch(context.ReadValue<float>());
        }
    }

    public void ChangeCamera(InputAction.CallbackContext context)
    {
        if (Player.Instance != null && context.performed && _currGameState == GameState.Gameplay)
        {
            Player.Instance.ChangeCamera();
        }
    }
    #endregion

    #region UI

    public void Click(InputAction.CallbackContext context)
    {
        switch(_currGameState)
        {
            case GameState.Title:
                ChangeScreen(GameState.Pregame);
                break;
            case GameState.Endgame:
                if(_bCanRestart)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                break;
            default:
                break;
        }
    }

    public void UnlockRestart()
    {
        _bCanRestart = true;
    }

    public int GetPregameCountdown()
    {
        return Mathf.CeilToInt(_fPregameCountdown);
    }

    public int GetScore()
    {
        return _iScore;
    }

    public int GetShotsFired()
    {
        return _iShotsFired;
    }

    public int GetTargetsHit()
    {
        return _iTargetsHit;
    }

    public float GetTimeRemaining()
    {
        return _fGameplayCountdown;
    }
    #endregion
}
