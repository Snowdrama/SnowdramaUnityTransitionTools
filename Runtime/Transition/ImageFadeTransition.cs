using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Snowdrama.Transition
{
    public class ImageFadeTransition : Transition
    {
        public Image transitionImage;
        public Color transitionColor;
        private void Start()
        {
            if(transitionImage == null)
            {
                transitionImage = GetComponent<Image>();
            }
        }
        public override void UpdateTransition(float transitionValue)
        {

            transitionColor.a = transitionValue;
            transitionImage.color = transitionColor;
        }
        private void OnValidate()
        {
            if (transitionImage == null)
            {
                transitionImage = GetComponent<Image>();
            }
        }
    }
}