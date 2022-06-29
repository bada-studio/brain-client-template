using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    IEnumerator Start() {
        yield return null;
        var scene = SceneControllerBase.GetNextScene();
        SceneManager.LoadScene(scene.ToString());
    }
}