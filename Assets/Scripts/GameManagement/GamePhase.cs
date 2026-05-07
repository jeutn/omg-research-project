using UnityEngine;

public enum GamePhase
{
    Setup,
    MarketOpen,
    Planning,
    MarketClose,
    Production,
    Cleanup,
    GameEnd
}

public enum PlayerTurn
{
    player1,
    player2
}