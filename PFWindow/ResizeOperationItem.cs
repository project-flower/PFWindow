using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Text.RegularExpressions;

namespace PFWindow
{
    public class ResizeOperationItem
    {
        #region Public Fields

        public readonly Size Size;
        public readonly string Text;

        #endregion

        #region Static Methods

        public static IEnumerable<ResizeOperationItem> Load(StringCollection lines)
        {
            foreach (string line in lines)
            {
                Match match = Regex.Match(line, "^(?<name>[^,]+),(?<size>\\d+x\\d+)$", RegexOptions.IgnoreCase);
                ResizeOperationItem result;

                try
                {
                    if (!match.Success)
                    {
                        throw new Exception();
                    }

                    GroupCollection groups = match.Groups;
                    result = new ResizeOperationItem(
                        groups["name"].Value , TextParser.ToSize(groups["size"].Value));
                }
                catch
                {
                    throw new Exception($"Size confg\r\n\"{line}\"\r\nis invalid.");
                }

                yield return result;
            }
        }

        #endregion

        #region Public Methods

        public ResizeOperationItem(string text, Size size)
        {
            Text = text;
            Size = size;
        }

        public override string ToString()
        {
            return Text;
        }

        #endregion
    }
}
