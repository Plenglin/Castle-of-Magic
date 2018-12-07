using UnityEngine;
using System.Collections;
using CastleMagic.Game.Ability;
using System.ComponentModel;
using UnityEngine.UI;

namespace CastleMagic.UI.GameUI {

    public class AbilityButton : MonoBehaviour {

        [SerializeField]
        [ReadOnly(true)]
        private BaseAbility ability;

        public AbilityPanel parent;
        public Text abilityName, energyCost, charges;

        private Button button;

        private void Start() {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => {
                parent.OnAbilityClick(this);
            });
        }

        public void SetAbility(BaseAbility ability) {
            this.ability = ability;
            abilityName.text = ability.name;
            energyCost.text = ability.energy.ToString();
            charges.text = $"1/{ability.maxCharges}";
        }
        
    }

}