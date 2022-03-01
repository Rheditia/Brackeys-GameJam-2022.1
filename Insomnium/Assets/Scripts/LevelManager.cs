using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float sceneLoadDelay = 1f;
    Animator sceneTransition;

    private void Awake()
    {
        sceneTransition = GetComponentInChildren<Animator>();
    }

    public void LoadGame()
    {
        StartCoroutine(LoadLevel(1));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void LoadGameOver()
    {
        StartCoroutine(LoadLevel(2));
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        sceneTransition.SetTrigger("EndLevel");
        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(sceneIndex);
    }
}
