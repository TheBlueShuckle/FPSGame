using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public static class PointCounter
{
    public static int Points {  get; set; }

    public static void ResetPoints()
    {
        Points = 0;
    }

    public static void AddPoints(int addedPoints)
    {
        Points += addedPoints;
    }

    public static void RemovePoints(int removedPoints)
    {
        Points -= removedPoints;
    }
}
