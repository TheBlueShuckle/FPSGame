using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Timer
{
    float maxSeconds;
    float currentTime = 0;

    public Timer(float maxSeconds)
    {
        this.maxSeconds = maxSeconds;
    }

    public void Update(float deltaTime)
    {
        currentTime += deltaTime;
    }

    public bool IsDone()
    {
        return currentTime >= maxSeconds;
    }

    public float GetTimeLeft()
    {
        return maxSeconds - currentTime;
    }
}
