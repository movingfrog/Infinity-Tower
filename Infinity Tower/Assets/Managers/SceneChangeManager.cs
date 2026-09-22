using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance { get; private set; }

    [SerializeField]
    private GameObject loadingCanvas;

    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private float minLoadTime = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SceneChange<T>(T sceneValue)
    {
        StartCoroutine(SceneChangeCoroutine(sceneValue));
    }

    private IEnumerator SceneChangeCoroutine<T>(T sceneValue)
    {
        loadingCanvas.SetActive(true);
        progressBar.fillAmount = 0;

        float elapsedTime = 0f;
        AsyncOperation operation;
        if (sceneValue is string sceneName)
        {
            operation = SceneManager.LoadSceneAsync(sceneName);
        }
        else if (sceneValue is int sceneIndex)
        {
            operation = SceneManager.LoadSceneAsync(sceneIndex);
        }
        else
        {
            loadingCanvas.SetActive(false);
            Debug.LogError("잘못된 방식의 매게변수");
            yield break;
        }

        if (operation != null)
        {
            operation.allowSceneActivation = false;
            while (operation.progress < .9f || elapsedTime < minLoadTime)
            {
                elapsedTime += Time.deltaTime;

                float realProgress = Mathf.Clamp01(operation.progress / .9f);
                float timeProgress = Mathf.Clamp01(elapsedTime / minLoadTime);
                progressBar.fillAmount = Mathf.Min(realProgress, timeProgress);

                Debug.Log($"progress: {operation.progress}"); // 확인용
                yield return null;
            }

            progressBar.fillAmount = 1;
            yield return new WaitForSeconds(.1f);
            loadingCanvas.SetActive(false);
            operation.allowSceneActivation = true;
        }
    }
}
