using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace g200
{
    public class Utility
    {
        public static string GetDifficultyText(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Normal:
                    return "����";

                case Difficulty.Hard:
                    return "�����";

                case Difficulty.VeryHard:
                    return "�ſ� �����";
            }

            return "����";
        }
    }
}
