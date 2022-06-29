/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author You sungHyun <1seconds@biscuitlabs.io>
* @created 2022/06/29
* @desc SceneController Template
*/
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public enum SceneType {
    Intro,
    G100_GameName,
}

public interface SceneController {
    SceneType sceneType { get; }
}
