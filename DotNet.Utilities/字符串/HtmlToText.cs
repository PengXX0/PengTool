using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DotNet.Utilities.字符串
{
    /// <summary>
    /// Converts HTML to plain text.
    /// </summary>
    public class HtmlToText
    {
        // Static data tables
        protected static Dictionary<string, string> Tags;
        protected static HashSet<string> IgnoreTags;
        // Instance variables
        protected static TextBuilder Text;
        protected static string Html;
        protected static int Pos;
        // Static constructor (one time only)
        static HtmlToText()
        {
            Tags = new Dictionary<string, string>
            {
                {"address", "\n"},
                {"blockquote", "\n"},
                {"div", "\n"},
                {"dl", "\n"},
                {"fieldset", "\n"},
                {"form", "\n"},
                {"h1", "\n"},
                {"/h1", "\n"},
                {"h2", "\n"},
                {"/h2", "\n"},
                {"h3", "\n"},
                {"/h3", "\n"},
                {"h4", "\n"},
                {"/h4", "\n"},
                {"h5", "\n"},
                {"/h5", "\n"},
                {"h6", "\n"},
                {"/h6", "\n"},
                {"p", "\n"},
                {"/p", "\n"},
                {"table", "\n"},
                {"/table", "\n"},
                {"ul", "\n"},
                {"/ul", "\n"},
                {"ol", "\n"},
                {"/ol", "\n"},
                {"/li", "\n"},
                {"br", "\n"},
                {"/td", "\t"},
                {"/tr", "\n"},
                {"/pre", "\n"}
            };
            IgnoreTags = new HashSet<string> { "script", "noscript", "style", "object" };
        }
        /// <summary>
        /// Converts the given HTML to plain text and returns the result.
        /// </summary>
        /// <param name="html">HTML to be converted</param>
        /// <returns>Resulting plain text</returns>
        public static string Convert(string html)
        {
            // Initialize state variables
            Text = new TextBuilder();
            Html = html;
            Pos = 0;
            // Process input
            while (!EndOfText)
            {
                if (Peek() == '<')
                {
                    // HTML tag
                    bool selfClosing;
                    string tag = ParseTag(out selfClosing);
                    // Handle special tag cases
                    if (tag == "body")
                    {
                        // Discard content before <body>
                        Text.Clear();
                    }
                    else if (tag == "/body")
                    {
                        // Discard content after </body>
                        Pos = Html.Length;
                    }
                    else if (tag == "pre")
                    {
                        // Enter preformatted mode
                        Text.Preformatted = true;
                        EatWhitespaceToNextLine();
                    }
                    else if (tag == "/pre")
                    {
                        // Exit preformatted mode
                        Text.Preformatted = false;
                    }
                    string value;
                    if (Tags.TryGetValue(tag, out value))
                        Text.Write(value);
                    if (IgnoreTags.Contains(tag))
                        EatInnerContent(tag);
                }
                else if (Char.IsWhiteSpace(Peek()))
                {
                    // Whitespace (treat all as space)
                    Text.Write(Text.Preformatted ? Peek() : ' ');
                    MoveAhead();
                }
                else
                {
                    // Other text
                    Text.Write(Peek());
                    MoveAhead();
                }
            }
            // Return result
            return HttpUtility.HtmlDecode(Text.ToString());
        }
        // Eats all characters that are part of the current tag
        // and returns information about that tag
        protected static string ParseTag(out bool selfClosing)
        {
            string tag = String.Empty;
            selfClosing = false;
            if (Peek() == '<')
            {
                MoveAhead();
                // Parse tag name
                EatWhitespace();
                int start = Pos;
                if (Peek() == '/')
                    MoveAhead();
                while (!EndOfText && !Char.IsWhiteSpace(Peek()) &&
                  Peek() != '/' && Peek() != '>')
                    MoveAhead();
                tag = Html.Substring(start, Pos - start).ToLower();
                // Parse rest of tag
                while (!EndOfText && Peek() != '>')
                {
                    if (Peek() == '"' || Peek() == '\'')
                        EatQuotedValue();
                    else
                    {
                        if (Peek() == '/')
                            selfClosing = true;
                        MoveAhead();
                    }
                }
                MoveAhead();
            }
            return tag;
        }
        // Consumes inner content from the current tag
        protected static void EatInnerContent(string tag)
        {
            string endTag = "/" + tag;
            while (!EndOfText)
            {
                if (Peek() == '<')
                {
                    // Consume a tag
                    bool selfClosing;
                    if (ParseTag(out selfClosing) == endTag)
                        return;
                    // Use recursion to consume nested tags
                    if (!selfClosing && !tag.StartsWith("/"))
                        EatInnerContent(tag);
                }
                else MoveAhead();
            }
        }
        // Returns true if the current position is at the end of
        // the string
        protected static bool EndOfText
        {
            get { return (Pos >= Html.Length); }
        }
        // Safely returns the character at the current position
        protected static char Peek()
        {
            return (Pos < Html.Length) ? Html[Pos] : (char)0;
        }
        // Safely advances to current position to the next character
        protected static void MoveAhead()
        {
            Pos = Math.Min(Pos + 1, Html.Length);
        }
        // Moves the current position to the next non-whitespace
        // character.
        protected static void EatWhitespace()
        {
            while (Char.IsWhiteSpace(Peek()))
                MoveAhead();
        }
        // Moves the current position to the next non-whitespace
        // character or the start of the next line, whichever
        // comes first
        protected static void EatWhitespaceToNextLine()
        {
            while (Char.IsWhiteSpace(Peek()))
            {
                char c = Peek();
                MoveAhead();
                if (c == '\n')
                    break;
            }
        }
        // Moves the current position past a quoted value
        protected static void EatQuotedValue()
        {
            char c = Peek();
            if (c == '"' || c == '\'')
            {
                // Opening quote
                MoveAhead();
                // Find end of value
                Pos = Html.IndexOfAny(new[] { c, '\r', '\n' }, Pos);
                if (Pos < 0)
                    Pos = Html.Length;
                else
                    MoveAhead();  // Closing quote
            }
        }
        /// <summary>
        /// A StringBuilder class that helps eliminate excess whitespace.
        /// </summary>
        protected class TextBuilder
        {
            private readonly StringBuilder _text;
            private readonly StringBuilder _currLine;
            private int _emptyLines;
            private bool _preformatted;
            // Construction
            public TextBuilder()
            {
                _text = new StringBuilder();
                _currLine = new StringBuilder();
                _emptyLines = 0;
                _preformatted = false;
            }
            /// <summary>
            /// Normally, extra whitespace characters are discarded.
            /// If this property is set to true, they are passed
            /// through unchanged.
            /// </summary>
            public bool Preformatted
            {
                get
                {
                    return _preformatted;
                }
                set
                {
                    if (value)
                    {
                        // Clear line buffer if changing to
                        // preformatted mode
                        if (_currLine.Length > 0)
                            FlushCurrLine();
                        _emptyLines = 0;
                    }
                    _preformatted = value;
                }
            }
            /// <summary>
            /// Clears all current text.
            /// </summary>
            public void Clear()
            {
                _text.Length = 0;
                _currLine.Length = 0;
                _emptyLines = 0;
            }
            /// <summary>
            /// Writes the given string to the output buffer.
            /// </summary>
            /// <param name="s"></param>
            public void Write(string s)
            {
                foreach (char c in s)
                    Write(c);
            }
            /// <summary>
            /// Writes the given character to the output buffer.
            /// </summary>
            /// <param name="c">Character to write</param>
            public void Write(char c)
            {
                if (_preformatted)
                {
                    // Write preformatted character
                    _text.Append(c);
                }
                else
                {
                    switch (c)
                    {
                        case '\r':
                            break;
                        case '\n':
                            FlushCurrLine();
                            break;
                        default:
                            if (Char.IsWhiteSpace(c))
                            {
                                // Write single space character
                                int len = _currLine.Length;
                                if (len == 0 || !Char.IsWhiteSpace(_currLine[len - 1]))
                                    _currLine.Append(' ');
                            }
                            else
                            {
                                // Add character to current line
                                _currLine.Append(c);
                            }
                            break;
                    }
                }
            }
            // Appends the current line to output buffer
            protected void FlushCurrLine()
            {
                // Get current line
                string line = _currLine.ToString().Trim();
                // Determine if line contains non-space characters
                string tmp = line.Replace(" ", String.Empty);
                if (tmp.Length == 0)
                {
                    // An empty line
                    _emptyLines++;
                    if (_emptyLines < 2 && _text.Length > 0)
                        _text.AppendLine(line);
                }
                else
                {
                    // A non-empty line
                    _emptyLines = 0;
                    _text.AppendLine(line);
                }
                // Reset current line
                _currLine.Length = 0;
            }
            /// <summary>
            /// Returns the current output as a string.
            /// </summary>
            public override string ToString()
            {
                if (_currLine.Length > 0)
                    FlushCurrLine();
                return _text.ToString();
            }
        }
    }
}
