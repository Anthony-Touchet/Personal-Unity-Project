using System;
using UnityEngine;

namespace Stats
{
    public enum AttributeType
    {
        STRENGTH,
        DEXTERITY,
        CONSTITUTION,
        INTELLIGENCE,
        WISDOM,
        CHARISMA
    }

    public enum Skill
    {
        ACROBATICS,
        ANIMAL_HANDLING,
        ARCANA,
        ATHLETICS,
        DECEPTION,
        HISTORY,
        INSIGHT,
        INTIMIDATION,
        INVESTIGATION,
        MEDICINE,
        NATURE,
        PERCEPTION,
        PREFORMANCE,
        PERSUASION,
        RELIGION,
        SLIGHT_OF_HAND,
        STEALTH,
        SURVIVAL,
    }

    [Serializable]
    public class Attribute
    {
        [SerializeField]
        private int m_Magnitude;
        [SerializeField]
        private AttributeType m_AttributeType;

        public int Magnitude { get { return m_Magnitude; } }
        public AttributeType AttributeType { get { return m_AttributeType; } }
        public int AttributeBonus { get { return (Magnitude - 10) / 2; } }

        public Attribute(AttributeType pAttributeType, int pMagnitude)
        {
            m_Magnitude = pMagnitude;
            m_AttributeType = pAttributeType;
        }
    }

    public abstract class Ability
    {
        public float magnitude;

        public virtual object Execute()
        {
            return null;
        }
    }
}
