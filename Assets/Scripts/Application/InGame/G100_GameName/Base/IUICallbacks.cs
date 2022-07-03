namespace BCPG9 {
    interface IUIEventCallback {
        public void OnEventCall(BCPG9GameEventType eventType, BCPG9GameSettings gameData, BCPG9PlayData playData, string input = null);
    }

    interface IUIUpdateCallback {
        public void OnUpdateCall(BCPG9PlayData playData);
    }
}