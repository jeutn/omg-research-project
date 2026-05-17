using UnityEngine;

public enum GamePhase
{
    Setup,
    RoundOpen,
    MarketOpen,
    Planning,
    MarketClose,
    Production,
    RoundEnd,
    GameEnd
}

public enum PlayerTurn
{
    player1,
    player2
}