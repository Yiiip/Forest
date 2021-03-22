using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorMenuItems
{
    [MenuItem("Scene/Start")]
    static void EditStartScene()
    {
        OpenScene("Assets/Scenes/Start.unity");
    }

    // [MenuItem("Scene/Loading")]
    // static void EditLoadingScene()
    // {
    //     OpenScene("Assets/Scenes/Loading.unity");
    // }

    [MenuItem("Scene/Forest")]
    static void EditForestScene()
    {
        OpenScene("Assets/Scenes/Forest.unity");
    }

    [MenuItem("Scene/TinyGame_Coding_Scene")]
    static void EditTinyGame1Scene()
    {
        OpenScene("Assets/Scenes/TinyGame_Coding_Scene.unity");
    }

    [MenuItem("Scene/TinyGame_Stocks_Scene")]
    static void EditTinyGame2Scene()
    {
        OpenScene("Assets/Scenes/TinyGame_Stocks_Scene.unity");
    }

    [MenuItem("Scene/TinyGames")]
    static void EditTinyGamesScene()
    {
        OpenScene("Assets/Scenes/TinyGames.unity");
    }

    static void OpenScene(string scene)
    {
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scene);
    }
}
