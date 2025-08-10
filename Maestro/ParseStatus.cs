namespace SPToolkits.Maestro
{
    public enum ParseStatus 
    { 
        InvalidSourceString = 0,
        NoValidTokensFound,
        NoValidStatementsFound,
        NoValidCommandsFound,
        Failed,
        Successful
    }
}