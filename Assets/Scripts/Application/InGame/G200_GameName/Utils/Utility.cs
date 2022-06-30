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
                    return "보통";

                case Difficulty.Hard:
                    return "어려움";

                case Difficulty.VeryHard:
                    return "매우 어려움";
            }

            return "보통";
        }
    }
}
