namespace GameState
{
    public interface IGameState
    {
        bool Setup();
        void Run();
        bool Teardown();
    }
}