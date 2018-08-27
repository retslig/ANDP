namespace Common.Lib.MVC.Exceptions
{
    public class SessionExpiredException : System.Exception
    {
        // The default constructor needs to be defined
        // explicitly now since it would be gone otherwise.

        public SessionExpiredException()
        {
        }

        public SessionExpiredException(string message)
            : base(message)
        {
        }
    }
}
