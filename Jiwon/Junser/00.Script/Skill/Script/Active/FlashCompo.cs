using UnityEngine;
using Work.Jiwon.Code.SkillSystem;
using Work.JW.Code.Entities;
using DG.Tweening;
using UnityEngine.UI;

public class FlashCompo : MonoBehaviour, IPoolable
{
    [SerializeField] private PoolManagerSO poolManager;
    Image flashImage;
    Pool _myPool;
    public GameObject GameObject => gameObject;
    private float _duration, _disableDuration, currentTime;
    [field: SerializeField] public PoolTypeSO PoolType { get; private set; }
    Sequence flashSequence;

    private void Awake()
    {
        flashImage = GetComponentInChildren<Image>();
    }
    public void ResetItem()
    {
        flashSequence.Kill();
        flashImage.color = new Color(1,1,1,0);
        gameObject.SetActive(true);
    }
    public void StartFlash(float duration, float disableDuration)
    {
        gameObject.SetActive(true);
        _duration = duration;
        _disableDuration = disableDuration;
        currentTime = Time.time;

        flashSequence = DOTween.Sequence();
        flashSequence.OnPlay(() =>flashImage.color = new Color(1,1,1,0));
        flashSequence.SetUpdate(true);
        flashSequence.Append(flashImage.DOFade(1, _duration));
        flashSequence.Append(flashImage.DOFade(0, _disableDuration));
        flashSequence.AppendCallback(() => _myPool.Push(this));
    }
    public void SetUpPool(Pool pool)
    {
        _myPool = pool;
    }
}
