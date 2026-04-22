using UnityEngine;
using UnityEngine.UI;

namespace Work.Jiwon.Code.UI
{
    public class UISlot : MonoBehaviour
    {
        [SerializeField] private Image _iconSlot;

        public void InitSlot(Sprite icon)
        {
            _iconSlot.enabled = true;
            _iconSlot.sprite = icon;
        }
        
        public void ClearSlot()
        {
            _iconSlot.enabled = false;
            _iconSlot.sprite = null;
        }
    }
}