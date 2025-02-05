using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public event UnityAction levelStarted;

    [SerializeField] private TMP_Text _beginTimer;
    [SerializeField] private TMP_Text _levelTimer;
    [SerializeField] private TMP_Text _finishTime;

    public FinishZone finishZone { get; private set; }

    [SerializeField] private Button _restartButton;

    [SerializeField] private Button[] _chooseLevelButtons;

    [SerializeField] private AIController[] _aiControllers;

    [SerializeField] private GameObject _trackChoosePanel;
    [SerializeField] private GameObject _beginPanel;
    [SerializeField] private GameObject _finishPanel;

    [SerializeField] private TrackLoader _trackLoader;

    private Coroutine _levelCoroutine;

    private float _beginTime = 3;
    private float _levelTime = 0;

    void Start()
    {
        InitButtons();
    }
    private void InitButtons()
    {
        for (int i = 0; i < _chooseLevelButtons.Length; ++i)
        {
            int levelIndex = i;
            _chooseLevelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
        }
        _restartButton.onClick.AddListener(RestartLevel);
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        finishZone.levelFinnished -= StopLevelTimer;
    }
    private void LoadLevel(int num)
    {
        _trackLoader.LoadTrack(num);
        finishZone = _trackLoader.currentTrack.GetComponentInChildren<FinishZone>();
        finishZone.levelFinnished += StopLevelTimer;
        StartCoroutine(BeginTimerCoroutine());
        _levelCoroutine = StartCoroutine(LevelTimerCoroutine());
        _trackChoosePanel.SetActive(false);
        _beginPanel.SetActive(true);
        for (int i = 0; i < _aiControllers.Length; ++i)
        {
            _aiControllers[i].IntallWaypoints(num);
        }
    }
    private void StopLevelTimer()
    {
        StopCoroutine(_levelCoroutine);
        _finishTime.text = (_levelTime).ToString("F2");
        _finishPanel.SetActive(true);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private IEnumerator LevelTimerCoroutine()
    {
        while (true)
        {
            _levelTime += Time.deltaTime;
            _levelTimer.text = (_levelTime).ToString("F2");
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    private IEnumerator BeginTimerCoroutine()
    {
        _beginTimer.text = ((int)_beginTime).ToString();
        while (_beginTime != 0)
        {
            yield return new WaitForSeconds(1f);
            _beginTime -= 1f;
            _beginTimer.text = ((int)_beginTime).ToString();
        }
        _beginPanel.SetActive(false);
        _levelTimer.gameObject.SetActive(true);
        levelStarted.Invoke();
    }
}
