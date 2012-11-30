using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Buddy.Gw2;
using Buddy.Gw2.Objects;
using iCombat.Composites;
using iCombat.GUI;
using iCombat.Wrappers;
using iGuardian.Settings;
using Typhon.BehaviourTree;
using Typhon.Common;
using Typhon.CommonBot;
using Typhon.DefaultRoutines;
using Action = Typhon.BehaviourTree.Action;

/**
 * iGuardian - The best Guardian routine on this side of the universe
 * Credits to superreen for help regarding routines
 **/
namespace iGuardian
{
    public class iGuardian : CombatRoutine
    {

        #region Variables

        private Window _configWindow;
        public WeaponType Primary, Secondary;
        private static readonly ContextChangeHandler ctxChanger = ctx => new RoutineContext();

        public bool startup = false;
        #endregion

        #region ICombatRoutine

        public static string ProjectName = "iGuardian";

        public override string Name { get { return ProjectName; } }
        public override Version Version { get { return new Version(0, 0, 8); } }
        public override string Author { get { return "iuser99";  } }

        public override void Initialize()
        {
            
        }

        public override void Dispose()
        {
            
        }

        public override Window ConfigWindow
        {
            get
            {
                if (_configWindow == null)
                {
                    _configWindow = new ConfigWindow("Configuration", Name,
                                                     "The iGuardian Defense System " + Version, 350, 400,
                                                     iSettings.Instance);
                    _configWindow.Closed += ConfigWindowClosed;
                }
                return _configWindow;
            }
        }
                
        public override Composite Combat
        {
            get { return GuardianCombat(); }
        }
                
        public override Composite OutOfCombat
        {
            get { return GuardianBuffs(); }
        }

        public override Composite Pull
        {
            get { return GuardianPull(); }
        }
        #endregion

        #region Logging Method

        public static void Log(string message, bool debug = false)
        {
            message = string.Format("[{0}{1}] {2}", ProjectName, debug ? " -> Debug" : "", message);
            if (debug)
                Logging.WriteDiagnostic(message);
            else
                Logging.Write(message);
        }

        public static void Log(string message, object arguements, bool debug = false)
        {
            message = string.Format(message, arguements);
            Log(message, debug);
        }

        #endregion

        #region Methods

        public void ConfigWindowClosed(object sender, EventArgs args)
        {
            iSettings.Instance.Save();
            _configWindow.Closed -= ConfigWindowClosed;
            _configWindow = null;
        }

        #endregion

        #region Behaviors

        public Composite GuardianPull()
        {
            return new PrioritySelector(
                // Simple, but ugly, run-once action
                /*new Action(ctx => {
                    if (!startup)
                    {
                        Primary = BuddyGw.Me.Inventory.CurrentWeaponType;
                        BuddyGw.Me.SwitchWeapons();
                        Secondary = BuddyGw.Me.Inventory.CurrentWeaponType;
                        BuddyGw.Me.SwitchWeapons();

                        if (Primary == WeaponType.Unknown || Secondary == WeaponType.Unknown)
                        {
                            Log("Current weapon set is not supported. Please contact {0}.", Author);
                        }

                        startup = true;
                    } 
                }),*/
                // Weapon switches
                //new CreateWeaponSwitchBehavior(WeaponType.Scepter, ctx => iSettings.Instance.SceptorPull ? (Primary == WeaponType.Scepter || Secondary == WeaponType.Scepter) : false),
                // Sceptor pull
                Movement.MoveIntoRangeBehavior(1200),
                new CreateSpellBehavior("Orb of Wrath"),
                // Greatsword pull
                Movement.MoveIntoRangeBehavior(600),
                new CreateSpellBehavior("Leap of Faith"),
                // Sword pull
                new CreateSpellBehavior("Flashing Blade"),
                // Regular walk 
                Movement.MoveIntoRangeBehavior(200),
                // Instigate combat
                GuardianCombat()
                );
        }

        public Composite GuardianCombat()
        {
            return new PrioritySelector(
                ctxChanger,
                CombatUtilities.CreateWaitForCast(),
                // Weapon switches
                //new CreateWeaponSwitchBehavior(WeaponType.Sword, ctx => (Primary == WeaponType.Sword || Secondary == WeaponType.Sword) ? (ctx.CountViableEnemies(300f) < 1 || ctx.CurrentPlayerHealthPercentage < 50) : false),
                //new CreateWeaponSwitchBehavior(WeaponType.Greatsword, ctx => (Primary == WeaponType.Greatsword|| Secondary == WeaponType.Greatsword) ? (ctx.CurrentPlayerHealthPercentage > 50 || ctx.CountViableEnemies(300f) >= 1) : false),
                // Healing skills
                new CreateSpellBehavior("Signet of Resolve", ctx => ctx.CurrentPlayerHealthPercentage < iSettings.Instance.SignetOfResolvePercentage),
                new CreateSpellBehavior("Shelter", ctx => ctx.CurrentPlayerHealthPercentage < iSettings.Instance.ShelterPercentage),
                new CreateSpellBehavior("Healing Breeze", ctx => ctx.CurrentPlayerHealthPercentage < iSettings.Instance.HealingBreezePercentage),
                // Greatsword combat
                new CreateSpellBehavior("Symbol of Wrath", ctx => ctx.DistanceToTarget < iSettings.Instance.MininumRange), 
                new CreateSpellBehavior("Whirling Wrath", ctx => ctx.CountViableEnemies(iSettings.Instance.WhirlingBladeAOERange) > iSettings.Instance.WhirlingBladeAOERange || ctx.CurrentTargetHealthPercentage > 60),
                new CreateSpellBehavior("Blinding Blade"),
                new CreateSpellBehavior("Pull", ctx => ctx.DistanceToTarget < iSettings.Instance.MininumRange),
                new CreateSpellBehavior("Wrathful Strike"),
                new CreateSpellBehavior("Vengeful Strike"),
                // Sword combat
                new CreateSpellBehavior("Zealot's Defense", ctx => ctx.DistanceToTarget < iSettings.Instance.MininumRange && ctx.CurrentTargetHealthPercentage > iSettings.Instance.WhirlingBladeSinglePercent),
                new CreateSpellBehavior("Flashing Blade"),
                new CreateSpellBehavior("Sword of Wrath"),
                new CreateSpellBehavior("Sword Arc"),
                new CreateSpellBehavior("Sword Wave"),
                // Hammer-Time
                new CreateSpellBehavior("Ring of Warding"),
                new CreateSpellBehavior("Banish"),
                new CreateSpellBehavior("Zealot's Embrace"),
                new CreateSpellBehavior("Mighty Blow"),
                new CreateSpellBehavior("Symbol of Protection", ctx => false),
                new CreateSpellBehavior("Hammer Bash"),
                new CreateSpellBehavior("Hammer Swing"),
                // Mace combat
                new CreateSpellBehavior("Protector's Strike"),
                new CreateSpellBehavior("Symbol of Faith"),
                new CreateSpellBehavior("Faithful Strike"),
                new CreateSpellBehavior("Pure Strike"),
                new CreateSpellBehavior("True Strike"),
                // Sceptor combat
                new CreateSpellBehavior("Chains of Light"),
                new CreateSpellBehavior("Smite", ctx => false), 
                new CreateSpellBehavior("Orb of Wrath"),
                // Spear combat
                new CreateSpellBehavior("Wrathful Grasp"),
                new CreateSpellBehavior("Spear Wall"),
                new CreateSpellBehavior("Brilliance"),
                new CreateSpellBehavior("Zealot's Flurry"),
                new CreateSpellBehavior("Spear of Light"),
                // Trident combat
                new CreateSpellBehavior("Weight of Justice"),
                new CreateSpellBehavior("Refraction"),
                new CreateSpellBehavior("Purifying Blast"),
                new CreateSpellBehavior("Purify"),
                new CreateSpellBehavior("Light of Judgment"),
                // Staff combat
                new CreateSpellBehavior("Line of Warding", ctx => false), 
                new CreateSpellBehavior("Empower"),
                new CreateSpellBehavior("Symbol of Swiftness", ctx => false), 
                new CreateSpellBehavior("Flash of Light"),
                new CreateSpellBehavior("Orb of Light"),
                new CreateSpellBehavior("Wave of Wrath"),
                // Focus combat
                new CreateSpellBehavior("Shield of Wrath"),
                new CreateSpellBehavior("Ray of Judgment"),
                // Shield combat
                new CreateSpellBehavior("Shield of Absorption"),
                new CreateSpellBehavior("Shield of Judgment", ctx => iSettings.Instance.ShieldOfJudgmentAOE > ctx.CountViableEnemies(30f) && iSettings.Instance.ShieldOfJudgmentPercentage > ctx.CurrentPlayerHealthPercentage),
                // Torch combat
                new CreateSpellBehavior("Cleansing Flame"),
                new CreateSpellBehavior("Zealot's Fire"),
                new CreateSpellBehavior("Zealot's Flame"),
                // Utility skills (not really implemented, due to lack of stack detection, etc.)
                new CreateSpellBehavior("Command"),
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
                new CreateSpellBehavior("Judge's Intervention", ctx => ctx.CountViableEnemies(1200f) > iSettings.Instance.JudgeInterventionCount),
                new CreateSpellBehavior("Contemplation of Purity"),
                new CreateSpellBehavior("Wall of Reflection", ctx => iSettings.Instance.WallOfReflection ? ctx.DistanceToTarget > 500f : false), 
                new CreateSpellBehavior("Sanctuary", ctx => false), 
                new CreateSpellBehavior("Purging Flames", ctx => iSettings.Instance.PurgingFlames ? ctx.Buffs.Count() > iSettings.Instance.PurgingFlamesCount : false), 
                new CreateSpellBehavior("Hallowed Ground", ctx => iSettings.Instance.HallowedGround ? ctx.GetBuff("Stun") != null : false),
                // Downed combat
                new CreateSpellBehavior("Symbol of Judgment", ctx => false), 
                new CreateSpellBehavior("Wave of Light"),
                new CreateSpellBehavior("Wrath"),
                new CreateSpellBehavior("Bandage"),
                // Drowning combat
                new CreateSpellBehavior("Renewing Current"),
                new CreateSpellBehavior("Reveal the Depths"),
                new CreateSpellBehavior("Shackle"),
                new CreateSpellBehavior("Bandage"),
                // Elite skillz
                new PrioritySelector(
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
                    new CreateSpellBehavior("Heal Area", ctx => false) 
                ),
                new CreateSpellBehavior("Tome of Courage"),
                new CreateSpellBehavior("Renewed Focus")
                );
        }

        public Composite GuardianBuffs()
        {
            return new Typhon.BehaviourTree.Action(ctx => RunStatus.Failure);
        }
        #endregion

        #region Event Handling

        #endregion
    }
}
