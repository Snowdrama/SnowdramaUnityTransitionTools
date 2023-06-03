using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Snowdrama.Transition
{
    [ExecuteAlways]
    public class TransitionDriver : MonoBehaviour
    {
        [SerializeField]
        private GameObject transitionCanvas;

        [SerializeField]
        private bool pauseTimeDuringTransition = false;

        [SerializeField]
        private List<Transition> transitionList;
        [SerializeField]
        private Transition currentTransition;

        [SerializeField]
        private bool randomizeHideTransition;
        [SerializeField]
        private bool randomizeShowTransition;

        private bool hiding;

        [Header("Debug")]
        [SerializeField, Range(0, 1)]
        public float debugTransitionValue;
        private void OnEnable()
        {
            SceneController.transitionCallbacks.onTransitionStarted += OnTransitionStarted;
            SceneController.transitionCallbacks.onHideStarted += OnHideStarted;
            SceneController.transitionCallbacks.onHideComplteted += OnHideComplteted;
            SceneController.transitionCallbacks.onScenesLoaded += OnScenesLoaded;
            SceneController.transitionCallbacks.onShowStarted += OnShowStarted;
            SceneController.transitionCallbacks.onTransitionCompltete += OnTransitionComplete;
            SceneController.transitionCallbacks.onShowComplteted += OnShowComplteted;
        }

        private void OnDisable()
        {
            SceneController.transitionCallbacks.onTransitionStarted -= OnTransitionStarted;
            SceneController.transitionCallbacks.onHideStarted -= OnHideStarted;
            SceneController.transitionCallbacks.onHideComplteted -= OnHideComplteted;
            SceneController.transitionCallbacks.onScenesLoaded -= OnScenesLoaded;
            SceneController.transitionCallbacks.onShowStarted -= OnShowStarted;
            SceneController.transitionCallbacks.onTransitionCompltete -= OnTransitionComplete;
            SceneController.transitionCallbacks.onShowComplteted -= OnShowComplteted;
        }

        private void Start()
        {
            FindTransitions();
        }
        private void OnValidate()
        {
            FindTransitions();
        }

        public void FindTransitions()
        {
            if (transitionCanvas == null)
            {
                transitionCanvas = this.transform.GetChild(0).gameObject;
            }
            if(transitionCanvas.transform.childCount != transitionList.Count)
            {
                transitionList.Clear();
            }
            for (int i = 0; i < transitionCanvas.transform.childCount; i++)
            {
                var child = transitionCanvas.transform.GetChild(i);

                var transitionElement = child.GetComponent<Transition>();
                if (transitionElement)
                {
                    if (!transitionList.Contains(transitionElement))
                    {
                        transitionList.Add(transitionElement);
                    }
                }
            }
            if (currentTransition == null && transitionList.Count > 0)
            {
                currentTransition = transitionList[0];
            }
        }

        public void OnTransitionStarted()
        {
            Debug.Log("Transition Started");
            transitionCanvas?.SetActive(true);
            if (pauseTimeDuringTransition)
            {
                Time.timeScale = 0;
            }
            if (currentTransition == null && transitionList.Count > 0)
            {
                currentTransition = transitionList[0];
            }

            if(randomizeHideTransition && transitionList.Count > 0)
            {
                currentTransition?.gameObject?.SetActive(false);
                currentTransition = transitionList.GetRandom();
            }

            currentTransition?.gameObject?.SetActive(true);
            currentTransition?.OnTransitionStarted();
        }
        public void OnHideStarted()
        {
            currentTransition?.OnHideStarted();
            hiding = true;
        }
        public void OnHideComplteted()
        {
            currentTransition?.OnHideComplteted();
            hiding = false;
        }
        public void OnScenesLoaded()
        {
            currentTransition?.OnScenesLoaded();
        }
        public void OnShowStarted()
        {
            if (currentTransition == null && transitionList.Count > 0)
            {
                currentTransition = transitionList[0];
            }

            if (randomizeShowTransition && transitionList.Count > 0)
            {
                currentTransition?.gameObject?.SetActive(false);
                currentTransition = transitionList.GetRandom();
            }
            currentTransition?.gameObject?.SetActive(true);
            currentTransition?.OnShowStarted();
        }
        public void OnShowComplteted()
        {
            currentTransition?.OnShowComplteted();
        }
        public void OnTransitionComplete()
        {

            Debug.Log("Transition Complete");
            currentTransition?.OnTransitionComplete();

            transitionCanvas?.SetActive(false);
            if (pauseTimeDuringTransition)
            {
                Time.timeScale = 1;
            }
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                debugTransitionValue = SceneController.transitionValue;
                currentTransition?.UpdateTransition(SceneController.transitionValue, hiding);
            }
            else
            {
                currentTransition?.UpdateTransition(debugTransitionValue, hiding);
            }
        }
    }
}