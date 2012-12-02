using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Buddy.Gw2;
using Buddy.Gw2.Objects;
using Typhon.BehaviourTree;
using Typhon.Common;


namespace iGuardian.Wrappers
{
    internal class CreateSpellBehavior : Composite
    {
        private string SpellName { get; set; }
        private Func<RoutineContext, bool> Condition { get; set; }

        internal CreateSpellBehavior(string spellName, Func<RoutineContext, bool> condition = null)
        {
            SpellName = spellName;
            Condition = condition;
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
                            if (spell.IsReady)
                            {

                                SpellManager.Cast(spell);
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
