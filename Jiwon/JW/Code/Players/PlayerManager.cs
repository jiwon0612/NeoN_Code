using System;
using UnityEngine;
using Work.Jiwon.Code.Player;
using Work.JW.Code.Core.Dependencies;
using Work.JW.Code.Entities;

namespace Work.JW.Code.Players
{
    [DefaultExecutionOrder(-9)]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private EntityFinderSO playerFinder;
        [Inject] private Player _player;

        private void Awake()
        {
            playerFinder.SetEntity(_player);
        }
    }
}