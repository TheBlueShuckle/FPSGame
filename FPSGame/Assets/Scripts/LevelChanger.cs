using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;

    private int levelToLoad;

    private void Update()
    {
        if (player.GetComponent<PlayerHealth>().Health <= 0)
        {
            FadeToLevel(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
