using System;
using UnityEngine;

namespace Work.JW.Code.Enemies.EnemyWeapons.Bullets
{
    public class BulletRenderer : MonoBehaviour
    {
        [ColorUsage(true,true)]
        [SerializeField] private Color enemyColor;
        
        [ColorUsage(true,true)]
        [SerializeField] private Color playerColor;
        
        private int _emissionHash = Shader.PropertyToID("_EmissionColor");
        private Material _myMat;

        private void Awake()
        {
            _myMat = GetComponent<Renderer>().material;
        }

        public void SetColor(bool isAttackToEnemy)
        {
            if(isAttackToEnemy)
                _myMat.SetColor(_emissionHash, playerColor);
            else
            {
                _myMat.SetColor(_emissionHash, enemyColor);
            }
        }
    }
}