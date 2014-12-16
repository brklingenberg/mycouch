﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;

namespace MyCouch.Net
{
    public abstract class Connection : IConnection
    {
        protected HttpClient HttpClient { get; private set; }
        protected bool IsDisposed { get; private set; }

        public Uri Address
        {
            get { return HttpClient.BaseAddress; }
        }

        protected Connection(Uri uri, int timeout)
        {
            Ensure.That(uri, "uri").IsNotNull();

            HttpClient = CreateHttpClient(uri, timeout);
            IsDisposed = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed || !disposing)
                return;

            if (HttpClient != null)
            {
                HttpClient.CancelPendingRequests();
                HttpClient.Dispose();
                HttpClient = null;
            }
        }

        protected virtual void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        protected HttpClient CreateHttpClient(Uri uri, int timeout)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(uri.GetAbsoluteUriExceptUserInfo().TrimEnd(new[] { '/' }))
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpContentTypes.Json));

            if (timeout != 0)
            {
                client.Timeout = TimeSpan.FromMilliseconds(timeout);
            }

            var basicAuthString = uri.GetBasicAuthString();
            if (basicAuthString != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthString.Value);
            }

            return client;
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest)
        {
            ThrowIfDisposed();

            OnBeforeSend(httpRequest);

            using (var message = CreateHttpRequestMessage(httpRequest))
            {
                return await HttpClient.SendAsync(message).ForAwait();
            }
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            OnBeforeSend(httpRequest);

            using (var message = CreateHttpRequestMessage(httpRequest))
            {
                return await HttpClient.SendAsync(message, cancellationToken).ForAwait();
            }
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption)
        {
            ThrowIfDisposed();

            OnBeforeSend(httpRequest);

            using (var message = CreateHttpRequestMessage(httpRequest))
            {
                return await HttpClient.SendAsync(message, completionOption).ForAwait();
            }
        }

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            OnBeforeSend(httpRequest);

            using (var message = CreateHttpRequestMessage(httpRequest))
            {
                return await HttpClient.SendAsync(message, completionOption, cancellationToken).ForAwait();
            }
        }

        protected virtual HttpRequestMessage CreateHttpRequestMessage(HttpRequest httpRequest)
        {
            httpRequest.RemoveRequestTypeHeader();

            var message = new HttpRequestMessage(httpRequest.Method, GenerateRequestUri(httpRequest))
            {
                Content = httpRequest.Content,
            };

            foreach (var kv in httpRequest.Headers)
                message.Headers.TryAddWithoutValidation(kv.Key, kv.Value);

            return message;
        }

        protected virtual void OnBeforeSend(HttpRequest httpRequest) { }

        protected virtual string GenerateRequestUri(HttpRequest httpRequest)
        {
            return string.Format("{0}/{1}", Address.ToString().TrimEnd('/'), httpRequest.RelativeUrl.TrimStart('/'));
        }
    }
}