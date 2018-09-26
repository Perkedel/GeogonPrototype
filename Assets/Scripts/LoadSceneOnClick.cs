using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//https://unity3d.com/learn/tutorials/topics/user-interface-ui/creating-main-menu
//Tutorial Main menu from Unity
//Lazy to watch! long video!!!

public class LoadSceneOnClick : MonoBehaviour
{

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}