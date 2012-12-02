using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buddy.Gw2;
using Buddy.Gw2.Objects;
using iGuardian.Methods;
using Typhon.BehaviourTree;

namespace iGuardian.Wrappers
{
    internal class CreateWeaponSwitchBehavior : Composite
    {
        private WeaponType WeaponType { get; set; } 
        private Func<RoutineContext, bool> Condition { get; set; }

        internal CreateWeaponSwitchBehavior(WeaponType weaponType, Func<RoutineContext, bool> condition = null)
        {
            WeaponType = weaponType;
            Condition = condition;
        }

        protected override IEnumerable<RunStatus> Execute(object context)
        {
            RoutineContext ctx = context as RoutineContext;
            if (ctx != null)
            {
                if (ctx.CurrentWeapon != WeaponType)
                {
                    bool currentCondition = Condition == null || Condition(ctx);
                    if (currentCondition)
                    {
                        Logger.WriteVerbose("Switching weapons from {0} to {1}.", ctx.CurrentWeapon.ToString(), this.WeaponType.ToString());
                        BuddyGw.Me.SwitchWeapons();
                        return new List<RunStatus>() { RunStatus.Success}; 
                    }
                }
            }
            return new List<RunStatus>() { RunStatus.Failure };
        }
    }
}
