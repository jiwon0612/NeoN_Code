using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Work.JW.Code.Core.EventSystems;

namespace Work.Jiwon.Code.UI
{
    public class SkillUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO skillChannel;

        [SerializeField] private UISlot _activeSkillSlot;
        [SerializeField] private Transform _passiveSkillSlot;

        [SerializeField] private UISlot _skillSlotPrefab;
        [SerializeField] private int defaultSlotCount = 5;

        private List<UISlot> _slotList;
        private int _slotIndex;

        private void Awake()
        {
            skillChannel.AddListener<GetSkillEvent>(HandleGetSkillEvent);
            skillChannel.AddListener<UseSkillEvent>(HandleUseSkillEvent);

            InitUI();
            _activeSkillSlot.ClearSlot();
        }

        private void InitUI()
        {
            _slotList = new List<UISlot>();

            for (int i = 0; i < defaultSlotCount; i++)
            {
                UISlot slot = Instantiate(_skillSlotPrefab, _passiveSkillSlot);
                slot.gameObject.SetActive(false);
                _slotList.Add(slot);
            }
        }

        private void OnDestroy()
        {
            skillChannel.RemoveListener<GetSkillEvent>(HandleGetSkillEvent);
            skillChannel.RemoveListener<UseSkillEvent>(HandleUseSkillEvent);
        }

        private void HandleGetSkillEvent(GetSkillEvent evt)
        {
            if (evt.skill.isActive)
            {
                _activeSkillSlot.InitSlot(evt.skill.icon);
            }
            else
            {
                if (_slotIndex >= defaultSlotCount)
                {
                    UISlot slotPrefab = Instantiate(_skillSlotPrefab, _passiveSkillSlot);
                    slotPrefab.InitSlot(evt.skill.icon);
                    _slotList.Add(slotPrefab);
                    _slotIndex++;
                }
                else
                {
                    UISlot slot = _slotList[_slotIndex];
                    slot.gameObject.SetActive(true);
                    slot.InitSlot(evt.skill.icon);
                    _slotIndex++;
                }
            }
        }

        private void HandleUseSkillEvent(UseSkillEvent evt)
        {
            _activeSkillSlot.ClearSlot();
        }
    }
}