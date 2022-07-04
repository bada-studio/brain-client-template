namespace BCPG9 {
    public enum BCPG9GameEventType {
        // State
        Reset,
        NewQuiz,
        Correct,
        Incorrect,
        End,

        // Single Event
        ResetInput,
        Input,
        HintOpen,
        Pass,
        CloseEnd,
        Pause,
        Resume
    }
}