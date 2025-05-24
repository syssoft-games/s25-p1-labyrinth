using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameManager : MonoBehaviour
{
    public GameState gameState = GameState.Playing;
    public GameObject player;
    public GameObject enemy;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public GameObject goal;
    public Light sun;
    public Camera mainCamera;
    public IntroCam introCamera;
    public GameObject minimapBorder;
    public GameObject minimapImage;
    public CompassArrow compassArrow;
    public AudioSource stepSound;

    #region Singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void StartPlaying()
    {
        Instance.mainCamera.enabled = true;
        Instance.gameState = GameState.Playing;
        Instance.minimapBorder.SetActive(true);
        Instance.minimapImage.SetActive(true);
        Instance.compassArrow.StartIndicating(Instance.goal.transform);
    }
    public void StartIntro(float x, float z)
    {
        Instance.introCamera.StartPan(x, z);
    }
    public void Die()
    {
        Instance.gameState = GameState.Lost;
        Instance.loseText.gameObject.SetActive(true);
        Instance.stepSound.Stop();
        Instance.stepSound.enabled = false;
    }
    public void Win(float timeTaken)
    {
        string timeMessage = $"gz, you beat the maze. Time: {timeTaken:F2}s";
        winText.text = timeMessage;
        winText.gameObject.SetActive(true);
        Instance.gameState = GameState.Won;
        Instance.stepSound.Stop();
        Instance.stepSound.enabled = false;
    }
}

public enum GameState
{
    Playing, Lost, Won
}