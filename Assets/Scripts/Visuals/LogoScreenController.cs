using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScreenController : MonoBehaviour
{
    private const string MENU_SCENE = "Menu";

    void Start()
    {
        StartCoroutine(LoadMenu());
    }

    private IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(MENU_SCENE);
    }
}
