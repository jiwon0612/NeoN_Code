using UnityEngine;
using Work.Jiwon.Code.SkillSystem;

namespace Assets.Work.Junser._00.Script
{
    class EventTester : MonoBehaviour
    {
        public void Test(SkillDataSO obj)
        {
            Debug.Log(obj.ToString());
        }
    }
}
