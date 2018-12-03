using System;
using UnityEngine.Networking;

namespace CastleMagic.Game.GameInfo.PlayerActions {
    public abstract class TurnAction {

        /// <summary>
        /// Executes the ghost action.
        /// </summary>
        /// <returns><c>true</c>, if ghost action was executed, <c>false</c> otherwise.</returns>
        public abstract bool ExecuteGhostAction();

        /// <summary>
        /// Undos the ghost action.
        /// </summary>
        public abstract void UndoGhostAction();

        /// <summary>
        /// Executes the action.
        /// </summary>
        public abstract void ExecuteAction();

        /// <summary>
        /// Like ToString() but worse.
        /// </summary>
        /// <returns>The action done represented as a cohesive sentance.</returns>
        public virtual String ActionToString() {
            return $"Action {this}";
        }
    }
}
