using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buddy.Gw2;
using Buddy.Gw2.Objects;
using Typhon.CommonBot;
using Typhon.CommonBot.Settings;

namespace iGuardian.Methods
{
    internal class iTargeting : ITargetingProvider
    {

        public List<Gw2Object> GetObjectsByWeight()
        {
            return BuddyGw.Objects.GetObjectsOfType<Gw2Character>().Where(o =>
                BuddyGw.Objects.IsValid(o) &&
                !Blacklist.Contains(o.AgentId) &&
                o.IsAlive &&
                !o.IsCritter && 
                o.Distance < CharacterSettings.Instance.PullRange &&
                (o.IsHostile || o.IsIndifferent) &&
                BuddyGw.IsVisible(o))
            .Select(o => new { Unit = o, Weight = Calculations.CalculateWeight(o) })
            .OrderByDescending(o => o.Weight)
            .Select(o => o.Unit)
            .Cast<Gw2Object>()
            .ToList();
        }

    }
}
