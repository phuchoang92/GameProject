using Game.Control;
using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneLoad : MonoBehaviour
{
    [SerializeField] int sceneToLoad=0;
    private void OnEnable()
    {
        StartCoroutine(Transition());
    }
    private IEnumerator Transition()
    {
        DontDestroyOnLoad(gameObject);

        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        savingWrapper.Save();

        Destroy(gameObject);
    }
}
