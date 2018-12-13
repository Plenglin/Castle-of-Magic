using CastleMagic.Game;
using CastleMagic.Game.Entities;
using CastleMagic.Game.Selection;
using CastleMagic.UI;
using CastleMagic.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CastleMagic.UI.GameUI {  // TODO: don't double UI

    public class AbilityPanel : MonoBehaviour {

        public GameObject panel;
        private Lazy<SelectionManager> selection = new Lazy<SelectionManager>(() => FindObjectOfType<SelectionManager>());
        private GameObject buttonPrefab;

        private GameObjectPool pool;

        // Use this for initialization
        private void Start() {
            buttonPrefab = Resources.Load("Prefabs/UI/AbilityButton") as GameObject;
            selection.Value.OnSelectionChange += OnSelectionChange;
            pool = new GameObjectPool(() => {
                return Instantiate(buttonPrefab, panel.transform);
            });
        }

        // Update is called once per frame
        private void OnSelectionChange() {
            Debug.Log("Selection changed. Updating ability list");
            var entity = selection.Value.selected.GetComponent<EntityController>();

            if (entity != null && entity.abilities.Count > 0) {
                // Generate buttons
                Debug.Log($"generating ability panel {entity.abilities}");
                int i = 0;
                pool.Acquire(entity.abilities, (child, ab) => {
                    var crt = child.GetComponent<RectTransform>();
                    crt.anchoredPosition = new Vector3(0f, -5 - i * 40f);
                    var btn = child.GetComponent<AbilityButton>();
                    btn.SetAbility(ab);
                    btn.parent = this;
                    i++;
                });

                // Set panel size
                var rt = panel.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector3(0, 0);
                rt.sizeDelta = new Vector2(50, 10 + 50f * entity.abilities.Count);
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