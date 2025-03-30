using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuizScore
{
    public int correctCount;
    public int incorrectCount;

    public QuizScore(int correct, int incorrect)
    {
        correctCount = correct;
        incorrectCount = incorrect;
    }
}
