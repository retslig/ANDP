namespace Common.Lib.Common.UsernameToken
{
    public class UsernameInfo
    {
        string _userName;
        string _password;

        public UsernameInfo(string userName, string password)
        {
            this._userName = userName;
            this._password = password;
        }

        public string Username { get { return _userName; } }
        public string Password { get { return _password; } }
    }
}
