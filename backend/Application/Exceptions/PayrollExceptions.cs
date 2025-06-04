namespace backend.Application.Exceptions
{
    public class PayrollException : Exception
    {
        public string ErrorType { get; }

        public PayrollException(string errorType, string message) : base(message)
        {
            ErrorType = errorType;
        }
    }
}