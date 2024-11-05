using System;
using Microsoft.Xna.Framework;

namespace TankyProjekt;

public class Timer
{
    private Action _action;
    private float _timer;
    private float _timeNeeded;

    public Timer(float timeNeeded, Action action)
    {
        _timeNeeded = timeNeeded;
        _action = action;
        _timer = 0f;
    }

    public void Update(GameTime gameTime)
    {
        _timer += (float) gameTime.ElapsedGameTime.TotalSeconds;
        
        if (_timer > _timeNeeded)
        {
            _action.Invoke();
        }
    }
}