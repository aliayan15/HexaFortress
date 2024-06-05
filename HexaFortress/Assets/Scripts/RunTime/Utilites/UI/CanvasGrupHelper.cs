using UI;

namespace MyUtilities.UI
{

    public static class CanvasGrupHelper
    {
        public static void HideAll(this CanvasGrupItem[] cgs)
        {
            for (int i = 0; i < cgs.Length; i++)
            {
                cgs[i].Toogle(false);
            }
        }

        public static void HideAllExceptOne(this CanvasGrupItem[] csg, CanvasGrupItem toShow)
        {
            for (int i = 0; i < csg.Length; i++)
            {
                if (toShow != null && csg[i] == toShow)
                {
                    csg[i].Toogle(true);
                }
                else
                {
                    csg[i].Toogle(false);
                }
            }
        }
    }
}
