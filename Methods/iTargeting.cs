using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buddy.Gw2;
using Buddy.Gw2.Objects;
using iGuardian.Settings;
using Typhon.CommonBot;
using Typhon.CommonBot.Settings;

namespace iGuardian.Methods
{
    internal class iTargeting : ITargetingProvider
    {

        public List<Gw2Object> GetObjectsByWeight()
        {
            return iSettings.Instance.HealAndFight ? Game.GetAllWeight() : (iSettings.Instance.HealOnly ? Game.GetFriendliesByWeight() : Game.GetEnemiesByWeight());
        }

    }
}
