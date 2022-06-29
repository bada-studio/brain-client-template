/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author You sungHyun <1seconds@biscuitlabs.io>
* @created 2022/06/29
* @desc SceneSwitcher Template
*/
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