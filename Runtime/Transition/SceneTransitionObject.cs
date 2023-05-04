using System;
using System.Collections.Generic;
using UnityEngine;

namespace Snowdrama.Transition
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SceneTransitionObject", menuName = "Snowdrama/Transitions/Scene Transition")]
    public class SceneTransitionObject : ScriptableObject
    {

        [SerializeField] private SceneTransition sceneTransition;

        public void TransitionToThis()
        {
            Debug.Log("Going To Scene");
            SceneController.StartTransition(sceneTransition);
        }
    }
    public enum SceneTransitionMode
    {

        [Tooltip("Normal")]
        Normal,
        [Tooltip("Adds the listed scenes without removing any existing senes")]
        Additively,
    }


    [System.Serializable]
    public class SceneTransition
    {

        [Tooltip("How to manage the existing scnees")]
        [SerializeField] public SceneTransitionMode transitionMode;

        [Tooltip("A list of scenes to load")]
        [SerializeField] public List<SceneTransitionData> scenes;

        [Header("Time")]
        [SerializeField] public float hideSceneDuration;

        [Tooltip("Add a fake load time to make sure the transition doesn't look ugly when the scene loads too fast")]
        [SerializeField] public float fakeLoadBufferTime;
        [SerializeField] public float showSceneDuration;


        [Header("Force Unload DontDestroy Scene")]
        [Tooltip("A list of DontDestroy scenes to force unload")]
        [SerializeField] public List<string> doNotDestroyScenesToUnload;
    }

    [System.Serializable]
    public class SceneTransitionData
    {
        public string SceneName;
        public bool dontDestroyOnLoad;
        public bool reloadIfAlreadyExists;
    }


    [System.Serializable]

    public class SceneTransitionAsync_LoadData
    {
        public string sceneName;
        public AsyncOperation asyncOperation;
        public bool complete = false;
    }
}
