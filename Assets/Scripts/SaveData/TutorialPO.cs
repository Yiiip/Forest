using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialPO : IPersistentObject
{
    public HashSet<string> tutorialProgress;

    public void Init()
    {
        if (tutorialProgress == null)
            tutorialProgress = new HashSet<string>();
    }

    public bool IsFinished(string key)
    {
        return tutorialProgress != null && tutorialProgress.Contains(key);
    }

    public void Finish(string key)
    {
        tutorialProgress?.Add(key);
    }
}

public static class TutorialConst
{
    public const string Key_FirstOpen = "Key_FirstOpen";
}
