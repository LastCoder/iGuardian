using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buddy.Gw2;
using iGuardian.Composites;
using iGuardian.Methods;
using iGuardian.Settings;
using iGuardian.Wrappers;
using Typhon.BehaviourTree;
using Action = Typhon.BehaviourTree.Action;

namespace iGuardian.Behaviors
{
    internal class Guardian
    {

        public static Composite GuardianCombat()
        {
            return new PrioritySelector(
                Downed(),
                Drowning(),
                Healing(),
                Elite(),
                Utilities(),
                WeaponSkills());
        }

        #region Regular Skills
        public static Composite Downed()
        {
            return new Decorator(ctx => true,
                new PrioritySelector(new CreateInteractiveSpellBehavior("Symbol of Judgment", BuddyGw.Me),
                new CreateSpellBehavior("Wave of Light"),
                new CreateSpellBehavior("Wrath"),
                new CreateSpellBehavior("Bandage")));
        }

        public static Composite Drowning()
        {
            return new Decorator(ctx => true,
                new PrioritySelector(new CreateSpellBehavior("Renewing Current"),
                new CreateSpellBehavior("Reveal the Depths"),
                new CreateSpellBehavior("Shackle"),
                new CreateSpellBehavior("Bandage")));
        }

        public static Composite Elite()
        {
            return new PrioritySelector(new PrioritySelector(
                    new CreateSpellBehavior("Judgment"),
                    new CreateSpellBehavior("Zealot's Fervor"),
                    new CreateSpellBehavior("Smiter's Boon"),
                    new CreateSpellBehavior("Affliction"),
                    new CreateSpellBehavior("Conflagrate")
                ),
                new CreateSpellBehavior("Tome of Wrath"),
                new PrioritySelector(
                    new CreateSpellBehavior("Light of Deliverance"),
                    new CreateSpellBehavior("Pacifism"),
                    new CreateSpellBehavior("Protective Spirit"),
                    new CreateSpellBehavior("Purifying Ribbon"),
                    new CreateInteractiveSpellBehavior("Heal Area", BuddyGw.Me)
                ),
                new CreateSpellBehavior("Tome of Courage"),
                new CreateSpellBehavior("Renewed Focus"));
        }

        public static Composite Utilities()
        {
            return new PrioritySelector(new CreateSpellBehavior("Command"),
                new CreateSpellBehavior("Sword of Justice"),
                new CreateSpellBehavior("Shield of the Avenger"),
                new CreateSpellBehavior("Hammer of Wisdom"),
                new CreateSpellBehavior("Bow of Truth"),
                new CreateSpellBehavior("Signet of Wrath"),
                new CreateSpellBehavior("Signet of Mercy"),
                new CreateSpellBehavior("Signet of Jugdment"),
                new CreateSpellBehavior("Bane Signet"),
                new CreateSpellBehavior("\"Stand Your Ground!\""),
                new CreateSpellBehavior("\"Save Yourselves!\""),
                new CreateSpellBehavior("\"Retreat!\""),
                new CreateSpellBehavior("\"Hold the Line!\""),
                new CreateSpellBehavior("Smite Condition"),
                new CreateSpellBehavior("Merciful Intervention"),
                new CreateSpellBehavior("Judge's Intervention", ctx => ctx.CountViableEnemies(1200f) >= iSettings.Instance.JudgeInterventionCount),
                new CreateSpellBehavior("Contemplation of Purity"),
                new CreateSpellBehavior("Wall of Reflection", ctx => iSettings.Instance.WallOfReflection ? ctx.DistanceToTarget > 500f : false),
                new CreateInteractiveSpellBehavior("Sanctuary", BuddyGw.Me),
                new CreateSpellBehavior("Purging Flames", ctx => iSettings.Instance.PurgingFlames ? ctx.Buffs.Count() > iSettings.Instance.PurgingFlamesCount : false),
                new CreateSpellBehavior("Hallowed Ground", ctx => iSettings.Instance.HallowedGround ? ctx.HasBuff("Stun") : false));
        }

        public static Composite Healing()
        {
            return new PrioritySelector(new CreateSpellBehavior("Signet of Resolve", ctx => ctx.CurrentPlayerHealthPercentage <= iSettings.Instance.SignetOfResolvePercentage),
                new CreateSpellBehavior("Shelter", ctx => ctx.CurrentPlayerHealthPercentage <= iSettings.Instance.ShelterPercentage),
                new CreateSpellBehavior("Healing Breeze", ctx => ctx.CurrentPlayerHealthPercentage <= iSettings.Instance.HealingBreezePercentage));
        }
        #endregion 

        #region Weapon Skills

        public static Composite WeaponSkills()
        {
            return new PrioritySelector(
                GreatSword(),
                Sword(),
                Torch(),
                Shield(),
                Focus(),
                Staff(),
                Trident(),
                Spear(),
                Sceptor(),
                Mace(),
                Hammer());
        }

        public static Composite Torch()
        {
            return new PrioritySelector(new CreateSpellBehavior("Cleansing Flame"),
                new CreateSpellBehavior("Zealot's Fire"),
                new CreateSpellBehavior("Zealot's Flame"));
        }

        public static Composite Shield()
        {
            return new PrioritySelector(new CreateSpellBehavior("Shield of Absorption"),
                new CreateSpellBehavior("Shield of Judgment", ctx => iSettings.Instance.ShieldOfJudgmentAOE > ctx.CountViableEnemies(30f) && iSettings.Instance.ShieldOfJudgmentPercentage > ctx.CurrentPlayerHealthPercentage));
        }

        public static Composite Focus()
        {
            return new PrioritySelector(new CreateSpellBehavior("Shield of Wrath"),
                new CreateSpellBehavior("Ray of Judgment"));
        }

        public static Composite Staff()
        {
            return new PrioritySelector(new CreateInteractiveSpellBehavior("Line of Warding", Game.GetBestCluster(), ctx => ctx.DistanceToTarget > 50),
                new CreateSpellBehavior("Empower"),
                new CreateInteractiveSpellBehavior("Symbol of Swiftness", BuddyGw.Me),
                new CreateSpellBehavior("Flash of Light"),
                new CreateSpellBehavior("Orb of Light"),
                new CreateSpellBehavior("Wave of Wrath"));
        }

        public static Composite Trident()
        {
            return new PrioritySelector(new CreateSpellBehavior("Weight of Justice"),
                new CreateSpellBehavior("Refraction"),
                new CreateSpellBehavior("Purifying Blast"),
                new CreateSpellBehavior("Purify"),
                new CreateSpellBehavior("Light of Judgment"));
        }

        public static Composite Spear()
        {
            return new PrioritySelector(new CreateSpellBehavior("Wrathful Grasp"),
                new CreateSpellBehavior("Spear Wall"),
                new CreateSpellBehavior("Brilliance"),
                new CreateSpellBehavior("Zealot's Flurry"),
                new CreateSpellBehavior("Spear of Light"));
        }

        public static Composite Sceptor()
        {
            return new PrioritySelector(new CreateSpellBehavior("Chains of Light"),
                new CreateInteractiveSpellBehavior("Smite", Game.GetBestCluster()),
                new CreateSpellBehavior("Orb of Wrath"));
        }

        public static Composite Mace()
        {
            return new PrioritySelector(new CreateSpellBehavior("Protector's Strike"),
                new CreateSpellBehavior("Symbol of Faith"),
                new CreateSpellBehavior("Faithful Strike"),
                new CreateSpellBehavior("Pure Strike"),
                new CreateSpellBehavior("True Strike"));
        }

        public static Composite Hammer()
        {
            return new PrioritySelector(new CreateSpellBehavior("Ring of Warding"),
                new CreateSpellBehavior("Banish"),
                new CreateSpellBehavior("Zealot's Embrace"),
                new CreateSpellBehavior("Mighty Blow"),
                new CreateInteractiveSpellBehavior("Symbol of Protection", BuddyGw.Me),
                new CreateSpellBehavior("Hammer Bash"),
                new CreateSpellBehavior("Hammer Swing"));
        }

        public static Composite Sword()
        {
            return new PrioritySelector(new CreateSpellBehavior("Zealot's Defense", ctx => ctx.DistanceToTarget <= iSettings.Instance.MininumRange && ctx.CurrentTargetHealthPercentage > iSettings.Instance.WhirlingBladeSinglePercent),
                new CreateSpellBehavior("Flashing Blade"),
                new CreateSpellBehavior("Sword of Wrath"),
                new CreateSpellBehavior("Sword Arc"),
                new CreateSpellBehavior("Sword Wave"));
        }

        public static Composite GreatSword()
        {
            return new PrioritySelector(new CreateSpellBehavior("Symbol of Wrath", ctx => ctx.DistanceToTarget <= iSettings.Instance.MininumRange),
                new CreateSpellBehavior("Whirling Wrath", ctx => ctx.CountViableEnemies(iSettings.Instance.WhirlingBladeAOERange) > iSettings.Instance.WhirlingBladeAOERange || ctx.CurrentTargetHealthPercentage > 60),
                new CreateSpellBehavior("Blinding Blade"),
                new CreateSpellBehavior("Pull", ctx => ctx.DistanceToTarget <= iSettings.Instance.MininumRange),
                new CreateSpellBehavior("Wrathful Strike"),
                new CreateSpellBehavior("Vengeful Strike"));
        }

        #endregion

        #region Weapon Switching
        #endregion
    }
}
