using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Work.Jiwon.Code.EventSystem;
using Work.JW.Code.Core.EventSystems;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO scoreChannel;
    [SerializeField] private GameEventChannelSO playerChannel;
    public int comboCount { get; private set; }

    public event Action<int> OnScoreChange;
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text comboCountUI;
    [SerializeField] private Image comboCircle;
    [SerializeField] private float comboResumeTime;

    [SerializeField] private float scoreAmount = 100f;
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private GameEventChannelSO gameChannel;

    private bool isStoped;
    //public event Action<int> OnComboValueChanged;
    public float currentScore { get; private set; } = 0;
    private float comboCharge = 0;
    private ShakeCompo shaker;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        shaker = GetComponent<ShakeCompo>();

        scoreChannel.AddListener<AddScoreEvent>(HandleAddScoreEvent);
        playerChannel.AddListener<PlayerDeadEvent>(HandlePlayerDeadEvent);
        gameChannel.AddListener<GameStopEvent>(HandleStopEvent);
    }

    private void HandleStopEvent(GameStopEvent stopEvent)
    {
        isStoped = stopEvent.isStop;
    }

    private void Update()
    {
        UpdateCombo();
    }

    private void OnEnable()
    {
        InitValue();
        ResetVisual();
    }

    private void OnDestroy()
    {
        scoreChannel.RemoveListener<AddScoreEvent>(HandleAddScoreEvent);
        playerChannel.RemoveListener<PlayerDeadEvent>(HandlePlayerDeadEvent);
    }

    private void ResetVisual()
    {
        SetComboUI();
        comboCircle.fillAmount = comboCharge;
    }

    private void InitValue()
    {
        comboCharge = 0;
        comboCount = 0;
    }

    private void UpdateCombo()
    {
        if (comboCount > 0 && isStoped == false)
        {
            comboCharge -= Time.deltaTime / comboResumeTime;
            DOTweenModuleUI.DOFillAmount(comboCircle, comboCharge, 0.1f);
            CheckCombo();
        }
    }

    private void HandleAddScoreEvent(AddScoreEvent evt)
    {
        AddCombo();
        /*float calcScore = evt.score * 100 *(4.5f* Mathf.Log10(comboCount)+1);
        currentScore += Mathf.RoundToInt(calcScore)/10*10;*/

        currentScore += evt.score * comboCount;

        SetScoreUI();
    }

    private void HandlePlayerDeadEvent(PlayerDeadEvent evt)
    {
        string curUserId = Environment.MachineName;
        var changeHighScoreEvent = ScoreEvents.ChangeHighScoreEvent.Initializer(curUserId, (int)currentScore);
        scoreChannel.RaiseEvent(changeHighScoreEvent);
    }

    private void CheckCombo()
    {
        if (comboCharge < 0)
        {
            comboCount = 0;
            SetComboUI();
        }
    }

    public void AddCombo()
    {
        comboCount++;
        comboCharge = 1;
        shaker.Shake(0.5f, 25, comboCircle.rectTransform);
        SetComboUI();
    }

    private void SetComboUI()
    {
        comboCountUI.SetText($"X{comboCount}");
    }

    private void SetScoreUI()
    {
        scoreText.SetText(Mathf.RoundToInt(currentScore).ToString());
    }

    public void OnScoreDoubleSkill(float multiflyer)
    {
        scoreAmount *= multiflyer;
    }

    public void OnComboDurationUpgrade(float multiflyer)
    {
        comboResumeTime *= multiflyer;
    }
}