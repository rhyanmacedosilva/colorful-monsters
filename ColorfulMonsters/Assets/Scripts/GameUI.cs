using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    private AudioSource audioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
		{
            audioSource.Play();
            StartCoroutine(ChangeScene());
		}
    }

    IEnumerator ChangeScene()
	{
        yield return new WaitForSeconds(1);

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SceneManager.LoadScene("SampleScene");
        }
        else if (SceneManager.GetActiveScene().name == "GameOver")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
