using System.Collections;
using System.Collections.Generic;

public static class PointCounter
{
    public static int Points {  get; private set; }

    public static void AddPoints(int points)
    {
        Points += points;
    }

    public static void RemovePoints(int removedPoints)
    {
        Points -= removedPoints;
    }
}
