using System;
using System.Collections.Generic;
using System.IO;
namespace CardItNow
{
    public static class CyberSourceConfiguration
    {

        private static Dictionary<string, string> _configurationDictionary = new Dictionary<string, string>();

        public static Dictionary<string, string> GetConfiguration()
        {
            if (_configurationDictionary.Count == 0)
            {
                _configurationDictionary.Add("authenticationType", "HTTP_SIGNATURE");
                _configurationDictionary.Add("merchantID", "testrest");
                  _configurationDictionary.Add("merchantsecretKey", "yBJxy6LjM2TmcPGu+GaJrHtkke25fPpUX+UY6/L/1tE=");
                _configurationDictionary.Add("merchantKeyId", "08c94330-f618-42a3-b09d-e1e43be5efda");

               // _configurationDictionary.Add("merchantsecretKey", "e916ff207ea447b58da2ff8c6d8dae82d7b70bfd7d4d4020b0d552197c174d5e2c39d3618421403c849f78df6b101d83a37f2aa8c2c14bd6ae28bdd6122d958c821984cce68e4de292bfebecad40aadd967607ee630b45dcb97afe7997a4870574069c1a43a94fffa8931ee206f3965f904040b5453844ee830efde3ba8ed2e4");
               // _configurationDictionary.Add("merchantKeyId", "67ee04894df739af9ae4f87aa5f6bb32");
                _configurationDictionary.Add("keysDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Source\\Resource"));
                _configurationDictionary.Add("keyFilename", "testrest");
                _configurationDictionary.Add("runEnvironment", "apitest.cybersource.com");
                _configurationDictionary.Add("keyAlias", "testrest");
                _configurationDictionary.Add("keyPass", "testrest");
                _configurationDictionary.Add("timeout", "300000");

                // Configs related to meta key
                _configurationDictionary.Add("portfolioID", string.Empty);
                _configurationDictionary.Add("useMetaKey", "false");

                // Configs related to OAuth
                _configurationDictionary.Add("enableClientCert", "false");
                _configurationDictionary.Add("clientCertDirectory", "Resource");
                _configurationDictionary.Add("clientCertFile", "");
                _configurationDictionary.Add("clientCertPassword", "");
                _configurationDictionary.Add("clientId", "");
                _configurationDictionary.Add("clientSecret", "");

                // _configurationDictionary.Add("proxyAddress", string.Empty);
                // _configurationDictionary.Add("proxyPort", string.Empty);
                // _configurationDictionary.Add("proxyUsername", string.Empty);
                // _configurationDictionary.Add("proxyPassword", string.Empty);
            }

            return _configurationDictionary;
        }
    }
}
