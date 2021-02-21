using System;

namespace Jlabarca.OneBigMob
{
    public interface IGameState
    {
        event Action<int> OnScoreChange;
        event Action<int> OnCountChange;

        int Score { get; set; }
        int Count { get; set; }
    }
}
