using CastleMagic.Game;
using CastleMagic.Game.Selection;
using CastleMagic.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CastleMagic.UI.GameUI {  // TODO: don't double UI

    public class AbilityPanel : MonoBehaviour {

        public GameObject panel;
        private Lazy<EntitySelectionManager> selection = new Lazy<EntitySelectionManager>(() => FindObjectOfType<EntitySelectionManager>());
        private GameObject buttonPrefab;

        // Use this for initialization
        void Start() {
            buttonPrefab = Resources.Load("Prefabs/UI/AbilityButton") as GameObject;
            selection.Value.OnSelectionChange += OnSelectionChange;
        }

        // Update is called once per frame
        void OnSelectionChange() {
            Debug.Log("Selection changed. Updating ability list");
            var selected = selection.Value.selected;
            var rt = panel.GetComponent<RectTransform>();

            foreach (Transform child in panel.transform) {
                Destroy(child.gameObject);
            }

            if (selected != null && selected.abilities.Count > 0) {
                Debug.Log($"generating ability panel {selected.abilities}");
                // Generate buttons
                int i = 0;
                foreach (var ab in selected.abilities) {
                    GameObject child = Instantiate(buttonPrefab, panel.transform);
                    var crt = child.GetComponent<RectTransform>();
                    crt.anchoredPosition = new Vector3(0f, -5 - i * 40f);
                    var bt = child.GetComponent<AbilityButton>();
                    bt.SetAbility(ab);
                    bt.parent = this;
                    i++;
                }

                // Generate Panel
                rt.anchoredPosition = new Vector3(0, 0);
                rt.sizeDelta = new Vector2(50, 10 + 50f * selected.abilities.Count);
                panel.SetActive(true);
            } else {
                panel.SetActive(false);
            }
        }

        public void OnAbilityClick(AbilityButton button) {
            Debug.Log($"ability: {button} was clicked");
        }
    }

}