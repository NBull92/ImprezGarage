//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger.Helpers
{
    using System.Linq;

    public class Variable
    {
        /// <summary>
        /// Takes a camelCase, PascalCase, snake_case etc. variable name and makes it presentable:
        /// Camel Case, Pascal Case, Snake Case etc.
        /// </summary>
        public static string NameToLabel(string variableName)
        {
            if (variableName.Length < 3)
                return variableName;

            var finalString = variableName.Trim('_');
            if (variableName.Contains("_"))
            {
                var pos = finalString.TakeWhile(ch => ch != '_').Count();
                while (pos != finalString.Length)
                {
                    var head = finalString.Substring(0, pos);
                    var tail = finalString.Substring(pos + 1);
                    head = char.ToUpper(head[0]) + head.Substring(1);
                    tail = char.ToUpper(tail[0]) + tail.Substring(1);
                    finalString = head + " " + tail;

                    pos = finalString.TakeWhile(ch => ch != '_').Count();
                }
            }

            var endPos = finalString.Substring(1).TakeWhile(ch => !char.IsUpper(ch)).Count();
            var lastPos = 0;
            while (lastPos + endPos + 1 < finalString.Length)
            {
                var head = finalString.Substring(0, lastPos + endPos + 1);
                var tail = finalString.Substring(lastPos + endPos + 1);
                var prevLen = finalString.Length;
                var lenDelta = 0;

                if (char.IsLetter(tail[0]) && char.IsLetter(head.Last()) && char.IsUpper(tail[0]) && char.IsUpper(head.Last()))
                {
                    var upperRunLen = 1;
                    while (upperRunLen < tail.Length && char.IsLetter(tail[upperRunLen]) && char.IsUpper(tail[upperRunLen]))
                        ++upperRunLen;
                    if (upperRunLen == tail.Length
                    || char.IsWhiteSpace(tail[upperRunLen])
                    || (char.IsLetter(tail[upperRunLen])
                    && !char.IsLower(tail[upperRunLen])))
                    {
                        lenDelta = finalString.Length - prevLen;
                        lastPos = lastPos + endPos + 1;
                        endPos = tail.Substring(1).TakeWhile(ch => !char.IsUpper(ch)).Count();
                        lastPos += lenDelta;
                        continue;
                    }
                }
                head = char.ToUpper(head[0]) + head.Substring(1);
                tail = char.ToUpper(tail[0]) + tail.Substring(1);
                finalString = head.Trim() + " " + tail.Trim();

                lenDelta = finalString.Length - prevLen;
                lastPos = lastPos + endPos + 1;
                endPos = tail.Substring(1).TakeWhile(ch => !char.IsUpper(ch)).Count();
                lastPos += lenDelta;
            }

            return finalString;
        }
    }
}   //ImprezGarage.Modules.Logger.Helpers namespace 