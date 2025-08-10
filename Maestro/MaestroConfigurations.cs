using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SPToolkits.Maestro
{
    /// <summary>
    /// Provides all essential configurations needed to build the terminal and other non-essential perferences.
    /// </summary>
    public sealed class MaestroConfigurations
    {
        public readonly IMaestroIOHandler ioHandler;
        public readonly IEnumerable<IMaestroCommand> commands;

        public IMaestroParser parser = new MaestroDefaultParser();
        public System.Action<CommandExecutionResult> onCommandExecutedCallback = null;
        public bool printCommandExecutionResult = true;
        public bool printParserErrors = true;
        public string helpKeyword = "help";
        public string lineStarter = "> ";

        public char submissionChar = '\n';
        public System.Func<bool> submissionCondition = null;

        private MaestroConfigurations(IMaestroIOHandler ioHandler, IEnumerable<IMaestroCommand> commands) 
        {
            this.ioHandler = ioHandler;
            this.commands = commands;
        }

        /// <param name="ioHandler">This will provide the terminal with the input reader and output writer methods.</param>
        /// <param name="commands">All valid commands this terminal can execute.</param>
        public static MaestroConfigurations Create(IMaestroIOHandler ioHandler, IEnumerable<IMaestroCommand> commands) 
        {
            if (ioHandler == null)
                throw new System.Exception("IO provider cannot be null.");

            if (commands == null || commands.Count() <= 0)
                throw new System.Exception("No valid commands were passed in to the terminal builder.");

            return new MaestroConfigurations(ioHandler, commands);
        }


        /// <summary>
        /// The input scanner will submit the input string for execution upon encountering this character.
        /// <br>This logic is superceded if a submissionCondition is explicitely defined.</br>
        /// </summary>
        /// <param name="instance">The instance of this object</param>
        public MaestroConfigurations SetSubmissionChar(char submissionChars)
        {
            this.submissionChar = submissionChars;
            return this;
        }

        /// <summary>
        /// The input scanner will submit the input string upon this condition returning true.
        /// <br>This logic will supercede any implicite submission character check.</br>
        /// </summary>
        /// <param name="instance">The instance of this object</param>
        public MaestroConfigurations SetSubmissionCondition(System.Func<bool> submissionCondition)
        {
            this.submissionCondition = submissionCondition;
            return this;
        }

        /// <summary>
        /// Outputs a reference to this MaestroConfiguration instance.
        /// </summary>
        /// <param name="instance">The instance of this object</param>
        public MaestroConfigurations MakeReference(out MaestroConfigurations instance) 
        {
            instance = this;
            return this;
        }

        /// <summary>
        /// Sets a callback action that will be invoked when commands are executed.
        /// </summary>
        /// <param name="onCommandExecutedCallback">Callback event for when commands are executed.</param>
        public MaestroConfigurations SetOnCommandExecutedCallback(System.Action<CommandExecutionResult> onCommandExecutedCallback)
        {
            this.onCommandExecutedCallback = onCommandExecutedCallback;
            return this;
        }

        /// <summary>
        /// Set a keyword for the typical "help" commands in a terminal which prints out all the valid commands and their descriptions (default: <b>help</b>).
        /// </summary>
        /// <param name="helpKeyword">Help command keyword</param>
        public MaestroConfigurations SetHelpKeyword(string helpKeyword) 
        {
            this.helpKeyword = helpKeyword;
            return this;
        }


        /// <summary>
        /// Set a new parser for the terminal to use.
        /// <br>By default it uses the MaestroParser.</br>
        /// </summary>
        /// <param name="parser">A IMaestroParser object which defines the parsing methods.</param>
        public MaestroConfigurations SetParser(IMaestroParser parser) 
        {
            this.parser = parser;
            return this;
        }

        /// <summary>
        /// Sets whether the command executions results should be printed or not.
        /// </summary>
        /// <param name="printResults">If true, the command execution results will be printed to the terminal output.</param>
        public MaestroConfigurations SetPrintCommandExecutionResults(bool printResults) 
        {
            printCommandExecutionResult = printResults;
            return this;
        }

        /// <summary>
        /// A string that will be printed at the begining of every line on the terminal output.(default: <b>'> '</b>).
        /// </summary>
        /// <param name="lineStarter"></param>
        /// <returns></returns>
        public MaestroConfigurations SetLineStarter(string lineStarter) 
        {
            this.lineStarter = lineStarter;
            return this;
        }

        /// <summary>
        /// Sets boolean flag for whether the terminal should be print any parser errors or not.
        /// </summary>
        /// <param name="printParserErrors">If true, the terminal will print out the parse-status if parsing is not succesful.</param>
        public MaestroConfigurations SetPrintParserErrors(bool printParserErrors)
        {
            this.printParserErrors = printParserErrors;
            return this;
        }
    }
}
