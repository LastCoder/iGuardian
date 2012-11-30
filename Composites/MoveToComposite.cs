using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Typhon.BehaviourTree;
using Typhon.Common;
using Typhon.CommonBot.Navigation;
using Typhon.Navigation;

namespace iCombat.Wrappers
{
    internal class MoveToComposite : Composite
    {
        private Vector3 _CurrentPosition;
        private Func<RoutineContext, Vector3> _TargetPosition;
        private Func<RoutineContext, bool> _IsInRange;
        private string _DestinationName;
        private float _LastDistance = 0;

        internal MoveToComposite(Func<RoutineContext, Vector3> targetPosition, Func<RoutineContext, bool> isInRange, string destinationName)
        {
            _IsInRange = isInRange;
            _TargetPosition = targetPosition;
            _DestinationName = destinationName;
        }

        public override RunStatus Tick(object context)
        {

            RoutineContext ctx = (RoutineContext)context;
            ctx.Update();
            _CurrentPosition = _TargetPosition(ctx);
            if (_IsInRange(ctx))
            {

                Typhon.Navigation.Movement.StopMoving();
                return RunStatus.Failure;
            }
            else
            {
                if (_CurrentPosition != Vector3.Zero)
                {
                    MoveResult movement = Navigator.MoveTo(_CurrentPosition, _DestinationName);
                    if (movement == MoveResult.Failure)
                    {

                        _LastDistance = -1;
                        return RunStatus.Failure;
                    }
                    else if (movement == MoveResult.Done)
                    {

                        _LastDistance = -1;
                        return RunStatus.Failure;
                    }
                    else
                    {
                        float currentDistance = ctx.CurrentPlayerPosition.Distance(_CurrentPosition);
                        if (_LastDistance != -1 && currentDistance > _LastDistance)
                        {

                            _LastDistance = -1;
                            return RunStatus.Success;
                        }
                        else
                        {

                            _LastDistance = ctx.CurrentPlayerPosition.Distance(_CurrentPosition);
                            return RunStatus.Running;
                        }
                    }
                }
                else
                {

                    _LastDistance = -1;
                    return RunStatus.Failure;
                }
            }
        }

        protected override IEnumerable<RunStatus> Execute(object context)
        {

            RoutineContext ctx = (RoutineContext)context;
            _CurrentPosition = _TargetPosition(ctx);
            if (_IsInRange(ctx))
            {

                Typhon.Navigation.Movement.StopMoving();
                yield return RunStatus.Failure;
            }
            else
            {
                if (_CurrentPosition != Vector3.Zero)
                {
                    MoveResult movement = Navigator.MoveTo(_CurrentPosition, _DestinationName);
                    if (movement == MoveResult.Failure)
                    {
                        _LastDistance = -1;
                        yield return RunStatus.Failure;
                    }
                    else if (movement == MoveResult.Done)
                    {

                        _LastDistance = -1;
                        yield return RunStatus.Failure;
                    }
                    else
                    {
                        float currentDistance = ctx.CurrentPlayerPosition.Distance(_CurrentPosition);
                        if (_LastDistance != -1 && currentDistance > _LastDistance)
                        {

                            _LastDistance = -1;
                            yield return RunStatus.Success;
                        }
                        else
                        {

                            _LastDistance = ctx.CurrentPlayerPosition.Distance(_CurrentPosition);
                            yield return RunStatus.Running;
                        }
                    }
                }
                else
                {
                    _LastDistance = -1;
                    yield return RunStatus.Failure;
                }
            }
        }
    }
}
