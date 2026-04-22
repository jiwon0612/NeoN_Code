using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Work.Jiwon.Code.SkillSystem;
using Work.JW.Code.Core.EventSystems;
using Random = UnityEngine.Random;

public class StoreSystem : MonoBehaviour
{
    private Dictionary<StoreDisplayer, SkillDataSO> displayedItem = new Dictionary<StoreDisplayer, SkillDataSO>();

    private StoreDisplayer selectedItemView;
    [SerializeField] private List<SkillDataSO> purchaceableItemContainer;
    private List<SkillDataSO> currentItemContainer;
    [SerializeField] private Image backGround;
    private List<StoreDisplayer> displayers = new List<StoreDisplayer>();
    [SerializeField] private float selectionFadeDuration;

    public UnityEvent<SkillDataSO> OnItemPurchace;

    private void Awake()
    {
        currentItemContainer = purchaceableItemContainer.ToList();
    }

    private void OnEnable()
    {
        SetItemOnDisplay();
    }
    public void SetItemOnDisplay()
    {
        ShuffleSkills(ref currentItemContainer);
        displayedItem.Clear();
        for (int i = 0; i < 3; i++)
        {
            if (displayers.Count <= i)
            {
                displayers.Add(transform.GetChild(i).GetComponentInChildren<StoreDisplayer>());
            }

            displayers[i].InitPanel();
            if (currentItemContainer.Count > i)
            {
                displayedItem.Add(displayers[i], currentItemContainer[i]);
                displayers[i].SetDisplayItem(currentItemContainer[i]);
            }
            else
            {
                displayers[i].SetDisplayItem(null);
            }
            displayers[i].ResetPanel();
        }
    }

    private void ShuffleSkills<T>(ref List<T> skillList)
    {
        for(int i = 0; i < skillList.Count - 1; i++)
        {
            int randomIndex = Random.Range(i, skillList.Count);
            T temp = skillList[randomIndex];
            skillList[randomIndex] = skillList[i];
            skillList[i] = temp;
        }
    }
    public void SetSelectedItem(StoreDisplayer storeDisplayer)
    {
        selectedItemView = storeDisplayer;
    }

    public void GetSelectedItem()
    {
        OnItemPurchace?.Invoke(displayedItem[selectedItemView]);
        if(displayedItem[selectedItemView].isActive == false)
        {
            currentItemContainer.Remove(displayedItem[selectedItemView]);
        }
        gameObject.SetActive(false);
    }

    public void SetDisplayImage(StoreDisplayer storeDisplayer, bool setRayTarget)
    {
        foreach(StoreDisplayer item in displayers)
        {
            if(item == storeDisplayer)
            {
                Sequence seq = DOTween.Sequence();
                seq.SetUpdate(true);
                if(setRayTarget == false)
                {
                    seq.AppendCallback(() => 
                    { 
                        backGround.gameObject.SetActive(true);
                    });
                }
                else
                {
                    seq.AppendCallback(() => 
                    {
                        backGround.gameObject.SetActive(false);
                    });
                }
                continue;
            }
            item.UIImage.raycastTarget = setRayTarget;
        }
    }
}