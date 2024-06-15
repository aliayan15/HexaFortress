using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class TileRotation
    {
        public static float GetTileRotation(int index)
        {
            float ros = 0;
            switch (index)
            {
                case 6:
                case 0:
                    ros = 0;
                    break;
                case 1:
                    ros = 60;
                    break;
                case 2:
                    ros = 120;
                    break;
                case 3:
                    ros = 180;
                    break;
                case 4:
                    ros = 240;
                    break;
                case 5:
                    ros = 300;
                    break;
                default:
                    Debug.LogError("Unvalid tile rotation index");
                    break;
            }

            return ros;
        }

        public static float GetNextRotation(int index, out int newIndex)
        {
            index++;
            if (index > 6)
                index = 1;
            newIndex = index;
            return GetTileRotation(index);
        }

        public static float GetPreviousRotation(int index, out int newIndex)
        {
            index--;
            if (index < 0)
                index = 5;
            newIndex = index;
            return GetTileRotation(index);
        }
    }
}