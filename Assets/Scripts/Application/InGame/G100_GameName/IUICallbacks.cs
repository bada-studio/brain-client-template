namespace BCPG9 {
    public enum BCPG9GameEventType {
        Reset,
        ResetInput,
        NewQuiz,
        Input,
        HintOpen,
        Pass,
        Correct,
        Incorrect,
        CloseEnd,
        End,
        Pause,
        Resume
    }

    [System.Serializable]
    public class BCPG9GameData {
        public float limitedTime;
        public int standardScore;
        public float comboCheckTime;
        public int maxComboCount;
        public float comboMultiplier;
        public float passMultiplier;
    }

    public class BCPG9PlayData {
        public BCPG9Rule rule;
        public int score;
        public int comboCount;
        public float remainTime;
        public float remainTimeRatio;
    }

    interface IUIEventCallback {
        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameData gameData, BCPG9PlayData playData, string input = null);
    }

    interface IUIUpdateCallback {
        public void OnUpdateCall(BCPG9PlayData playData);
    }

    interface IGameModule {
        public void Initialize(BCPG9GameData gameData, BCPG9_FourWord gameManager);
        public void ResetModule();
    }
}