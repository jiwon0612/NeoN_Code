using System;
using UnityEngine;
using Work.JW.Code.Enemies.States;

namespace Work.JW.Code.Test
{
    public class Tester : MonoBehaviour
    {
        [ContextMenu("Test")]
        private void Test()
        {
            print(Enum.GetValues(typeof(EnemyStateType)).Length);
        }
    }
}