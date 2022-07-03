namespace BCPG9 {
    interface IGameModule {
        public void Initialize(BCPG9GameSettings gameData, BCPG9_FourWord gameManager);
        public void ResetModule();
    }
}