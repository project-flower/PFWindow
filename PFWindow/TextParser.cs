using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace PFWindow
{
    public static class TextParser
    {
        #region Public Methods

        public static Size ToSize(string text)
        {
            Match match = Regex.Match(text, "^(?<width>\\d+)x(?<height>\\d+)$");

            if (!match.Success)
            {
                throw new Exception($"\r\n\"{text}\"\r\nfailed to parse to size.");
            }

            GroupCollection groups = match.Groups;

            try
            {
                return new Size(int.Parse(groups["width"].Value), int.Parse(groups["height"].Value));
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
