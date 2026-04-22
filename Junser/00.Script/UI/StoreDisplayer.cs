using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Work.Jiwon.Code.SkillSystem;
using Work.JW.Code.Core.EventSystems;

public class StoreDisplayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnSkillSelect;
    
    private RectTransform displayChild;
    private RectTransform uiTransform;
    private Image uiDisplay;
    [SerializeField] private GameEventChannelSO gameChannel;

    public Image UIImage { get { return uiDisplay; } private set { uiDisplay = value; } }

    [SerializeField] private TextMeshProUGUI skillName, detail;
    [SerializeField] private Image thumbNail;

    [SerializeField] private float sizeMultifly;
    [SerializeField] private float selectSizeMultifly;
    [SerializeField] private float duration;
    [SerializeField] private float selectDuration;
    [SerializeField] private float rotateDuration;
    [SerializeField] private float rotateScale;
    [SerializeField] private float item3DVolume;

    [SerializeField] private float holdShakeScale, holdShakeDuration;

    [SerializeField] private Image blinkPanel;

    [SerializeField] private StoreSystem storeSystem;

    private Vector3 uiOriginalSize;
    private Vector3 uiMultifliedSize;
    private Vector3 uiOriginPosition;
    private float width;
    private float height;
    private bool isSelected = false;
    private bool interactable = true;
    private bool inited = false;
    Sequence shakeSeq;

    [SerializeField] private float clickOverWindow, holdWindow;

    private SkillDataSO displayingItem;
    
    public void InitPanel()
    {
        if (inited) return;
        uiTransform = transform as RectTransform;
        storeSystem = GetComponentInParent<StoreSystem>();
        uiDisplay = GetComponent<Image>();
        displayChild = uiTransform.GetChild(0).transform as RectTransform;

        uiOriginalSize = uiTransform.localScale;
        uiMultifliedSize = uiTransform.localScale*sizeMultifly;
        uiOriginPosition = uiTransform.anchoredPosition;

        width = uiTransform.rect.width;
        height = uiTransform.rect.height;
        inited = true;
    }

    public void ResetPanel()
    {
        uiTransform.localScale = uiOriginalSize;
        uiTransform.anchoredPosition = uiOriginPosition;
        foreach (RectTransform child in displayChild)
        {
            child.anchoredPosition = new Vector3(child.anchoredPosition.x, child.anchoredPosition.y, 0);
        }
        displayChild.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactable)
            return;
        if (!isSelected)
        {
            uiTransform.DOScale(uiMultifliedSize, duration).SetUpdate(true);
        }
        foreach (Transform child in displayChild)
        {
            child.DOLocalMoveZ(-item3DVolume, duration).SetUpdate(true);
        }
        uiTransform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shakeSeq.Kill();
        if (!interactable)
            return;
        displayChild.DOLocalRotate(Vector3.zero, rotateDuration).SetUpdate(true);
        
        foreach (Transform child in displayChild)
        {
            child.DOLocalMoveZ(0, duration).SetUpdate(true);
        }
        if (!isSelected)
        {
            ReturnUI();
        }
    }

    private void ReturnUI()
    {
        uiTransform.DOAnchorPos(uiOriginPosition, duration).SetUpdate(true);
        uiTransform.DOScale(uiOriginalSize, duration).SetUpdate(true);
    }

    public void SetDisplayItem(SkillDataSO itemSO)
    {
        if(itemSO == null)
        {
            skillName.text = "";
            thumbNail.color = Color.clear;
            interactable = false;
            UIImage.raycastTarget = false;
            detail.SetText("");
            return;
        }
        interactable = true;
        UIImage.raycastTarget = true;
        displayingItem = itemSO;
        skillName.text = displayingItem.skillName;
        thumbNail.sprite = displayingItem.icon;
        thumbNail.color = Color.white;

        detail.SetText(displayingItem.description);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        if (!interactable)
            return;
        float xRot = (eventData.position.x-uiTransform.anchoredPosition.x-Screen.width/2) / (width / 2);
        float yRot = (eventData.position.y-uiTransform.anchoredPosition.y-Screen.height/2) / (height / 2);
        Vector3 targetRotation = new Vector3(-yRot * rotateScale, xRot * rotateScale, 0);
        displayChild.DORotate(-targetRotation, rotateDuration).SetUpdate(true);
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (!interactable)
            return;
            CancleBehave();
    }

    private void SelectBehave()
    {
        OnSkillSelect?.Invoke();
        
        gameChannel.RaiseEvent(GameEvents.GameStopEvent.Initializer(false));
        blinkPanel.DOFade(0,holdWindow).SetEase(Ease.OutCubic).SetUpdate(true);
        storeSystem.GetSelectedItem();
    }

    private void CancleBehave()
    {
        shakeSeq.Kill();
        blinkPanel.color = new Color(1,1,1,0);
        uiTransform.DOAnchorPos(uiOriginPosition, selectDuration);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        storeSystem.SetSelectedItem(this);
        shakeSeq = DOTween.Sequence();
        shakeSeq.SetUpdate(true);

        int repeat = (int)(holdWindow / holdShakeDuration);
        shakeSeq.Append(uiTransform.DOShakeAnchorPos(holdWindow, holdShakeScale, repeat, 90, true, false));
        shakeSeq.Join(blinkPanel.DOFade(1, holdWindow).SetEase(Ease.InCubic));

        shakeSeq.OnComplete(() =>
        {
            SelectBehave();
        });
        shakeSeq.Play();
    }
}
