using DocuSign.eSign.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace TacarEZDocusignAPI
{
    public class Utility
    {
        public static string GetEnvironmentVariable(string key)
        {
            return System.Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
        }

        public static string GetAccessToken()
        {
            string basePath = GetEnvironmentVariable("basePath");
            var _apiClient = new ApiClient(basePath);
            var scopes = new List<string>
                {
                    "signature",
                    "impersonation",
                };
            var accessToken = _apiClient.RequestJWTUserToken(GetEnvironmentVariable("clientId"), GetEnvironmentVariable("userId"), GetEnvironmentVariable("oAuthBasePath"),
                DSHelper.ReadFileContent(DSHelper.PrepareFullPrivateKeyFilePath("private.key")), 1, scopes);
            return accessToken.access_token;
        }
    }
}
