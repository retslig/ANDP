using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Common.Lib.Common.UsernameToken
{
    public class UsernameToken : SecurityToken
    {
        UsernameInfo _usernameInfo;
        ReadOnlyCollection<SecurityKey> _securityKeys;
        DateTime _created = DateTime.Now;
        DateTime _expiration = DateTime.Now + new TimeSpan(10, 0, 0);
        Guid _id = Guid.NewGuid();
        byte[] _nonce = new byte[16];

        public UsernameToken(UsernameInfo usernameInfo, string nonce, string created)
        {
            if (usernameInfo == null)
                throw new ArgumentNullException("usernameInfo");

            _usernameInfo = usernameInfo;

            if (nonce != null)
            {
                _nonce = Convert.FromBase64String(nonce);
            }

            if (created != null)
            {
                _created = DateTime.Parse(created);
            }

            // the user name token is not capable of any crypto
            _securityKeys = new ReadOnlyCollection<SecurityKey>(new List<SecurityKey>());
        }

        public UsernameToken(UsernameInfo usernameInfo) : this(usernameInfo, null, null) { }

        public UsernameInfo UsernameInfo { get { return _usernameInfo; } }

        public override ReadOnlyCollection<SecurityKey> SecurityKeys { get { return _securityKeys; } }

        public override DateTime ValidFrom { get { return _created; } }
        public override DateTime ValidTo { get { return _expiration; } }
        public override string Id { get { return _id.ToString(); } }

        public string GetPasswordDigestAsBase64()
        {
            // generate a cryptographically strong random value
            RandomNumberGenerator rndGenerator = new RNGCryptoServiceProvider();
            rndGenerator.GetBytes(_nonce);

            // get other operands to the right format
            byte[] time = Encoding.UTF8.GetBytes(GetCreatedAsString());
            byte[] pwd = Encoding.UTF8.GetBytes(_usernameInfo.Password);
            byte[] operand = new byte[_nonce.Length + time.Length + pwd.Length];
            Array.Copy(_nonce, operand, _nonce.Length);
            Array.Copy(time, 0, operand, _nonce.Length, time.Length);
            Array.Copy(pwd, 0, operand, _nonce.Length + time.Length, pwd.Length);
            
            // create the hash
            SHA1 sha1 = SHA1.Create();
            return Convert.ToBase64String(sha1.ComputeHash(operand));
        }

        public string GetNonceAsBase64()
        {
            return Convert.ToBase64String(_nonce);            
        }

        public string GetCreatedAsString()
        {
            return XmlConvert.ToString(_created.ToUniversalTime(), "yyyy-MM-ddTHH:mm:ssZ");
        }

        public bool ValidateToken(string password)
        {
            byte[] pwd = Encoding.UTF8.GetBytes(password);
            byte[] createdBytes = Encoding.UTF8.GetBytes(GetCreatedAsString());
            byte[] operand = new byte[_nonce.Length + createdBytes.Length + pwd.Length];
            Array.Copy(_nonce, operand, _nonce.Length);
            Array.Copy(createdBytes, 0, operand, _nonce.Length, createdBytes.Length);
            Array.Copy(pwd, 0, operand, _nonce.Length + createdBytes.Length, pwd.Length);
            SHA1 sha1 = SHA1.Create();
            string trueDigest = Convert.ToBase64String(sha1.ComputeHash(operand));

            return String.Compare(trueDigest, _usernameInfo.Password) == 0;
        }
    }
}
