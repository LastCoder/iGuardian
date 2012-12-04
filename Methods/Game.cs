using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buddy.Gw2;
using Buddy.Gw2.Objects;
using Typhon.Common;
using Typhon.CommonBot;
using Typhon.CommonBot.Settings;

namespace iGuardian.Methods
{
    internal class Game
    {

        public static List<Gw2Character> GetSurrounding(Vector3 position, int radius)
        {
            return BuddyGw.Objects.GetObjectsOfType<Gw2Character>().Where(u => BuddyGw.Objects.IsValid(u) && !u.IsCritter
                && Calculations.Distance(u.Position, BuddyGw.Me.Position) <= radius).ToList();
        }

        public static List<Gw2Character> GetSurrounding(Vector3 position)
        {
            return GetSurrounding(position, 200);
        }

        public static List<Gw2Character> GetClusters(Vector3 position)
        {
            return BuddyGw.Objects.GetObjectsOfType<Gw2Character>().Where(u => BuddyGw.Objects.IsValid(u) && !u.IsFriendly &&
                (u.IsIndifferent || u.IsInCombat || u.IsHostile) && u.IsAlive).Select(o => new { Unit = o, Surrounding = GetSurrounding(o.Position).Count() }).OrderByDescending(o => o.Surrounding).Select(o => o.Unit).Cast<Gw2Character>().ToList();
        }

        public static List<Gw2Object> GetFriendliesByWeight()
        {
            return BuddyGw.Objects.GetObjectsOfType<Gw2Character>().Where(o =>
                BuddyGw.Objects.IsValid(o) &&
                !Blacklist.Contains(o.AgentId) &&
                o.IsAlive &&
                !o.IsCritter &&
                o.Distance < CharacterSettings.Instance.PullRange &&
                (o.IsFriendly) &&
                BuddyGw.IsVisible(o))
            .Select(o => new { Unit = o, Weight = Calculations.CalculateFriendlyWeight(o) })
            .OrderByDescending(o => o.Weight)
            .Select(o => o.Unit)
            .Cast<Gw2Object>()
            .ToList();
        }

        public static List<Gw2Object> GetEnemiesByWeight()
        {
            return BuddyGw.Objects.GetObjectsOfType<Gw2Character>().Where(o =>
                BuddyGw.Objects.IsValid(o) &&
                !Blacklist.Contains(o.AgentId) &&
                o.IsAlive &&
                !o.IsCritter &&
                o.Distance < CharacterSettings.Instance.PullRange &&
                (o.IsHostile || o.IsIndifferent) &&
                BuddyGw.IsVisible(o))
            .Select(o => new { Unit = o, Weight = Calculations.CalculateEnemyWeight(o) })
            .OrderByDescending(o => o.Weight)
            .Select(o => o.Unit)
            .Cast<Gw2Object>()
            .ToList();
        }

        public static List<Gw2Object> GetAllWeight()
        {
            return BuddyGw.Objects.GetObjectsOfType<Gw2Character>().Where(o =>
                BuddyGw.Objects.IsValid(o) &&
                !Blacklist.Contains(o.AgentId) &&
                o.IsAlive &&
                !o.IsCritter &&
                o.Distance < CharacterSettings.Instance.PullRange &&
                (o.IsHostile || o.IsIndifferent || o.IsFriendly) &&
                BuddyGw.IsVisible(o))
                .Select(o => new { Unit = o, Weight = o.IsFriendly ? Calculations.CalculateFriendlyWeight(o) : Calculations.CalculateEnemyWeight(o) })
            .OrderByDescending(o => o.Weight)
            .Select(o => o.Unit)
            .Cast<Gw2Object>()
            .ToList();
        }

        public static Gw2Character GetBestCluster()
        {
            return Game.GetClusters(BuddyGw.Me.Position).First();
        }
    }
}
