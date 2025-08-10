using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace SPToolkits.Maestro
{
    /// <summary>
    /// Maestro terminal environment. 
    /// <br>Handles all the terminal logic.</br>
    /// </summary>
    public sealed class MaestroTerminal
    {
        public bool IsScanningAsync { get; private set; } = false;
        /// <summary>
        /// A globally unique identifier for this terminal.
        /// </summary>
        public readonly string GUID;
        public IEnumerable<IMaestroCommand> Commands => _configurations.commands;
        private MaestroConfigurations _configurations;
        private CommandHandler _commandHandler;
        private IMaestroIOHandler _ioHandler;

        /// <summary>
        /// Initialize the terminal with the given configurations.
        /// </summary>
        public MaestroTerminal(MaestroConfigurations configurations)
        {
            GUID = GenerateUID();
            _configurations = configurations;

            _ioHandler = _configurations.ioHandler;
            _commandHandler = new CommandHandler(this, configurations.commands);
        }

        ~MaestroTerminal() { IsScanningAsync = false; }

        public void StartAsyncScanner() 
        {
            if (IsScanningAsync == true)
                return;

            IsScanningAsync = true;
            ScanInputSubmissionAsync();
        }

        public void StopAsyncScanner() { IsScanningAsync = false; }


        /// <summary>
        /// Asynchronously scans the terminal input field for the submission character (default: newline <b>\n</b>).
        /// </summary>
        public async void ScanInputSubmissionAsync()
        {
            string input;
            do
            {
                input = _ioHandler.Read();
                if (IsInputSubmitReady(input))
                {
                    _ioHandler.ClearInput();
                    Scan(input);
                }
                await Task.Yield();
            }
            while (IsScanningAsync);
        }

        /// <summary>
        /// Scans the terminal input for input submission character (default: <b>\n</b>).
        /// <br>Upon submission, the input will be scanned for commands and the inputfield will be cleared to prevent repeated scanning of the same command.</br>
        /// </summary>
        public void ScanInputSubmission() 
        {
            string input = _ioHandler.Read();
            if (!IsInputSubmitReady(input))
                return;

            _ioHandler.ClearInput();
            Scan(input);
        }

        /// <summary>
        /// Scans the terminal input for any valid commands and executes them if found.
        /// <br>Does not wait for input submission character.</br>
        /// </summary>
        public void ScanInput() 
        {
            Scan(_ioHandler.Read());
        }

        /// <summary>
        /// Scans the given input string for any valid commands and executes them if found.
        /// <br>Does not wait for input submission character.</br>
        /// </summary>
        /// <param name="source">The source string to be scanned.</param>
        public void Scan(string source) 
        {
            //Check for predefined commands
            if (WasPredefinedCommandExecuted(source))
                return;

            ParserOutput parserOutput = _configurations.parser.Parse(source);
            if (parserOutput.status != ParseStatus.Successful)
            {
                if(_configurations.printParserErrors)
                    TerminalWrite($"Unable to parse source string (status: {parserOutput.status}).");
                return;
            }

            CommandExecutionResult[] results = _commandHandler.Execute(parserOutput.commands);

            foreach (CommandExecutionResult result in results)
            {
                if (_configurations.printCommandExecutionResult)
                    PrintCommandExecutionResult(result);

                _configurations.onCommandExecutedCallback?.Invoke(result);
            }
        }


        /// <summary>
        /// Writes the given string to the terminal output.
        /// </summary>
        public void TerminalWrite(string output) 
        {
            if (!string.IsNullOrEmpty(_configurations.lineStarter))
                output = $"{_configurations.lineStarter}{output}";
            _ioHandler.Write(output);
        }

        /// <summary>
        /// Prints out information of all the commands as defined.
        /// </summary>
        /// <returns>Neatly formatted command information.</returns>
        public void PrintHelperInformation() 
        {
            TerminalWrite("Start commands with: " + _configurations.parser.CommandStart);
            if (Commands.Count() <= 0)
            {
                TerminalWrite("No commands defined.");
                return;
            }
            TerminalWrite("COMMANDS <command_keyword>, <min_arg_count>");
            foreach (var cmd in Commands) 
            {
                string buf = $"{cmd.Keyword}, {cmd.MinArgCount}";
                TerminalWrite(buf);
            }
        }

        /// <returns>True, if a predefined command was found and executed.</returns>
        private bool WasPredefinedCommandExecuted(string source) 
        {
            if (source == _configurations.helpKeyword)
            {
                PrintHelperInformation();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if input was submitted either based on a submission-character check or any explicitely defined condition in the configs.
        /// </summary>
        /// <returns>True, if input detection condition is satisfied and false otherweise.</returns>
        private bool IsInputSubmitReady(string source)
        {
            if (string.IsNullOrEmpty(source)) 
                return false;

            if (_configurations.submissionCondition == null)
                return source[^1] == _configurations.submissionChar;
            else 
                return _configurations.submissionCondition.Invoke();
        }
        
        /// <summary>
        /// Prints information based on the execution result to the terminal output.
        /// </summary>
        private void PrintCommandExecutionResult(CommandExecutionResult result) 
        {
            switch (result.executionStatus)
            {
                default:
                    TerminalWrite($"[{result.parsedCommand.keyword}] status: {result.executionStatus}. " + result.exception?.Message);
                    break;
                case CommandExecutionStatus.Successful:
                    TerminalWrite($"[{result.parsedCommand.keyword}] executed succesfully.");
                    break;
                case CommandExecutionStatus.FailedExecution:
                    TerminalWrite($"[{result.parsedCommand.keyword}] execution failed.");
                    break;
                case CommandExecutionStatus.InvalidArgumentCount:
                    TerminalWrite($"[{result.parsedCommand.keyword}] does not meet required argument count. [entered: {result.parsedCommand.argumentCount}] [req: {result.commandDefinition.MinArgCount}] ");
                    break;
                case CommandExecutionStatus.KeywordNotFound:
                    TerminalWrite($"[{result.parsedCommand.keyword}] keyword not found.");
                    break;
                case CommandExecutionStatus.FatalError:
                    if(result.exception != null)
                        TerminalWrite($"[{result.parsedCommand.keyword}] fatal: " + result.exception.Message);
                    break;
            }
        }

        /// <summary>
        /// Generates a globally unique identifier for this terminal.
        /// </summary>
        /// <returns>The generated GUID.</returns>
        private string GenerateUID()
        {
            return $"{DateTime.UtcNow}" +
                   $"/{Guid.NewGuid()}" +
                   $"/{GetHashCode()}";
        }
    }
}
