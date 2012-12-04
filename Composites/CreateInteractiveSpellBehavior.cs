using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buddy.Gw2;
using Buddy.Gw2.Objects;
using iGuardian.Methods;
using iGuardian.Wrappers;
using Typhon.BehaviourTree;
using Typhon.Common;

namespace iGuardian.Composites
{
    internal class CreateInteractiveSpellBehavior : Composite
    {
        private string SpellName { get; set; }
        private Gw2Character Object { get; set; } 
        private Func<RoutineContext, bool> Condition { get; set; }

        internal CreateInteractiveSpellBehavior(string spellName, Gw2Character Object, Func<RoutineContext, bool> condition = null)
        {
            SpellName = spellName;
            Condition = condition;
            this.Object = Object;
        }

        protected override IEnumerable<RunStatus> Execute(object context)
        {

            RoutineContext ctx = context as RoutineContext;
            if (ctx != null)
            {
                if (ctx.SkillNames.Contains(SpellName))
                {
                    Gw2Skill spell = ctx.GetSpell(SpellName);
                    if (spell != null)
                    {
                        bool currentCondition = Condition == null || Condition(ctx);
                        if (currentCondition)
                        {
                            if (spell.IsReady && BuddyGw.IsVisible(Object))
                            {
                                Logger.WriteVerbose("Casting {0} on position {1}.", spell.Name, Object.Position);
                                SpellManager.Cast(spell.Name, Object);
                                return new List<RunStatus>() { RunStatus.Success };

                            }
                        }
                    }
                }
            }
            return new List<RunStatus>() { RunStatus.Failure };

        }
    }
}
