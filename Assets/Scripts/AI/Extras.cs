using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ConqsterAI
{
    public class Extras
    {

        private int ClampAngle(int value)
        {

            int[] angles = new int[9] { 0, 45, 90, 135, 180, 225, 270, 315, 360 };

            var closest = int.MaxValue;
            var minDifference = int.MaxValue;

            foreach (var angle in angles)
            {
                var difference = Mathf.Abs((long)angle - value);
                if (minDifference > difference)
                {
                    minDifference = (int)difference;
                    closest = angle;
                }
            }

            return closest;
        }
    }

}
