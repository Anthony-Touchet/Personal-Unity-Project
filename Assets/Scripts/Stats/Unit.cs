using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(menuName = "Unit")]
    public class Unit : ScriptableObject
    {
        [SerializeField]
        private List<Attribute> attributes;
        [SerializeField]
        private List<Skill> proficentcySkills;

        private int level = 1;

        public int ProficentcyBonus
        {
            get { return level/5 + 2; }
        }

        public Unit(int str, int dex, int con, int intell, int wis, int cha)
        {
            attributes = new List<Attribute>
            {
                new Attribute(AttributeType.STRENGTH, str),
                new Attribute(AttributeType.DEXTERITY, dex),
                new Attribute(AttributeType.CONSTITUTION, con),
                new Attribute(AttributeType.INTELLIGENCE, intell),
                new Attribute(AttributeType.WISDOM, wis),
                new Attribute(AttributeType.CHARISMA, cha)
            };
        }

        public Attribute ReturnSkill(AttributeType attributeType)
        {
            return attributes.FirstOrDefault(attribute => attribute.AttributeType == attributeType);
        }

        public bool IsProficentInSkill(Skill skill)
        {
            return proficentcySkills.Contains(skill);
        }

        public List<int> RollDice(uint range, uint quantity)
        {
            var rolls = new List<int>();
            var r  = new System.Random();

            for (var i = 0; i < quantity; i++)
            {
                rolls.Add(r.Next(1, (int)range + 1));
            }

            return rolls;
        }

        public List<int> RollDice(uint range, uint quantity, Attribute attribute)
        {
            var rolls = new List<int>();
            var r = new System.Random();

            for (var i = 0; i < quantity; i++)
            {
                var roll = r.Next(1, (int) range + 1);
                roll += attribute.AttributeBonus;
                rolls.Add(roll);
            }

            return rolls;
        }

        public List<int> RollDice(uint range, uint quantity, Attribute attribute, Skill skill)
        {
            var rolls = new List<int>();
            var r = new System.Random();

            for (var i = 0; i < quantity; i++)
            {
                var roll = r.Next(1, (int)range + 1);
                roll += attribute.AttributeBonus;
                if (IsProficentInSkill(skill))
                    roll += ProficentcyBonus;
                rolls.Add(roll);
            }

            return rolls;
        }
    }
}
