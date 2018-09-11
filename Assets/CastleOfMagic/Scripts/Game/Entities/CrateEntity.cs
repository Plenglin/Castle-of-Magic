using System;
namespace CastleMagic.Game.Entites
{
    /// <summary>
    /// I am crate!
    /// </summary>
    public class CrateEntity : Entity
    {
        public CrateEntity(int maxHealth)
        {
            maxEnergy = 0;
            energy = 0;
            this.maxHealth = maxHealth;
            health = this.maxHealth;
            invunerable = false;
        }
    }
}
