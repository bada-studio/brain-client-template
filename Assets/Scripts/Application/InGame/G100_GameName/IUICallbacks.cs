namespace BCPG9 {
    interface IUICallbacks {
        void OnAnswer(bool isCorrect, int score, float gameTime, int comboCount);
        void OnResetGame(float comboCheckTime);
        void OnUpdateAnswer(BCPG9Rule answer, float gameTime);
        void OnPause();
        void OnResume();
    }
}