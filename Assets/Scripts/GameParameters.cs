using UnityEngine;

public static class GameParameters
{
    public static string ACCUSATION_CORRECT_RANDOM_1 = "Comment ?? Tu m'as trouvé ?!";
    public static string ACCUSATION_CORRECT_RANDOM_2 = "Toi, ici ?! Comment as-tu fait ?!";
    public static string ACCUSATION_CORRECT_RANDOM_3 = "Nooon, pas toi !! Qu'est-ce que tu fais là ?!";
    public static string[] ACCUSATIONS_CORRECT = { ACCUSATION_CORRECT_RANDOM_1, ACCUSATION_CORRECT_RANDOM_2, ACCUSATION_CORRECT_RANDOM_3 };

    public static string ACCUSATION_INCORRECT_RANDOM_1 = "Cretino! Pour qui vous prenez vous ?!";
    public static string ACCUSATION_INCORRECT_RANDOM_2 = "Scusi? Je ne vous connais pas !";
    public static string ACCUSATION_INCORRECT_RANDOM_3 = "Pazza! Qu'est ce que vous faites ?!";
    public static string[] ACCUSATIONS_INCORRECT = { ACCUSATION_INCORRECT_RANDOM_1, ACCUSATION_INCORRECT_RANDOM_2, ACCUSATION_INCORRECT_RANDOM_3 };

    public static string VICTORY_SCENE_NAME = "EndVictoryScene";
    public static string DEFEAT_SCENE_NAME = "EndScene";


    public static float DEFAULT_TIME_FAIL_ACCUSATION = 60.0f;

    public static int MAXIMUM_MASK_INVENTORY = 3;
}
