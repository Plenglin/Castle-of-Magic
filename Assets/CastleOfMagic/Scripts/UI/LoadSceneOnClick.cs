using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CastleMagic.UI {

    public class LoadSceneOnClick : MonoBehaviour {

        public string scene;

        private UnityAction action;

        private void Start() {
            action += DoLoadScene;
            GetComponent<Button>().onClick.AddListener(action);
        }

        public void DoLoadScene() {
            SceneManager.LoadScene(scene);
        }
    }

}