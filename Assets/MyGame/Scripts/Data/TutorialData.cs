using KingCat.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialData : MonoBehaviour
{
    public List<TutorialInfo> tutorials = new();

    public TutorialInfo GetTutorialByIndex(int index)
    {
        var info = tutorials.Find(item => item.index == index);
        //if (info == null || info.stepInfos == null) return new List<UIBaseButton>();
        if (info == null) return new TutorialInfo();

        return info;
    }

    public StepInfo GetStepByIndex(int tutorialIndex, int stepIndex)
    {
        var tutorial = GetTutorialByIndex(tutorialIndex);
        return tutorial.stepInfos.Find(item => item.index == stepIndex);
    }

}

[System.Serializable]
public class TutorialInfo
{
    public int index;
    public List<StepInfo> stepInfos;
    public bool isDone;

    public List<UIBaseButton> steps
    {
        get
        {
            var buttons = new List<UIBaseButton>();
            foreach (var stepInfo in stepInfos)
            {
                if (stepInfo != null)
                {
                    var btn = stepInfo.stepObject.GetComponent<UIBaseButton>();
                    //var btn = obj.GetComponent<UIBaseButton>();
                    if (btn != null) buttons.Add(btn);
                }
            }
            return buttons;
        }
    }
}

[System.Serializable]
public class StepInfo
{
    public int index;
    public GameObject stepObject;
    public GameObject UIStepCanActive;
    public bool isDone;
}
