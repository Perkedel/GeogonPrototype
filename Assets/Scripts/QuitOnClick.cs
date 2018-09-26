using UnityEngine;
using System.Collections;

//https://unity3d.com/learn/tutorials/topics/user-interface-ui/creating-main-menu
//Tutorial Main menu from Unity
//Lazy to watch! long video!!!

public class QuitOnClick : MonoBehaviour
{

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}