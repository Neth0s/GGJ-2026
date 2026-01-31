using UnityEngine;

public static class GameParameters
{
    public static string ACCUSATION_CORRECT_RANDOM_1 = "Aranccini brotheli. Mario.";
    public static string ACCUSATION_CORRECT_RANDOM_2 = "Coco maltese ez pz lemon napoli.";
    public static string ACCUSATION_CORRECT_RANDOM_3 = "Pouletto dineto.";
    public static string[] ACCUSATIONS_CORRECT = { ACCUSATION_CORRECT_RANDOM_1, ACCUSATION_CORRECT_RANDOM_2, ACCUSATION_CORRECT_RANDOM_3 };

    public static string ACCUSATION_INCORRECT_RANDOM_1 = "Ma que Pasa ?? (NON)";
    public static string ACCUSATION_INCORRECT_RANDOM_2 = "Es stupido ma què no es hables spanish (NON)";
    public static string ACCUSATION_INCORRECT_RANDOM_3 = "MONETTO NO ES BUENO";
    public static string[] ACCUSATIONS_INCORRECT = { ACCUSATION_INCORRECT_RANDOM_1, ACCUSATION_INCORRECT_RANDOM_2, ACCUSATION_INCORRECT_RANDOM_3 };

    public static string VICTORY_SCENE_NAME = "EndVictoryScene";
    public static string DEFEAT_SCENE_NAME = "EndScene";
}
