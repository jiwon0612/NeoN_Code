using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SandevistanEffect : MonoBehaviour
{
    [SerializeField] private VolumeProfile profile;
    [SerializeField] Color lensColor;
    private LensDistortion lensDistortion;
    private ColorAdjustments adjustments;


    private void Awake()
    {
        profile.TryGet<LensDistortion>(out LensDistortion distortion);
        profile.TryGet<ColorAdjustments>(out ColorAdjustments adjustments);
        lensDistortion = distortion;
        this.adjustments = adjustments;
    }
    public void Active()
    {
        lensDistortion.intensity.value = 0;
        adjustments.colorFilter.value = Color.white;
        DOTween.To(() => adjustments.colorFilter.value, x => adjustments.colorFilter.value = x, lensColor, 0.3f).SetUpdate(true);
        DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, 0.6f, 0.3f).SetUpdate(true);
        
        lensDistortion.active = true;
    }

    public void Disactive()
    {
        DOTween.To(() => adjustments.colorFilter.value, x => adjustments.colorFilter.value = x, Color.white, 0.3f).SetUpdate(true);
        DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, 0, 0.3f).SetUpdate(true);
    }
}
