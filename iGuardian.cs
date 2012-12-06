using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Buddy.Gw2;
using Buddy.Gw2.Objects;
using iGuardian.Behaviors;
using iGuardian.Composites;
using iGuardian.GUI;
using iGuardian.Methods;
using iGuardian.Settings;
using iGuardian.Wrappers;
using Typhon.BehaviourTree;
using Typhon.Common;
using Typhon.CommonBot;
using Typhon.CommonBot.Bots;
using Typhon.CommonBot.Settings;
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


        public iGuardian()
        {
            Instance = this;
        }

        public static iGuardian Instance { get; private set; }

        private Window _configWindow;
        public WeaponType Primary, Secondary;
        private static readonly ContextChangeHandler ctxChanger = ctx => new RoutineContext();

        public bool startup = false;
        #endregion

        #region ICombatRoutine

        public static string ProjectName = "iGuardian";

        public override string Name { get { return ProjectName; } }
        public override Version Version { get { return new Version(0, 1, 3); } }
        public override string Author { get { return "iuser99";  } }

        public override void Initialize()
        {
            CombatTargeting.Instance.Provider = new iTargeting();
            Logger.Write("Initialized iGuardian version {0}", Version);
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

        public static void Log(string message, object arguements, bool debug = false)
        {
            message = string.Format(message, arguements);
            Log(message, debug);
        }

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
            if (iSettings.Instance.HealOnly)
            {
                return new PrioritySelector(
                    ctxChanger,
                    GuardianHealing());
            }
            return new PrioritySelector(
                ctxChanger,
                // Simple, but ugly, run-once action
                /*new Action(ctx => {
                    if (!startup)
                    {
                        Log("We are switching weapons 2 times to log the current weapon type; this is not an error if it only switches 2 times!");
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
                // Instigate combat
                // Regular walk 
                Movement.MoveIntoRangeBehavior(50f),
                GuardianCombat()
                );
        }

        public Composite GuardianCombat()
        {
            return new PrioritySelector(
                ctxChanger,
                //CombatUtilities.CreateWaitForCast(),
                // Weapon switches
                //new CreateWeaponSwitchBehavior(WeaponType.Sword, ctx => (Primary == WeaponType.Sword || Secondary == WeaponType.Sword) ? (ctx.CountViableEnemies(300f) < 1 || ctx.CurrentPlayerHealthPercentage < 50) : false),
                //new CreateWeaponSwitchBehavior(WeaponType.Greatsword, ctx => (Primary == WeaponType.Greatsword|| Secondary == WeaponType.Greatsword) ? (ctx.CurrentPlayerHealthPercentage > 50 || ctx.CountViableEnemies(300f) >= 1) : false),
                Guardian.GuardianCombat()                
                );
        }

        public Composite GuardianBuffs()
        {
            return new PrioritySelector(
                ctxChanger,
                // staff
                new CreateInteractiveSpellBehavior("Symbol of Swiftness", BuddyGw.Me, ctx => BuddyGw.Me.InCombat && ctx.CountViableEnemies(150) < 1)
                );
        }

        public Composite GuardianHealing()
        {
            return new PrioritySelector(
                ctxChanger,
                // staff
                new CreateInteractiveSpellBehavior("Line of Warding", Game.GetBestCluster(), ctx => ctx.DistanceToTarget >= 50), 
                new CreateSpellBehavior("Empower"),
                new CreateInteractiveSpellBehavior("Symbol of Swiftness", BuddyGw.Me), 
                new CreateSpellBehavior("Flash of Light"),
                new CreateSpellBehavior("Orb of Light"),
                new CreateSpellBehavior("Wave of Wrath")
                );
        }
        #endregion

        #region Event Handling

        #endregion
    }
}
