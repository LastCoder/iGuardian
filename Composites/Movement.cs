using Buddy.Gw2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Typhon.BehaviourTree;
using Typhon.Common;
using Typhon.CommonBot;
using Typhon.Navigation;
using Action = Typhon.BehaviourTree.Action;
using Typhon.CommonBot.Navigation;
namespace iCombat.Wrappers
{


    internal static class Movement
    {

        internal static Composite MoveIntoRangeBehavior(float range)
        {
            return
                new PrioritySelector(
                //new Decorator(
                //    context => {
                //        RoutineContext ctx = (RoutineContext)context;
                //        Vector3 realPos = Gw2Math.VectorToLarge(ctx.CurrentPlayerPosition);
                //        if (ArchitectusSettings.Instance.Debug.IsMovementDebuggingActive)
                //            Logger.Write("Current player position: {0}", realPos);
                //        if (ctx.CurrentTargetPosition == Vector3.Zero)
                //            return false;
                //        Vector3 realTargetPos = Gw2Math.VectorToLarge(ctx.CurrentTargetPosition);
                //        if (ArchitectusSettings.Instance.Debug.IsMovementDebuggingActive)
                //            Logger.Write("Current target position: {0}", realTargetPos);
                //        float currentDistance = realPos.Distance(realTargetPos);
                //        if (ArchitectusSettings.Instance.Debug.IsMovementDebuggingActive)
                //            Logger.Write("Current distance: {0}", currentDistance);
                //        return !ctx.CurrentTargetIsVisible || range < currentDistance;
                //    },
                //    CommonBehaviors.CreateMoveTo(ret => BuddyGw.Me.CurrentTarget.Position, "Combat position")
                //),
                    new MoveToComposite(
                        ctx =>
                        {

                            return ctx.CurrentTargetPosition;
                        },
                        ctx =>
                        {
                            Vector3 realPos = Gw2Math.VectorToLarge(ctx.CurrentPlayerPosition);

                            if (ctx.CurrentTargetPosition == Vector3.Zero)
                                return false;
                            Vector3 realTargetPos = Gw2Math.VectorToLarge(ctx.CurrentTargetPosition);

                            float currentDistance = realPos.Distance(realTargetPos);

                            return ctx.CurrentTargetIsVisible && range > currentDistance;
                        },
                        "Combat position"
                    ),
                    new Decorator(
                        ctx =>
                        {
                            RoutineContext context = (RoutineContext)ctx;
                            if (context.CurrentTargetPosition == Vector3.Zero)
                                return false;
                            return !BuddyGw.Me.Agent.IsFacing(context.CurrentTargetPosition);
                        },
                        new Action(
                            ctx =>
                            {
                                RoutineContext context = (RoutineContext)ctx;
                                Navigator.Face(context.CurrentTargetPosition);
                            }
                        )
                    )
                );
        }
    }


}
