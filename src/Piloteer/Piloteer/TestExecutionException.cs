namespace Piloteer
{
    [Serializable]
    public class TestExecutionException : Exception
    {
        public virtual int ErrorCode { get; }

        public TestExecutionException(int errorCode) :
            base(ErrorCodes.GetErrorMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        public TestExecutionException(int errorCode, params object[] formatArgs) :
            base(ErrorCodes.GetErrorMessage(errorCode, formatArgs))
        {
            ErrorCode = errorCode;
        }

        public TestExecutionException(int errorCode, Exception inner) :
            base(ErrorCodes.GetErrorMessage(errorCode), inner)
        {
            ErrorCode = errorCode;
        }

        public TestExecutionException(int errorCode, Exception inner, params object[] formatArgs) :
           base(ErrorCodes.GetErrorMessage(errorCode, formatArgs), inner)
        {
            ErrorCode = errorCode;
        }
    }
}
