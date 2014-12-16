using System;
using MyCouch.Extensions;

namespace MyCouch.Net
{
    public class ServerClientConnection : Connection, IServerClientConnection
    {
        public ServerClientConnection(Uri serverUri, int timeout = 0) : base(serverUri, timeout)
        {
            var dbName = Address.ExtractDbName();

            if (!string.IsNullOrWhiteSpace(dbName))
            {
#if PCL
                throw new FormatException(
                    string.Format(ExceptionStrings.ServerClientSeemsToConnectToDb, Address.OriginalString));
#else
                throw new UriFormatException(
                    string.Format(ExceptionStrings.ServerClientSeemsToConnectToDb, Address.OriginalString));
#endif
            }
        }
    }
}