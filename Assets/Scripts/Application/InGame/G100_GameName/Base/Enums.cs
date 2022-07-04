namespace BCPG9 {
    /*
        Event Type
        State: Syncronous by animators fsm
        Single Event: Asyncronous call by input or sudden event.
    */
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