using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TestItemSO", menuName = "SO/TestItemSO")]
public class TestItemSO : ScriptableObject
{
    public string SkillName;
    public float Skillvalue;
    public string SkillDescription;
    public Image SkillImage;
}
