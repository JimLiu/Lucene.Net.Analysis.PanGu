using System;
using System.Collections.Generic;
using System.Text;

namespace PanGu.HighLight
{
    public interface Formatter
    {
        string HighlightTerm(string originalText);
    }
}
