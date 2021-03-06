﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BloodDonorsClientLibrary.Commands;
using BloodDonorsClientLibrary.Exceptions;
using BloodDonorsClientLibrary.Extensions;
using BloodDonorsClientLibrary.Models;
using Newtonsoft.Json;

namespace BloodDonorsClientLibrary.Services
{
    public abstract class HttpClientWithAuthorization : HttpClientBase
    {
        protected HttpClientWithAuthorization(HttpClient client) : base(client)
        {
            Token = "";
        }

        public bool IsLoggedIn { get; protected set; }
        protected string Token;

        /// <summary>
        /// Fires when user has been logged off (explicitly or after token expiration.
        /// </summary>
        public event EventHandler OnLogout;

        /// <summary>
        /// Fires when user has been logged in.
        /// </summary>
        public event EventHandler OnLogin;

        public async Task LoginAsync(string pesel, string password, string loginPath)
        {
            var loginCredentials = new LoginCredentials(pesel, password);

            var loginJson = JsonConvert.SerializeObject(loginCredentials);
            var response = await Client.PostJsonAsync(loginPath, new StringContent(loginJson));

            if (response.IsSuccessStatusCode)
            {
                var jwtJson = await response.Content.ReadAsStringAsync();
                var jwt = JsonConvert.DeserializeObject<Jwt>(jwtJson);

                Token = jwt.Token;
                AutomaticLogout(jwt.Expires);
                IsLoggedIn = true;
                OnLogin?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (response.StatusCode.Equals(HttpStatusCode.BadRequest))
                throw new InvalidLoginCredentialsException();
        }

        public void Logout()
        {
            Token = "";
            IsLoggedIn = false;

            OnLogout?.Invoke(this,EventArgs.Empty);
        }

        protected async void AutomaticLogout(DateTime dateTime)
        {
            var timeSpan = dateTime - DateTime.UtcNow;
            await Task.Delay(timeSpan);
            Logout();
        }
    }
}