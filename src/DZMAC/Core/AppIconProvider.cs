using System.Drawing;
using System.Windows.Forms;

namespace Dzmac.Core
{
    internal static class AppIconProvider
    {
        private static readonly Icon CachedIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

        public static Icon GetIcon() => CachedIcon;

        public static Bitmap GetBitmap(Size size)
        {
            if (CachedIcon == null)
            {
                return null;
            }

            return new Bitmap(CachedIcon.ToBitmap(), size);
        }
    }
}
