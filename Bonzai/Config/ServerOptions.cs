using System.Security.Cryptography.X509Certificates;

namespace Bonzai.Config
{
    /// <summary>
    /// Defines Bonzai related server options.
    /// </summary>
    // We don't need to warn about this not being instantiated since we're a library.
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ServerOptions
    {
        private int Port { get; }
        private string IpAddress { get; }
        public NetworkScheme Scheme { get; }
        private X509Certificate2 Certificate { get; }
        
        /// <summary>
        /// Gets a new instance of the options class, used for initialization of Bonzai itself.
        /// </summary>
        public ServerOptions(string ip, int port, NetworkScheme scheme, X509Certificate2 certificate = null)
        {
            this.IpAddress = ip;
            this.Port = port;
            this.Scheme = scheme;
            this.Certificate = certificate;
        }
        
        /// <summary>
        /// Returns the full websocket URL
        /// </summary>
        /// <returns>full websocket url</returns>
        public string GetWebsocketUrl()
        {
            return string.Concat(GetPrefix(), IpAddress, ":", Port.ToString());
        }

        /// <summary>
        /// Gets the websocket prefix depending on the scheme chosen.
        /// </summary>
        /// <returns>websocket url prefix</returns>
        private string GetPrefix()
        {
            string prefix;
            switch (Scheme)
            {
                case NetworkScheme.SslScheme:
                    prefix = "wss://";
                    break;
                case NetworkScheme.NoSslScheme:
                    prefix = "ws://";
                    break;
                default:
                    prefix = "ws://";
                    break;
            }

            return prefix;
        }

        /// <summary>
        /// Returns the defined X509 certificate.
        /// </summary>
        /// <returns>x509 certificate</returns>
        public X509Certificate2 GetCertificate()
        {
            return Certificate;
        }
    }
}