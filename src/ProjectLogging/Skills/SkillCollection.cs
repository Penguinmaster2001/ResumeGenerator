
using ProjectLogging.Records;



namespace ProjectLogging.Skills;



public class SkillCollection
{
    public Dictionary<string, HashSet<string>> CategorySkills = new();



    public void AddSkills<T>(List<T> records) where T : IRecord
    {
        foreach (IRecord record in records)
        {
            foreach (Skill skill in record.Skills)
            {
                if (!CategorySkills.ContainsKey(skill.Category))
                {
                    CategorySkills.Add(skill.Category, new());
                }

                CategorySkills[skill.Category].Add(skill.Name);
            }
        }
    }
}
