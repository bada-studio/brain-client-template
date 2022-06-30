interface IUICallbacks {
    void OnAnswer(bool isCorrect, int score, float gameTime, int combo);
    void OnReset();
    void OnInput();
}