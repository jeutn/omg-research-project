using System.Collections.Generic;

public static class GameUtils
{
    public static void FisherYates<T>(IList<T> collection)
    {
        for (int i = collection.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i+1);
            var temp = collection[i];
            collection[i] = collection[j];
            collection[j]= temp;
        }
    }
}