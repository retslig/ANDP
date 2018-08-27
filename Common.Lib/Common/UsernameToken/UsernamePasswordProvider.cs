namespace Common.Lib.Common.UsernameToken
{
    // this class provides the stored password for a given username
    public class UsernamePasswordProvider
    {
        public string RetrievePassword(string username)
        {
            if (username == "User1")
            {
                return "P@ssw0rd";
            }
            return null;
        }
    }
}
