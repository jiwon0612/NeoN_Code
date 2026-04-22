using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
public class ShakeCompo : MonoBehaviour
{
    Dictionary<RectTransform, Vector3> shakingObject = new Dictionary<RectTransform, Vector3>();
    public void Shake(float time, float power, RectTransform target)
    {
        target.DOKill();
        if (shakingObject.ContainsKey(target)==false)
        {
            shakingObject[target] = target.anchoredPosition;
        }
        else
        {
            target.anchoredPosition = shakingObject[target];
        }

        Sequence shake = DOTween.Sequence();
        shake.SetUpdate(true);
        shake.AppendCallback(() =>
        {
            if (shakingObject.ContainsKey(target))
            {
                target.anchoredPosition = shakingObject[target];
            }
        });

        shake.Append(target.DOShakeAnchorPos(time, power));

        shake.AppendCallback(() =>
        {
            if (shakingObject.ContainsKey(target))
            {
                target.anchoredPosition = shakingObject[target];
            }
        });

        shake.Play();
    }

}
