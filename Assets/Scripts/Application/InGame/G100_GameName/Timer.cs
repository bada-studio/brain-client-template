using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BCPG9 {
    public class Timer : MonoBehaviour {
        [SerializeField] private float limitedTime;

        public float time { get; private set; }

        public void ResetTimer() {
            time = 0;
        }

        private void Update() {
            time += Time.deltaTime;
        }
    }
}