using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoreScene {
    static string currSceneName = "Alpha1";
    static int currSceneIndex = 0;

    public static string CurrSceneName
    {
        get
        {
            return currSceneName;
        }

        set
        {
            currSceneName = value;
        }
    }

    public static int CurrSceneIndex
    {
        get
        {
            return currSceneIndex;
        }

        set
        {
            currSceneIndex = value;
        }
    }
}
