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
