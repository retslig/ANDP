//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Xml;
using System.ServiceModel.Security;
using System.IdentityModel.Tokens;
using System.IdentityModel.Selectors;

namespace Common.Lib.Common.UsernameToken
{
    /// <summary>
    /// UsernameSecurityTokenSerializer for use with the username Token
    /// </summary>
    /// 
    public class UsernameSecurityTokenSerializer : WSSecurityTokenSerializer
    {
        public UsernameSecurityTokenSerializer(SecurityTokenVersion version) : base() { }

        protected override bool CanReadTokenCore(XmlReader reader) 
        {
            XmlDictionaryReader localReader = XmlDictionaryReader.CreateDictionaryReader(reader);

            if (reader == null)
                throw new ArgumentNullException("reader");

            if (reader.IsStartElement(Constants.UsernameTokenName, Constants.UsernameTokenNamespace))
                return true;

            return base.CanReadTokenCore(reader);
        }

        protected override SecurityToken ReadTokenCore(XmlReader reader, SecurityTokenResolver tokenResolver)
        {
            if (reader == null) throw new ArgumentNullException("reader");

            if (reader.IsStartElement(Constants.UsernameTokenName, Constants.UsernameTokenNamespace))
            {
                string id = reader.GetAttribute(Constants.IdAttributeName, Constants.WsUtilityNamespace);

                reader.ReadStartElement();

                // read the user name
                string userName = reader.ReadElementString(Constants.UsernameElementName, Constants.UsernameTokenNamespace);

                // read the password hash
                string password = reader.ReadElementString(Constants.PasswordElementName, Constants.UsernameTokenNamespace);
                
                // read nonce
                string nonce = reader.ReadElementString(Constants.NonceElementName, Constants.UsernameTokenNamespace);
                
                // read created
                string created = reader.ReadElementString(Constants.CreatedElementName, Constants.WsUtilityNamespace);

                reader.ReadEndElement();

                UsernameInfo usernameInfo = new UsernameInfo(userName, password);

                return new UsernameToken(usernameInfo, nonce, created);
            }
            else
            {
                return WSSecurityTokenSerializer.DefaultInstance.ReadToken(reader, tokenResolver);
            }
        }

        protected override bool CanWriteTokenCore(SecurityToken token)
        {
            if (token is UsernameToken)
                return true;

            else
                return base.CanWriteTokenCore(token);
        }

        protected override void WriteTokenCore(XmlWriter writer, SecurityToken token)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            
            if (token == null)
                throw new ArgumentNullException("token");

            UsernameToken c = token as UsernameToken;
            if (c != null)
            {
                var passwordDigestAsBase64 = c.GetPasswordDigestAsBase64();
                var nonce = c.GetNonceAsBase64();
                var time = c.GetCreatedAsString();
                
                writer.WriteStartElement(Constants.UsernameTokenPrefix, Constants.UsernameTokenName, Constants.UsernameTokenNamespace);
                writer.WriteAttributeString(Constants.WsUtilityPrefix, Constants.IdAttributeName, Constants.WsUtilityNamespace, token.Id);
                writer.WriteElementString(Constants.UsernameElementName, Constants.UsernameTokenNamespace, c.UsernameInfo.Username);
                writer.WriteStartElement(Constants.UsernameTokenPrefix, Constants.PasswordElementName, Constants.UsernameTokenNamespace);
                writer.WriteAttributeString(Constants.TypeAttributeName, Constants.PasswordDigestType);
                writer.WriteValue(passwordDigestAsBase64);
                writer.WriteEndElement();
                writer.WriteStartElement(Constants.NonceElementName, Constants.UsernameTokenNamespace);
                writer.WriteAttributeString(Constants.NonceTypeAttributeName, Constants.NonceType);
                writer.WriteValue(nonce);
                writer.WriteEndElement();

                writer.WriteElementString(Constants.CreatedElementName, Constants.WsUtilityNamespace, time);
                writer.WriteEndElement();
                writer.Flush();
            }
            else
            {
                base.WriteTokenCore(writer, token);
            }
        }
    }
}
