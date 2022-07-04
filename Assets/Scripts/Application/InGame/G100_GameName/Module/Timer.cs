using UnityEngine;

namespace BCPG9 {
    /*
        Timer, has no update behaviour. Control by BCPG9_FourWord.cs (GameManager)
    */
    public class Timer : MonoBehaviour, IGameModule {
        public float time { get; private set; }
        
        #region Global Settings Clone
        private float limitedTime;
        #endregion

        public int remainTimeInt => Mathf.CeilToInt(limitedTime - time);
        public float remainTime => limitedTime - time;
        public bool isTimeExpired => remainTime <= 0;

        public void UpdateTimer() {
            time += Time.deltaTime;
        }

        public void Initialize(BCPG9GameSettings gameData, BCPG9_FourWord gameManager) {
            limitedTime = gameData.limitedTime;
        }

        public void ResetModule() {
            time = 0;
        }
    }
}