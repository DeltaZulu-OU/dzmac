using System.Drawing;
using System.Windows.Forms;

namespace Dzmac.Core
{
    internal static class AppIconProvider
    {
        private static readonly Icon CachedIcon = TryExtractIcon();

        public static Icon GetIcon() => CachedIcon;

        public static Bitmap GetBitmap(Size size)
        {
            if (CachedIcon == null)
            {
                return null;
            }

            using var source = CachedIcon.ToBitmap();
            return new Bitmap(source, size);
        }

        private static Icon TryExtractIcon()
        {
            try
            {
                return Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch
            {
                return null;
            }
        }
    }
}
