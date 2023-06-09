using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Snowdrama.Transition
{
    public abstract class Transition : MonoBehaviour
    {
        public abstract void UpdateTransition(float transitionValue, bool hiding);
        public virtual void OnTransitionStarted() { }
        public virtual void OnHideStarted() {}
        public virtual void OnHideComplteted() {}
        public virtual void OnScenesLoaded() {}
        public virtual void OnShowStarted() {}
        public virtual void OnShowComplteted() {}
        public virtual void OnTransitionComplete() {}
    }
}