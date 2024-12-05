using System.Collections.Generic;

public static class RaceResults
{
    public static List<PlayerResult> results = new List<PlayerResult>();

    public static void Clear()
    {
        results.Clear();
    }

    public static void SortResults()
    {
        results.Sort((x, y) => x.result.CompareTo(y.result));

        for (int i = 0; i < results.Count; i++)
        {
            results[i].position = i + 1; 
        }
    }
}
