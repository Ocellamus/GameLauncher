using System;
using System.Drawing;
using System.IO;

namespace GameLauncher
{
    /// <summary>
    /// Provides asset-loading helpers that normalise non-standard file
    /// extensions before handing them to the underlying image decoders.
    /// In particular, <c>.p00</c> files are treated as PNG images.
    /// </summary>
    internal static class AssetLoader
    {
        /// <summary>
        /// Loads an <see cref="Image"/> from <paramref name="path"/>.
        /// If the file extension is <c>.p00</c>, the file is interpreted as a
        /// PNG image regardless of its extension.
        /// </summary>
        /// <param name="path">Absolute or relative path to the image file.</param>
        /// <returns>
        /// A <see cref="Bitmap"/> containing the decoded image.
        /// The caller is responsible for disposing the returned bitmap.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="path"/> is null or empty.</exception>
        /// <exception cref="FileNotFoundException">The file does not exist.</exception>
        internal static Bitmap LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path must not be null or empty.", nameof(path));

            if (!File.Exists(path))
                throw new FileNotFoundException("Image file not found.", path);

            string extension = Path.GetExtension(path);

            // .p00 files are PNG data with a non-standard extension.
            // Stream the file into a MemoryStream so the image decoder
            // relies on file content (PNG header), not the file extension.
            // A MemoryStream is used instead of FileStream to avoid
            // locking the file for the lifetime of the Bitmap.
            if (string.Equals(extension, ".p00", StringComparison.OrdinalIgnoreCase))
            {
                var ms = new MemoryStream();
                using (var fs = File.OpenRead(path))
                {
                    fs.CopyTo(ms);
                }
                ms.Position = 0;
                return new Bitmap(ms);
            }

            // For all other extensions, load normally.
            return new Bitmap(path);
        }

        /// <summary>
        /// Returns the normalised extension for a given file path.
        /// <c>.p00</c> is mapped to <c>.png</c>; all other extensions are
        /// returned unchanged (lower-cased).
        /// </summary>
        internal static string NormaliseExtension(string path)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ext == ".p00" ? ".png" : ext;
        }
    }
}
