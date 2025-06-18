using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class CanvasManager : MonoBehaviour
{

    public AudioMixer mixer;
    private AudioSource audioSource;

    [SerializeField] private AudioClip pauseSound;

    [Header("Menu Canvases")]
    public GameObject pauseMenuCanvas;
    public GameObject hudCanvas;


    [Header("Text")]
    public TMP_Text masterVolSliderText;
    public TMP_Text livesText;
    public TMP_Text coinsText;

    [Header("Sliders")]
    public Slider masterVolSlider;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (masterVolSlider)
        {
            SetupSliderInformation(masterVolSlider, masterVolSliderText, "MasterVol");
            OnSliderValueChanged(masterVolSlider.value, masterVolSlider, masterVolSliderText, "MasterVol"); // Initialize the text with the current value
        }
        audioSource = GetComponent<AudioSource>();

        if (livesText)
        {
            GameManager.Instance.OnLivesChanged += UpdateLivesText;
            UpdateLivesText(GameManager.Instance.Lives); // Initialize the text with the current lives value
        }

        if (coinsText)
        {
            GameManager.Instance.OnCoinsChanged += UpdateCoinsText;
            UpdateCoinsText(GameManager.Instance.Coins); // Initialize the text with the current coins value
        }

    }

    private void SetupSliderInformation(Slider slider, TMP_Text sliderText, string parameterName)
    {
        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, slider, sliderText, parameterName));
    }

    private void OnSliderValueChanged(float value, Slider slider, TMP_Text sliderText, string parameterName)
    {
        value = (value == 0) ? -80 : Mathf.Log10(slider.value) * 20; // Convert to decibels
        sliderText.text = (value == -80) ? "0%" : $"{(int)(slider.value * 100)}%";
        mixer.SetFloat(parameterName, value);
    }

    private void UpdateLivesText(int value)
    {
        livesText.text = $"{value}";
    }

    private void UpdateCoinsText(int value)
    {
        coinsText.text = $"{value}";
    }


    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetMenus(GameObject canvasToActivate, GameObject canvasToDeactivate)
    {
        if (canvasToActivate) canvasToActivate.SetActive(true);
        if (canvasToDeactivate) canvasToDeactivate.SetActive(false);
    }

    private void resumeGame()
    {
        SetMenus(hudCanvas, pauseMenuCanvas);
        Time.timeScale = 1;
        GameManager.isPaused = false;
    }


    private void Update()
    {
        if (!pauseMenuCanvas) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenuCanvas.activeSelf)
            {
                SetMenus(hudCanvas, pauseMenuCanvas);
                Time.timeScale = 1;
                GameManager.isPaused = false;
            }
            else
            {
                SetMenus(pauseMenuCanvas, hudCanvas);
                audioSource.PlayOneShot(pauseSound);
                Time.timeScale = 0;
                GameManager.isPaused = true;

            }
        }
    }

}
