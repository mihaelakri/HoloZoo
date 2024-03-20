using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;/*
using PusherClient;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Threading;
public class MyHttpChannelAuthorizer: IAuthorizer, IAuthorizerAsync
{
    private readonly Uri _authEndpoint;

    public MyHttpChannelAuthorizer(string authEndpoint)
    {
        _authEndpoint = new Uri(authEndpoint);
    }

    public AuthenticationHeaderValue AuthenticationHeader { get; set; }

    public TimeSpan? Timeout { get; set; }
    public string Authorize(string channelName, string socketId)
    {
        string result;
        try
        {
            result = AuthorizeAsync(channelName, socketId).Result;
        }
        catch(AggregateException aggregateException)
        {
            throw aggregateException.InnerException;
        }

        return result;
    }

    public async Task<string> AuthorizeAsync(string channelName, string socketId)
    {
        string authToken = null;


        var baseAddress = new Uri(CommConstants.ServerURL);

        if (CommConstants.XSRF == "")
            Thread.Sleep(20);

        var cookieContainer = new CookieContainer();
        cookieContainer.Add(baseAddress, new Cookie("holozoo_session", CommConstants.Auth));
        cookieContainer.Add(baseAddress, new Cookie("XSRF-TOKEN", CommConstants.XSRF));


        using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
        using (var httpClient = new HttpClient(handler) { BaseAddress = baseAddress })
        {
            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("channel_name", channelName),
                new KeyValuePair<string, string>("socket_id", socketId),
                new KeyValuePair<string, string>("id_user", CommConstants.IdUser),
            };

            using (HttpContent content = new FormUrlEncodedContent(data))
            {
                HttpResponseMessage response = null;
                try
                {
                    PreAuthorize(httpClient);
                    Debug.Log("Pusher - PreAuthorize done");
                    response = await httpClient.PostAsync(_authEndpoint, content).ConfigureAwait(false);
                    Debug.Log("Pusher - response code: "+response.StatusCode);
                }
                catch (Exception e)
                {
                    ErrorCodes code = ErrorCodes.ChannelAuthorizationError;
                    if (e is TaskCanceledException)
                    {
                        code = ErrorCodes.ChannelAuthorizationTimeout;
                    }
                    Debug.Log("Pusher - error code: "+code);
                    Debug.Log("Pusher - _authEndpoint: "+_authEndpoint.OriginalString);
                    Debug.Log("Pusher - err: "+e);
                    throw new ChannelAuthorizationFailureException(code, _authEndpoint.OriginalString, channelName, socketId, e);
                }

                if (response.StatusCode == HttpStatusCode.RequestTimeout || response.StatusCode == HttpStatusCode.GatewayTimeout)
                {
                    throw new ChannelAuthorizationFailureException($"Authorization timeout ({response.StatusCode}).", ErrorCodes.ChannelAuthorizationTimeout, _authEndpoint.OriginalString, channelName, socketId);
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new ChannelUnauthorizedException(_authEndpoint.OriginalString, channelName, socketId);
                }

                try
                {
                    response.EnsureSuccessStatusCode();
                    authToken = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                catch(Exception e)
                {
                    throw new ChannelAuthorizationFailureException(ErrorCodes.ChannelAuthorizationError, _authEndpoint.OriginalString, channelName, socketId, e);
                }
            }
        }

        Debug.Log("Pusher - auth token: "+authToken);
        return authToken;
    }

    public Task<string> MakePostRequestAsync(string channel_name, string socket_id) {
        var tcs = new TaskCompletionSource<string>();

        UnityWebRequest request = new UnityWebRequest(CommConstants.ServerURL+"broadcasting/auth", "POST");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes("{\"channel_name\":\"channel_name\", \"socket_id\":\"socket_id\"}"));
        request.downloadHandler = new DownloadHandlerBuffer();

        var operation = request.SendWebRequest();

        operation.completed += op =>
        {
        if (request.result != UnityWebRequest.Result.Success)
            {
                tcs.SetException(new Exception("Failed to perform request"));
                Debug.Log(request.error);
            }
        else
            {
                Debug.Log(request.downloadHandler.text);
                tcs.SetResult(request.downloadHandler.text);
            }
        };
        return tcs.Task;
        
        
    }

    public virtual void PreAuthorize(HttpClient httpClient)
    {
        if (Timeout.HasValue)
        {
            httpClient.Timeout = Timeout.Value;
        }

        if (AuthenticationHeader != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeader;
        }
    }
}*/