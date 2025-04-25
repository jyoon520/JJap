using UnityEngine;

[CreateAssetMenu(fileName ="Monster", menuName = "Monster/Create new Monster")]
public class MonsterBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;

    [SerializeField] MonsterType type1;
    [SerializeField] MonsterType type2;

    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }
    public MonsterType Type1
    {
        get { return type1; }
    }

    public MonsterType Type2
    {
        get { return type2; }
    }
    public int MaxHp
    {
        get { return maxHp; }
    }

    public int Attack
    {
        get { return attack; }
    }
    public int Defense
    {
        get { return defense; }
    }


    public enum MonsterType
    {
        None,
        Normal,
        Flying,
    }
}

// 사용하기에 능력 부족