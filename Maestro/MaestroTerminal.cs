using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;

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

        private Task _asyncScanTask;
        private CancellationTokenSource _cancellationTokenSource;
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

        /// <summary>
        /// Starts scannign the input for submission asyncronously.
        /// </summary>
        public void StartAsyncScanner() 
        {
            if (IsScanningAsync == true)
                return;

            IsScanningAsync = true;
            _cancellationTokenSource = new CancellationTokenSource();
            _asyncScanTask = ScanInputSubmissionAsync(_cancellationTokenSource.Token);
        }

        /// <summary>
        /// Stops the async scanner task and cleans up.
        /// </summary>
        public async void StopAsyncScanner() 
        {
            if (IsScanningAsync == false)
                return;

            IsScanningAsync = false;
            _cancellationTokenSource.Cancel();

            try
            {
                await _asyncScanTask;
            }
            catch (OperationCanceledException) { }
            finally
            {
                _cancellationTokenSource.Dispose();
            }
        }


        /// <summary>
        /// Asynchronously scans the terminal input field for the submission character (default: newline <b>\n</b>).
        /// </summary>
        public async Task ScanInputSubmissionAsync(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested && IsScanningAsync)
            {
                var input = _ioHandler.Read();
                if (IsInputSubmitReady(input))
                {
                    Scan(input);
                    _ioHandler.ClearInput();
                }
                await Task.Delay(1, cancelToken);
            }
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
            if (WasPredefinedCommandExecuted(source))
                return;

            ParserOutput parserOutput = _configurations.Parser.Parse(source);
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
            if (Commands.Count() <= 0)
            {
                TerminalWrite("Start commands with: " + _configurations.Parser.CommandStart);
                return;
            }
            const string line = "------------------------------------";
            StringBuilder buffer = new StringBuilder($"COMMANDS\n{line}\n");
            string space = "    ";
            foreach (var cmd in Commands)
            {
                buffer.AppendLine($"{space}{cmd.Keyword}");
                buffer.AppendLine($"{space}min args: {cmd.MinArgCount}");
                if (cmd is ICommandDescriptionProvider)
                    buffer.AppendLine($"{space}{((ICommandDescriptionProvider)cmd).Description}");
                buffer.AppendLine(line);
            }
            TerminalWrite(buffer.ToString());
            return;
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
            if (result == null)
            {
                TerminalWrite("FATAL: Command execution result is null.");
                return;
            }

            StringBuilder argBuffer = new StringBuilder();
            for (int i = 0; i < result.parsedCommand.arguments.Length; i++) 
            {
                argBuffer.Append($"{result.parsedCommand.arguments[i]}");
                if (i == result.parsedCommand.arguments.Length - 1)
                    continue;
                argBuffer.Append(", ");
            }

            string commandStr = $"{result.parsedCommand.keyword}({argBuffer.ToString()})";

            switch (result.executionStatus)
            {
                default:
                    TerminalWrite($"{commandStr} status: {result.executionStatus}. " + result.exception?.Message);
                    break;
                case CommandExecutionStatus.Successful:
                    TerminalWrite($"{commandStr} executed successfully.");
                    break;
                case CommandExecutionStatus.FailedExecution:
                    TerminalWrite($"{commandStr} execution failed.");
                    break;
                case CommandExecutionStatus.InvalidArgumentCount:
                    TerminalWrite($"{commandStr} does not meet required argument count. [entered: {result.parsedCommand.argumentCount}] [req: {result.commandDefinition.MinArgCount}] ");
                    break;
                case CommandExecutionStatus.KeywordNotFound:
                    TerminalWrite($"{commandStr} keyword not found.");
                    break;
                case CommandExecutionStatus.FatalError:
                    if(result.exception != null)
                        TerminalWrite($"{commandStr} fatal: " + result.exception.Message);
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
