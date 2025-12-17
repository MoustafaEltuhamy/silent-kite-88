using System;
using AGAPI.Gameplay;

public class DefaultScoreSystem : IScoreSystem
{
    private int _score = 0;
    private readonly ScoreConfig _scoreConfig;

    public DefaultScoreSystem(ScoreConfig scoreConfig)
    {
        _scoreConfig = scoreConfig;
    }

    //----------- IScoreSystem Implementation -----------//
    public int Score => _score;

    public void ApplyMatchResult(bool isMatch)
    {
        if (isMatch)
            Add(_scoreConfig.MatchPoints);
        else
            Subtract(_scoreConfig.MismatchPenalty);
    }

    public void ResetScore(int initialScore = 0)
    {
        _score = initialScore;
    }

    //----------- Private Implementation -----------//
    private void Add(int amount)
    {
        if (amount <= 0) return;
        _score += amount;
    }

    private void Subtract(int amount)
    {
        if (amount <= 0) return;
        _score = Math.Max(0, _score - amount);
    }
}
