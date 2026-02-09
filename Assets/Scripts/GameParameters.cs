using UnityEngine;

public static class GameParameters
{
    public static string ACCUSATION_CORRECT_01 = "Comment ?? Tu m'as trouvé ?!";
    public static string ACCUSATION_CORRECT_02 = "Toi, ici ?! Comment as-tu fait ?!";
    public static string ACCUSATION_CORRECT_03 = "Nooon, pas toi !! Qu'est-ce que tu fais là ?!";
    public static string[] ACCUSATIONS_CORRECT = { ACCUSATION_CORRECT_01, ACCUSATION_CORRECT_02, ACCUSATION_CORRECT_03 };

    public static string ACCUSATION_INCORRECT_01 = "Cretino! Pour qui vous prenez vous ?!";
    public static string ACCUSATION_INCORRECT_02 = "Scusi? Je ne vous connais pas !";
    public static string ACCUSATION_INCORRECT_03 = "Pazza! Mais qu'est ce que vous faites ?!";
    public static string[] ACCUSATIONS_INCORRECT = { ACCUSATION_INCORRECT_01, ACCUSATION_INCORRECT_02, ACCUSATION_INCORRECT_03 };

    public static string VICTORY_SCENE_NAME = "EndVictoryScene";
    public static string DEFEAT_SCENE_NAME = "EndScene";


    public static float FAIL_ACCUSATION_PENALTY = 60.0f;
}
