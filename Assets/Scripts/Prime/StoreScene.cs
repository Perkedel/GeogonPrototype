using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoreScene {
    static string currSceneName = "Alpha1";
    static int currSceneIndex = 0;
    static string[] altNextSceneNames = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
    static int[] altNextSceneIndexes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    const string DevExitScene = "DeveloperExit";
    static string NextLevelNamer;

    public static void LoadSettings()
    {

    }

    public static void SaveSettings()
    {

    }

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

    public static string[] AltNextSceneNames
    {
        get
        {
            return altNextSceneNames;
        }

        set
        {
            altNextSceneNames = value;
        }
    }

    public static int[] AltNextSceneIndexes
    {
        get
        {
            return altNextSceneIndexes;
        }

        set
        {
            altNextSceneIndexes = value;
        }
    }

    public static string DevExitScene1
    {
        get
        {
            return DevExitScene;
        }
    }

    public static string NextLevelNaming
    {
        get
        {
            return NextLevelNamer;
        }
        set
        {
            NextLevelNamer = value;
        }
    }
}
