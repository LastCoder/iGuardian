using Buddy.Gw2;
using Buddy.Gw2.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Typhon.Common;
using Typhon.CommonBot;
using iGuardian.Methods;

// Combat context deterived from superreen (so, credits to that beast) 
namespace iGuardian.Wrappers
{
    internal class RoutineContext
    {

        internal void Update()
        {
            _skillNames = null;
            _currentPlayerPosition = null;
            _currentPlayerHealthPercentage = null;
            _currentTargetHealthPercentage = null;
            _currentTargetPosition = null;
            _isCasting = null;
            _skills = null;
            _currentTargetIsVisible = null;
            _currentWeapon = null;
            _buffs = null;
            _UnitPosition = null;
            _FriendlyPosition = null;
        }

        private HashSet<string> _skillNames;
        internal HashSet<string> SkillNames
        {
            get
            {

                return _skillNames ?? (_skillNames = new HashSet<string>(Skills.Keys));

            }
        }

        private Dictionary<string, Gw2Skill> _skills;
        internal Dictionary<string, Gw2Skill> Skills
        {
            get
            {

                if (_skills == null)
                {
                    _skills = SpellManager.Skills.Values.GroupBy(s => s.Name).ToDictionary(s => s.Key, s => s.First());
                }
                return _skills;

            }
        }

        private List<Gw2Skill> _buffs;
        internal List<Gw2Skill> Buffs
        {
            get
            {
                if (_buffs == null)
                    _buffs = BuddyGw.Me.Buffs.ToList();
                return _buffs;
            }
        }
     
        internal List<Gw2Skill> GetBuffs(Gw2Character player)
        {
            if (player != null)
            {
                return player.Buffs.ToList();
            }
            return null;
        }

        internal bool HasBuff(string name)
        {
            foreach (Gw2Skill skill in Buffs)
            {
                if (skill.Name == name)
                    return true;
            }
            return false;
        }

        internal Gw2Skill GetSpell(string name)
        {

            Gw2Skill s;
            if (!Skills.TryGetValue(name, out s))
                return null;
            return s;

        }

        internal float DistanceToTarget
        {
            get
            {

                return Calculations.Distance(CurrentPlayerPosition, CurrentTargetPosition);
            }
        }

        internal int CountVisibleEnemies
        {
            get
            {
                return UnitPositions.CachedPositions.Where(u => BuddyGw.IsVisible(u.Position)).Count();
            }
        }

        internal int CountViableEnemies(float radius)
        {
            return UnitPositions.CachedPositions.Where(u => BuddyGw.IsVisible(u.Position) && Vector3.Distance(CurrentPlayerPosition, u.Position) < radius).Count();
        }

        internal int CountViableFriendlies(float radius)
        {
            return FriendlyPositions.CachedPositions.Where(u => BuddyGw.IsVisible(u.Position) && Vector3.Distance(CurrentPlayerPosition, u.Position) < radius).Count();
        }

        private bool? _isCasting;
        internal bool IsCasting
        {
            get
            {

                return (_isCasting ?? (_isCasting = new bool?(BuddyGw.Me.CurrentlyCastingSkill != null))).Value;

            }
        }

        private double? _currentPlayerHealthPercentage;
        internal double CurrentPlayerHealthPercentage
        {
            get
            {

                return (_currentPlayerHealthPercentage ?? (_currentPlayerHealthPercentage = new double?(BuddyGw.Me.Health.Current / BuddyGw.Me.Health.Maximum))).Value;

            }
        }

        private double? _currentTargetHealthPercentage;
        internal double CurrentTargetHealthPercentage
        {
            get
            {

                if (BuddyGw.Me.CurrentTarget == null)
                {
                    return 0f;
                }
                return (_currentTargetHealthPercentage ?? (_currentTargetHealthPercentage = BuddyGw.Me.CurrentTarget.Health.Current / BuddyGw.Me.CurrentTarget.Health.Maximum)).Value;

            }
        }

        private Vector3? _currentPlayerPosition;
        internal Vector3 CurrentPlayerPosition
        {
            get
            {

                return (_currentPlayerPosition ?? (_currentPlayerPosition = new Vector3?(BuddyGw.Me.Position))).Value;

            }
        }

        private Vector3? _currentTargetPosition;
        internal Vector3 CurrentTargetPosition
        {
            get
            {
                if (BuddyGw.Me.CurrentTarget == null)
                {
                    return Vector3.Zero;
                }
                return (_currentTargetPosition ?? (_currentTargetPosition = new Vector3?(BuddyGw.Me.CurrentTarget.Position))).Value;

            }
        }

        private bool? _currentTargetIsVisible;
        internal bool CurrentTargetIsVisible
        {
            get
            {
                return (_currentTargetIsVisible ?? (_currentTargetIsVisible = new bool?(BuddyGw.IsVisible(CurrentTargetPosition)))).Value;
            }
        }

        private WeaponType? _currentWeapon;
        internal WeaponType CurrentWeapon
        {
            get
            {
                return (_currentWeapon ?? (_currentWeapon = BuddyGw.Me.Inventory.CurrentWeaponType)).Value;
            }
        }

        private PositionCache _UnitPosition;
        internal PositionCache UnitPositions
        {
            get
            {

                return _UnitPosition ?? (_UnitPosition = new PositionCache(BuddyGw.Objects.GetObjectsOfType<Gw2Character>().Where(u => BuddyGw.Objects.IsValid(u) && !u.IsFriendly && (u.IsIndifferent || u.IsInCombat || u.IsHostile) && u.IsAlive).Select(u => new PositionCache.CachedPosition { Position = u.Position, Radius = 3f })));

            }
        }

        private PositionCache _FriendlyPosition;
        internal PositionCache FriendlyPositions
        {
            get
            {
                return _FriendlyPosition ?? (_FriendlyPosition = new PositionCache(BuddyGw.Objects.GetObjectsOfType<Gw2Character>().Where(u => BuddyGw.Objects.IsValid(u) && u.IsFriendly && u.IsAlive && u.IsPlayer).Select(u => new PositionCache.CachedPosition { Position = u.Position, Radius = 3f })));
            }
        }
    }
}
