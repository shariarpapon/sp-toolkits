using System.Collections.Generic;
using System.Text;

namespace SPToolkits.Maestro
{
    public sealed class MaestroDefaultParser : IMaestroParser
    {
        public const char ESCAPE = '\\';
        public const char COMMAND_START = '/';
        public const char STRING_LITERAL = '"';

        public ParserOutput Parse(string source)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source))
                return new ParserOutput(ParseStatus.InvalidSourceString, null);

            string[] tokens = GenerateTokens(source);

            if (tokens == null || tokens.Length <= 0)
                return new ParserOutput(ParseStatus.NoValidTokensFound, null);

            List<ParsedCommand> cmds = new List<ParsedCommand>();

            bool parsing = false;
            string kwd = null;
            uint cmdCount = 0;
            List<string> args = new List<string>();

            foreach (string token in tokens)
            {
                if (token.StartsWith(COMMAND_START.ToString()))
                {
                    cmdCount++;
                    if (parsing == true)
                    {
                        cmds.Add(new ParsedCommand(kwd, args.ToArray()));
                        args.Clear();
                    }
                    kwd = token.Remove(0, 1);
                    parsing = true;
                    continue;
                }
                args.Add(token);
            }

            if (kwd != null)
                cmds.Add(new ParsedCommand(kwd, args.ToArray()));

            if (cmdCount == 0)
                return new ParserOutput(ParseStatus.NoValidCommandsFound, cmds.ToArray());

            return new ParserOutput(ParseStatus.Successful, cmds.ToArray()); ;
        }

        //Returns an array of strings that is essentially the source converted to tokens.
        private string[] GenerateTokens(string src)
        {
            var q = GetCharQueue(src);
            StringBuilder buffer = new StringBuilder();
            List<string> tokens = new List<string>();
            bool esacpeFlag = false;
            bool stringLiteralFlag = false;
            while (q.Count > 0)
            {
                char c = q.Dequeue();
                if (esacpeFlag)
                {
                    buffer.Append(c);
                    esacpeFlag = false;
                    continue;
                }

                switch (c)
                {
                    case ESCAPE:
                        esacpeFlag = true;
                        continue;
                    case STRING_LITERAL:
                        stringLiteralFlag = !stringLiteralFlag;
                        continue;
                    case ' ':
                        if (stringLiteralFlag) buffer.Append(c);
                        else if (!string.IsNullOrWhiteSpace(buffer.ToString()))
                        {
                            tokens.Add(buffer.ToString());
                            buffer.Clear();
                        }
                        continue;
                    default:
                        buffer.Append(c);
                        continue;
                }
            }
            if (buffer.Length > 0 && !string.IsNullOrWhiteSpace(buffer.ToString()))
                tokens.Add(buffer.ToString());
            return tokens.ToArray();
        }

        private Queue<char> GetCharQueue(string str)
        {
            Queue<char> queue = new Queue<char>();
            for (int i = 0; i < str.Length; i++)
                queue.Enqueue(str[i]);
            return queue;
        }
    }
}