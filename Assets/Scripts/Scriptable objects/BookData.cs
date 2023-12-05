using UnityEngine;

[CreateAssetMenu(fileName = "BookData", menuName = "ScriptableObjects/Items/BookData", order = 2)]
public class BookData : ItemData
{
    [SerializeField] private int _timeToRead;
    [SerializeField] private GlobalRepository.SkillType _skillType;

    public int TimeToRead => _timeToRead;
    public GlobalRepository.SkillType SkillType => _skillType;

    public override string GetAdditionalInfo()
    {
        string info = "";

        string hrsToRead = (_timeToRead / 60).ToString();
        string minsToRead = (_timeToRead - _timeToRead / 60 * 60).ToString();

        while (hrsToRead.Length <= 1) 
        { 
            hrsToRead = "0" + hrsToRead;
        }

        while (minsToRead.Length <= 1)
        {
            minsToRead = "0" + minsToRead;
        }

        info += "Skill: " + _skillType.ToString() + "\n";

        info += string.Format("<sprite name=\"TimeIcon\"> {0}:{1}", hrsToRead,minsToRead);

        info += "  <sprite name=\"WeightIcon\">" + Weight + "kg";

        return info;
    }

    public override void Use()
    {
        foreach (Item item in this.ItemsToAddAfterUse)
        {
            GlobalRepository.Inventory.AddItem(item, false);
        }

        GameObject.FindObjectOfType<ReadScreenShower>(true).OpenScreen(this);
    }
}
