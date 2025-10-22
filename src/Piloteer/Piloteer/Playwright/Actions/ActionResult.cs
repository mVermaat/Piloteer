namespace Piloteer.Playwright.Actions
{
    public class ActionResult
    {
        public bool AllowRetry { get; private set; }
        public int ErrorCode { get; private set; }
        public object[] ErrorMessageFormatArgs { get; private set; }
        public bool IsSuccessfull { get; private set; }

        public ActionResult()
        {
            ErrorMessageFormatArgs = [];
        }

        public static ActionResult Success()
            => new()
            {
                IsSuccessfull = true
            };

        public static ActionResult Fail(bool allowRetry, int errorCode, params object[] formatArgs)
            => new()
            {
                AllowRetry = allowRetry,
                ErrorCode = errorCode,
                ErrorMessageFormatArgs = formatArgs,
                IsSuccessfull = false
            };
    }

    public class ActionResult<T>
    {
        private ActionResult()
        {
            AllowRetry = true;
            ErrorMessageFormatArgs = [];
        }

        public bool AllowRetry { get; set; }

        public int ErrorCode { get; set; }

        public object[] ErrorMessageFormatArgs { get; set; }

        public bool IsSuccessfull { get; set; }

        public T? Result { get; set; }

        public static ActionResult<T> Success(T result)
           => new()
           {
               IsSuccessfull = true,
               Result = result,
           };

        public static ActionResult<T> Fail(bool allowRetry, int errorCode, params object[] formatArgs)
            => new()
            {
                AllowRetry = allowRetry,
                ErrorCode = errorCode,
                ErrorMessageFormatArgs = formatArgs,
                IsSuccessfull = false,
            };
    }
}
