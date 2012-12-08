using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buddy.Gw2;
using Buddy.Gw2.Objects;
using Typhon.Common;

namespace iGuardian.Methods
{
    internal class Calculations
    {

        public static float Distance(Vector3 a, Vector3 b)
        {
            float distance = 0;
            if (b == Vector3.Zero)
            {
                distance = float.MinValue;
                Logger.WriteVerbose("Position a: {0}", a);
                Logger.WriteVerbose("Position b: {0}", b);
            }
            else
            {
                a = Gw2Math.VectorToLarge(a);
                b = Gw2Math.VectorToLarge(b);
                distance = a.Distance(b);
            }
            return distance;
        }

        public static double CalculateEnemyWeight(Gw2Character character) 
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

        public static double CalculateFriendlyWeight(Gw2Character character)
        {
            double weight = 1;
            double healthpct = character.Health.Current / character.Health.Maximum;
            if (BuddyGw.Me.IsFightingForLife)
            {
                weight += healthpct;
            }

            weight += CharactersAround(character) * 5;
            weight *= character.InCombat ? 5 : 1;
            return weight;
        }

        public static int CharactersAround(Gw2Character character)
        {
            return Game.GetSurrounding(character.Position, 10).Count();
        }
           
      
    }
}
