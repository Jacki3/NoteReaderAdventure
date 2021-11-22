using UnityEngine;

public class CoreSkills : MonoBehaviour
{
    private static CoreSkills _i;

    public static CoreSkills i
    {
        get
        {
            return _i;
        }
    }

    private void Awake()
    {
        if (_i != null && _i != this)
            Destroy(this.gameObject);
        else
            _i = this;
    }

    public Skill[] skills;

    [System.Serializable]
    public class Skill
    {
        public string skillName;

        public string skillDescription;

        public PlayerSkills.SkillType skillType;

        public int skillPointsRequired;

        public bool requiresPriorSkill;

        public PlayerSkills.SkillType requiredSkill;
    }
}
