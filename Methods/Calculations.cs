using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buddy.Gw2;
using Buddy.Gw2.Objects;
using DisciplineBuddy.Methods;
using Typhon.Common;

namespace iCombat.iGuardian.Methods
{
    internal class Calculations
    {

        public static float Distance(Vector3 a, Vector3 b)
        {
            a = Gw2Math.VectorToLarge(a);
            b = Gw2Math.VectorToLarge(b);
            if (a == Vector3.Zero || b == Vector3.Zero)
            {
                Logger.WriteVerbose("Position a: {0}", a);
                Logger.WriteVerbose("Position b: {1}", b);
            }
            return a.Distance(b);
        }

        public static double CalculateWeight(Gw2Character character) 
        {
            double weight = 1;
            double healthpct = character.Health.Current / character.Health.Maximum;
            if (BuddyGw.Me.IsFightingForLife)
            {
                weight -= healthpct;
            }
            else
            {
                weight += healthpct;
            }

            weight -= character.Distance;
            weight -= character.IsVisible ? 0 : 1;
            weight *= character.IsPlayer ? 5 : 1;
            weight *= character.InCombat ? 5 : 1;
            return weight;
        }
    }
}
