using System.Collections.Generic;
using UnityEngine;

public static class GameParameters
{
    public static int AIPlayerIndex;
    public static int TeamSize;
    public static List<int> TeamIndexes;

    public static void ClearTeamIndexes() {
        TeamIndexes = new List<int>();

        for (int i = 0; i < 25; i++) {
            TeamIndexes.Add(-1);
        }
    }
}
