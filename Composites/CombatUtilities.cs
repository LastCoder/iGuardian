using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGuardian.Wrappers;
using Typhon.BehaviourTree;

namespace iGuardian.Composites
{
    internal static class CombatUtilities
    {
        internal static Composite CreateWaitForCast()
        {
            return new Decorator(
                ctx =>
                {
                    RoutineContext context = (RoutineContext)ctx;
                    return context.IsCasting;
                },
                new Typhon.BehaviourTree.Action(ctx => RunStatus.Success)
            );
        }
    }
}
