using CastleMagic.Game;
using CastleMagic.Game.Entites;
using CastleMagic.Util.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CastleMagic.UI {
    public class EntitySelectionManager : MonoBehaviour {

        private List<GameObject> highlighters = new List<GameObject>();
        private BoardManager boardManager;

        void Start() {
            boardManager = GameObject.FindWithTag("Board").GetComponent<BoardManager>();
        }

        void Update() {

        }
        
        public void SelectEntity(EntityController entity) {
            Debug.Log("Selected " + entity);
            highlighters.ForEach(it => {
                Destroy(it);
            });
            highlighters.Clear();
            var prefab = Resources.Load("Prefabs/UI/HexHighlighter") as GameObject;
            foreach (Tuple<HexCoord, int> pair in boardManager.board.PerformBFS(entity.HexTransform.Position, entity.energy)) {
                Debug.Log("Adding highlighter to " + pair);
                var obj = Instantiate(prefab);
                var trs = obj.GetComponent<HexTransform>();
                trs.Position = pair.Item1;
                trs.UpdatePhysicalPosition();
                highlighters.Add(obj);
            }
        }
    }
}