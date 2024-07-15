namespace Calculator.ErrorHandler
{
    public static class ErrorHandler
    {
        public static void SendError(Exception exception)
        {
            if (exception == null)
            {
                Console.WriteLine("An unknown error occurred.");
            }
            else
            {
                Console.WriteLine($"Exception: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
            }
        }
    }
}