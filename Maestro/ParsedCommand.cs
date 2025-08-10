using System.Text;

namespace SPToolkits.Maestro
{
    public readonly struct ParsedCommand
    {
        public readonly string keyword;
        public readonly string[] arguments;
        public readonly uint argumentCount;
        public ParsedCommand(string keyword, string[] arguments)
        {
            this.keyword = keyword;
            this.arguments = arguments;
            argumentCount = (uint)arguments.Length;
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder("keyword: " + keyword + "\n");
            buffer.AppendLine("args:");
            for(int i = 0; i < arguments.Length; i++)
                buffer.AppendLine(arguments[i]);
            return buffer.ToString();
        }
    }
}
