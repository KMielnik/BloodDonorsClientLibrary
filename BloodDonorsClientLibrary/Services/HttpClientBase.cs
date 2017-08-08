using System;
using System.Net.Http;
using System.Net.Http.Headers;
using BloodDonorsClientLibrary.Commands;

namespace BloodDonorsClientLibrary.Services
{
    public abstract class HttpClientBase
    {
        protected readonly HttpClient Client;

        protected HttpClientBase(HttpClient client)
        {
            Client = client;
        }
    }
}