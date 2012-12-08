using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Typhon.XmlEngine;
using Buddy.Gw2;
using System.ComponentModel;
using Typhon.Common.Xml;
using System.Configuration;
using iGuardian.GUI;

/**
 * <summary>
 * As the name suggests, settings for the combat routine
 * </summary>
 **/
namespace iGuardian.Settings
{
    [XmlElement("iGuardianSettings")]
    public class iSettings : XmlSettings
    {
        private static iSettings _Instance;

        public iSettings() :
            base(Path.Combine(Path.Combine(SettingsDirectory, iGuardian.ProjectName), string.Format("{0}Settings.xml", iGuardian.ProjectName)))
        {
        }

        public static iSettings Instance
        {
            get { return _Instance ?? (_Instance = new iSettings()); }
        }

        #region General Settings
        [XmlElement("DisableMovement")]
        [DisplayName("Disable movement behaviors")]
        [Description("")]
        [DefaultValue(false)]
        [Setting]
        [Category("General")]
        public bool MovementDisabled { get; set; }

        [XmlElement("MinimumRange")]
        [DisplayName("Mininum range to pull an enemy")]
        [Description("Mininum range to consider an enemy as ranged/kiting")]
        [DefaultValue(600f)]
        [Setting]
        [Limit(0, 1500)]
        [Category("General")]
        public float MininumRange { get; set; }

        #endregion

        #region Sceptor Settings

        [XmlElement("MaximumSceptorPullRange")]
        [DisplayName("Maximum pull range")]
        [Description("Maximum distance to pull an enemy.")]
        [DefaultValue(0.4f)]
        [Setting]
        [Limit(0, 1)]
        [Category("Sceptor")]
        public float MaximumSceptorPullRange { get; set; }

        [XmlElement("SceptorPull")]
        [DisplayName("Use Sceptor to pull enemies")]
        [Description("Use Sceptor to pull enemies")]
        [DefaultValue(false)]
        [Setting]
        [Category("Sceptor")]
        public bool SceptorPull { get; set; }

        #endregion

        #region Greatsword Settings
        [XmlElement("WhirlingRange")]
        [DisplayName("Minimum range to consider a group for AOE attacks")]
        [Description("Minimum range to consider a group for AOE attacks")]
        [DefaultValue(20)]
        [Setting]
        [Limit(0, 600)]
        [Category("Greatsword")]
        public float WhirlingBladeAOERange { get; set; }

        [XmlElement("WhirlingCount")]
        [DisplayName("Minimum amount of enemies to consider a group for AOE attacks")]
        [Description("Minimum amount of enemies to consider a group for AOE attacks")]
        [DefaultValue(1)]
        [Setting]
        [Limit(0, 20)]
        [Category("Greatsword")]
        public float WhirlingBladeAOECount { get; set; }

        [XmlElement("WhirlingPercent")]
        [DisplayName("Minimum percentage of HP to consider a single target for whirling blade.")]
        [Description("Minimum percentage of HP to consider a single target for whirling blade.")]
        [DefaultValue(60f)]
        [Setting]
        [Limit(0, 100)]
        [Category("Greatsword")]
        public float WhirlingBladeSinglePercent { get; set; }
        #endregion

        #region Shield settings
        [XmlElement("ShieldOfJudgmentAOE")]
        [DisplayName("Number of AOE to use shield of judgement on.")]
        [Description("Number of AOE to use shield of judgement on.")]
        [DefaultValue(2)]
        [Setting]
        [Limit(1, 10)]
        [Category("Sceptor")]
        public float ShieldOfJudgmentAOE { get; set; }

        [XmlElement("ShieldOfJudgmentPercentage")]
        [DisplayName("Percentage of HP to use shield of judgement on.")]
        [Description("Percentage of HP to use shield of judgement on.")]
        [DefaultValue(40)]
        [Setting]
        [Limit(1, 100)]
        [Category("Sceptor")]
        public float ShieldOfJudgmentPercentage { get; set; }


        #endregion

        #region Healing settings

        [XmlElement("SignetOfResolvePercentage")]
        [DisplayName("Mininum HP percentage to cast this healing spell.")]
        [Description("Mininum HP percentage to cast this healing spell.")]
        [DefaultValue(40)]
        [Setting]
        [Limit(0, 100)]
        [Category("Healing")]
        public float SignetOfResolvePercentage { get; set; }

        [XmlElement("ShelterPercentage")]
        [DisplayName("Mininum HP percentage to cast this healing spell.")]
        [Description("Mininum HP percentage to cast this healing spell.")]
        [DefaultValue(40)]
        [Setting]
        [Limit(0, 100)]
        [Category("Healing")]
        public float ShelterPercentage { get; set; }

        [XmlElement("HealingBreezePercentage")]
        [DisplayName("Mininum HP percentage to cast this healing spell.")]
        [Description("Mininum HP percentage to cast this healing spell.")]
        [DefaultValue(40)]
        [Setting]
        [Limit(0, 100)]
        [Category("Healing")]
        public float HealingBreezePercentage { get; set; }

        [XmlElement("HealOnly")]
        [DisplayName("Use Healing Only")]
        [Description("Use Healing Only")]
        [DefaultValue(false)]
        [Setting]
        [Category("Healing")]
        public bool HealOnly { get; set; }

        [XmlElement("HealAndFight")]
        [DisplayName("Use Healing and Fight")]
        [Description("Use Healing and Fight")]
        [DefaultValue(false)]
        [Setting]
        [Category("Healing")]
        public bool HealAndFight { get; set; }

        #endregion

        #region Utility settings
        [XmlElement("HallowedGround")]
        [DisplayName("Hallowed Ground to break stun")]
        [Description("Use Hallowed Ground to break stun.")]
        [DefaultValue(true)]
        [Setting]
        [Category("Utilities")]
        public bool HallowedGround { get; set; }

        [XmlElement("PurgingFlames")]
        [DisplayName("Purging Flames to remove harmful effects")]
        [Description("Use Purging Flames to remove all harmful effects on our character.")]
        [DefaultValue(true)]
        [Setting]
        [Category("Utilities")]
        public bool PurgingFlames { get; set; }

        [XmlElement("PurgingFlamesCount")]
        [DisplayName("Number of harmful effects to activate")]
        [Description("Number of harmful effects to activiate Purging Flames.")]
        [DefaultValue(3)]
        [Setting]
        [Limit(1, 10)]
        [Category("Utilities")]
        public int PurgingFlamesCount { get; set; }

        [XmlElement("WallOfReflection")]
        [DisplayName("Wall of Reflection to protect from rangers")]
        [Description("Use Wall of Reflection to protect from ranged enemies.")]
        [DefaultValue(true)]
        [Setting]
        [Category("Utilities")]
        public bool WallOfReflection { get; set; }

        [XmlElement("JudgeInterventionCount")]
        [DisplayName("Number of enemies to consider using Judge's intervention on.")]
        [Description("Number of enemies to consider using Judge's intervention on.")]
        [DefaultValue(3)]
        [Setting]
        [Limit(1, 50)]
        [Category("Utilities")]
        public int JudgeInterventionCount { get; set; }
        #endregion

    }
}
