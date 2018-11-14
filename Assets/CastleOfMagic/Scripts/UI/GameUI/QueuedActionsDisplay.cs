using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CastleMagic.Game.GameInfo.PlayerActions;
using System.Collections.Generic;
using System;

namespace CastleMagic.UI.GameUI {
    public class QueuedActionsDisplay : MonoBehaviour {

        public List<TurnAction> actions;
        public Text text;

        // Use this for initialization
        void Start() {
            actions = new List<TurnAction>();
            text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update() {

        }

        public void AddAction(TurnAction action) {
            actions.Add(action);
            OnActionListChange();
        }

        public void UndoAction() {
            actions.RemoveAt(actions.Count - 1);
            OnActionListChange();
        } 

        void OnActionListChange() {
            text.text = "";
            foreach(TurnAction ta in actions) {
                text.text += ta + Environment.NewLine;
            }
        }

        public void OnNewTurn() {
            actions.Clear();
            OnActionListChange();
        }
    }
}