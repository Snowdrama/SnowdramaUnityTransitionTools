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
        private bool rotateTransitionsOnHide;
        [SerializeField]
        private bool rotateTransitionsOnShow;

        [SerializeField]
        private int transitionIndex = 0;

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
            SceneController.transitionCallbacks.onHideCompleted += OnHideCompleted;
            SceneController.transitionCallbacks.onScenesLoaded += OnScenesLoaded;
            SceneController.transitionCallbacks.onShowStarted += OnShowStarted;
            SceneController.transitionCallbacks.onTransitionCompltete += OnTransitionComplete;
            SceneController.transitionCallbacks.onShowCompleted += OnShowCompleted;
        }

        private void OnDisable()
        {
            SceneController.transitionCallbacks.onTransitionStarted -= OnTransitionStarted;
            SceneController.transitionCallbacks.onHideStarted -= OnHideStarted;
            SceneController.transitionCallbacks.onHideCompleted -= OnHideCompleted;
            SceneController.transitionCallbacks.onScenesLoaded -= OnScenesLoaded;
            SceneController.transitionCallbacks.onShowStarted -= OnShowStarted;
            SceneController.transitionCallbacks.onTransitionCompltete -= OnTransitionComplete;
            SceneController.transitionCallbacks.onShowCompleted -= OnShowCompleted;
        }

        private void Start()
        {
            FindTransitions();
            transitionCanvas?.SetActive(false);
            currentTransition?.gameObject?.SetActive(false);
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

            if(transitionList == null)
            {
                transitionList = new List<Transition>();
            }

            if (transitionCanvas.transform.childCount != transitionList.Count)
            {
                transitionList.Clear();
            }

            Debug.Log($"Checking for child transitions");
            for (int i = 0; i < transitionCanvas.transform.childCount; i++)
            {
                var child = transitionCanvas.transform.GetChild(i);
                Debug.Log($"Checking {child.name}");

                var transitionElement = child.GetComponent<Transition>();
                if (transitionElement)
                {
                    if (!transitionList.Contains(transitionElement))
                    {
                        transitionList.Add(transitionElement);
                    }
                }
            }

            ValidateTransition();
        }

        public void OnTransitionStarted()
        {
            if (pauseTimeDuringTransition)
            {
                Time.timeScale = 0;
            }

            ValidateTransition();

            RandomizeTransition(randomizeHideTransition);
            RotateToNextTransition(rotateTransitionsOnHide);

            currentTransition?.gameObject?.SetActive(true);

            transitionCanvas?.SetActive(true);
            currentTransition?.OnTransitionStarted();
        }
        public void OnHideStarted()
        {
            currentTransition?.OnHideStarted();
            hiding = true;
        }
        public void OnHideCompleted()
        {
            currentTransition?.OnHideCompleted();
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

            RandomizeTransition(randomizeShowTransition);
            RotateToNextTransition(rotateTransitionsOnShow);

            currentTransition?.OnShowStarted();
        }
        public void OnShowCompleted()
        {
            currentTransition?.OnShowCompleted();
        }
        public void OnTransitionComplete()
        {
            if (pauseTimeDuringTransition)
            {
                Time.timeScale = 1;
            }
            transitionCanvas?.SetActive(false);
            currentTransition?.OnTransitionComplete();
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


        public void ValidateTransition()
        {
            if(transitionList.Count == 0)
            {
                Debug.LogError($"Transition list has 0 transitions, one transition is required", this.gameObject);
                return;
            }

            if (currentTransition == null)
            {
                transitionIndex = 0;
                currentTransition = transitionList[transitionIndex];
            }
        }

        public void RandomizeTransition(bool randomize)
        {
            if (randomize && transitionList.Count > 0)
            {
                transitionIndex = Random.Range(0, transitionList.Count);
                currentTransition?.gameObject?.SetActive(false);
                currentTransition = transitionList[transitionIndex];
                currentTransition?.gameObject?.SetActive(true);
            }
        }
        public void RotateToNextTransition(bool shouldRotate)
        {
            if (shouldRotate && transitionList.Count > 0)
            {
                transitionIndex++;
                transitionIndex = transitionIndex % transitionList.Count;

                currentTransition?.gameObject?.SetActive(false);

                currentTransition = transitionList[transitionIndex];
                currentTransition?.gameObject?.SetActive(true);
            }
        }
    }
}