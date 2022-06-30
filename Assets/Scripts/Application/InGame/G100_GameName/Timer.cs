using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    public float time { get; private set; }

    public void ResetTimer() {
        time = 0;
    }

    private void Update() {
        time += Time.deltaTime;
    }
}