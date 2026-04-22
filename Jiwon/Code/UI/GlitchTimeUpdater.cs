using System;
using UnityEngine;
using UnityEngine.UI;

namespace Work.Jiwon.Code.UI
{
    public class GlitchTimeUpdater : MonoBehaviour
    {
        private Material _material;

        private void Start()
        {
            _material = GetComponent<Image>().material;
        }

        private void Update()
        {
            if (_material != null)
            {
                _material.SetFloat("_UnscaledTime", Time.unscaledTime);
            }
        }
    }
}