using Ami.BroAudio;
using Ami.BroAudio.Runtime;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.JW.Code.Core.EventSystems;

public class SettingUI : MonoBehaviour
{
    [SerializeField] GameEventChannelSO scoreChannel;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_InputField inputField;
    
    [SerializeField] private TMP_Text inputWarningMessage;
    [SerializeField] private TMP_Text saveMessage;
    
    private float currentVolume;
    private string currentName;
    private const string username = "user";
    private const string volume = "volume";

    private void Awake()
    {
        scoreChannel.AddListener<WarningChangeUserNameEvent>(HandleWarningChangeEvent);
        scoreChannel.AddListener<SaveUserScoreEvent>(HandleSaveUserNameEvent);
        
        slider.onValueChanged.AddListener(OnSliderValueChange);
        inputField.onValueChanged.AddListener(OnTextEnter);
        inputField.onEndEdit.AddListener(OnTextEnter);
        
        inputWarningMessage.gameObject.SetActive(false);
        saveMessage.gameObject.SetActive(false);
    }

    private void HandleWarningChangeEvent(WarningChangeUserNameEvent evt)
    {
        inputWarningMessage.text = evt.warningMessage;
        inputWarningMessage.gameObject.SetActive(true);
        inputField.text = "";
        DOVirtual.DelayedCall(2, () => inputWarningMessage.gameObject.SetActive(false));
    }
    
    private void HandleSaveUserNameEvent(SaveUserScoreEvent evt)
    {
        saveMessage.gameObject.SetActive(evt.isCompleted);

        if (evt.isCompleted)
        {
            DOVirtual.DelayedCall(1, () => saveMessage.gameObject.SetActive(false));
        }
    }

    private void OnEnable()
    {
        SetUIDefaultValue();
    }

    private void SetUIDefaultValue()
    {
        currentVolume = PlayerPrefs.GetFloat(volume, 1f);
        currentName = PlayerPrefs.GetString(username, "");
        slider.value = currentVolume;
        inputField.text = currentName;
    }
    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChange);
        inputField.onValueChanged.RemoveListener(OnTextEnter);
        inputField.onEndEdit.RemoveListener(OnTextEnter);
    }
    public void OnSliderValueChange(float val)
    {
        currentVolume = val;
    }

    public void OnTextEnter(string text)
    {
        currentName = text;
    }

    public void SetVolume()
    {
        currentVolume = slider.value;
        BroAudio.SetVolume(currentVolume);
        PlayerPrefs.SetFloat(volume, currentVolume);
    }

    public void Submit()
    {
        currentName = inputField.text;

        var evt = ScoreEvents.ChangeUserNameEvent.Initializer(currentName);
        scoreChannel.RaiseEvent(evt);
        
        inputField.text = "";
        
        PlayerPrefs.SetString(username, currentName);
        PlayerPrefs.Save();
    }
}
