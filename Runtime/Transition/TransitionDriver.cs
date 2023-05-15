using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Snowdrama.Transition
{
    public class TransitionDriver : MonoBehaviour
    {
        [SerializeField]
        private List<Transition> transitionList;
        [SerializeField]
        private Transition currentTransition;

        [SerializeField]
        private bool randomizeHideTransition;
        [SerializeField]
        private bool randomizeShowTransition;


        private bool hiding;
        private void OnEnable()
        {
            SceneController.transitionCallbacks.onTransitionStarted += OnTransitionStarted;
            SceneController.transitionCallbacks.onHideStarted += OnHideStarted;
            SceneController.transitionCallbacks.onHideComplteted += OnHideComplteted;
            SceneController.transitionCallbacks.onScenesLoaded += OnScenesLoaded;
            SceneController.transitionCallbacks.onShowStarted += OnShowStarted;
            SceneController.transitionCallbacks.onShowComplteted += OnShowComplteted;
        }

        private void OnDisable()
        {
            SceneController.transitionCallbacks.onTransitionStarted -= OnTransitionStarted;
            SceneController.transitionCallbacks.onHideStarted -= OnHideStarted;
            SceneController.transitionCallbacks.onHideComplteted -= OnHideComplteted;
            SceneController.transitionCallbacks.onScenesLoaded -= OnScenesLoaded;
            SceneController.transitionCallbacks.onShowStarted -= OnShowStarted;
            SceneController.transitionCallbacks.onShowComplteted -= OnShowComplteted;
        }

        public void OnTransitionStarted()
        {
            if(currentTransition == null && transitionList.Count > 0)
            {
                currentTransition = transitionList[0];
            }

            if(randomizeHideTransition && transitionList.Count > 0)
            {
                currentTransition = transitionList.GetRandom();
            }

            currentTransition.OnTransitionStarted();
        }
        public void OnHideStarted()
        {
            currentTransition.OnHideStarted();
            hiding = true;
        }
        public void OnHideComplteted()
        {
            currentTransition.OnHideComplteted();
            hiding = false;
        }
        public void OnScenesLoaded()
        {
            currentTransition.OnScenesLoaded();
        }
        public void OnShowStarted()
        {
            if (currentTransition == null && transitionList.Count > 0)
            {
                currentTransition = transitionList[0];
            }

            if (randomizeShowTransition && transitionList.Count > 0)
            {
                currentTransition = transitionList.GetRandom();
            }
            currentTransition.OnShowStarted();
        }
        public void OnShowComplteted()
        {
            currentTransition.OnShowComplteted();
        }
        public void OnTransitionComplete()
        {
            currentTransition.OnTransitionComplete();
        }

        private void Update()
        {
            currentTransition.UpdateTransition(SceneController.transitionValue, hiding);
        }
    }
}