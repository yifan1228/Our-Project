using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // 用于协程

public class AdditiveSceneLoader : MonoBehaviour
{
    void Start()
    {
        //TODO: 添加UI场景

        int i = 0;
        while (true)
        {
            i++;
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); // 确保场景路径有效
            if (string.IsNullOrEmpty(scenePath))
            {
                Debug.LogError("Scene path is empty or invalid for index " + i + " .");
                break; // 如果路径无效，退出循环
            }
            StartCoroutine(LoadAdditiveSceneAsync(scenePath));
        }
    }

    IEnumerator LoadAdditiveSceneAsync(string sceneName)
    {
        if (SceneUtility.GetBuildIndexByScenePath(sceneName) == -1)
        {
            Debug.LogError("Scene '" + sceneName + "' is not in Build Settings!");
            yield break;
        }

        // 开始异步附加加载
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // 等待加载完成
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading " + sceneName + " progress: " + asyncLoad.progress * 100 + "%");
            yield return null; // 等待下一帧
        }

        Debug.Log("Scene '" + sceneName + "' loaded additively.");

        // 可选：将新加载的场景设置为活动场景，以便在Hierarchy中更容易管理
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    // 卸载一个附加加载的场景
    public void UnloadAdditiveScene(string sceneName)
    {
        // 检查场景是否已加载
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            Debug.Log("Scene '" + sceneName + "' unloaded.");
        }
        else
        {
            Debug.LogWarning("Scene '" + sceneName + "' is not currently loaded.");
        }
    }
}
