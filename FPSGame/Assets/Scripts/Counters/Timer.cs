using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Timer
{
    float duration;
    float currentTime = 0;

    public Timer(float duration)
    {
        this.duration = duration;
    }

    public void Update(float deltaTime)
    {
        currentTime += deltaTime;
    }

    public bool IsDone()
    {
        return currentTime >= duration;
    }

    public float GetTimeLeft()
    {
        return duration - currentTime;
    }
}
