﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyCouch.Net;

namespace MyCouch.UnitTests.Fakes
{
    public class ServerClientConnectionFake : IServerClientConnection
    {
        public Uri Address { get; private set; }

        public ServerClientConnectionFake(Uri address)
        {
            Address = address;
        }

        public void Dispose() { }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return null;
        }
    }

    public class DbClientConnectionFake : IDbClientConnection
    {
        public string DbName { get; private set; }
        public Uri Address { get; private set; }

        public DbClientConnectionFake(Uri address, string dbName)
        {
            Address = address;
            DbName = dbName;
        }

        public void Dispose() { }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
