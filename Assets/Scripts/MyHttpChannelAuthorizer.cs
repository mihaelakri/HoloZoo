using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
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
        // Guard.ChannelName(channelName);

        string authToken = null;


        var baseAddress = new Uri(CommConstants.ServerURL);

        if (CommConstants.Auth == "")
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
            };

            using (HttpContent content = new FormUrlEncodedContent(data))
            {
                HttpResponseMessage response = null;
                try
                {
                    PreAuthorize(httpClient);
                    response = await httpClient.PostAsync(_authEndpoint, content).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    ErrorCodes code = ErrorCodes.ChannelAuthorizationError;
                    if (e is TaskCanceledException)
                    {
                        code = ErrorCodes.ChannelAuthorizationTimeout;
                    }

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

        return authToken;
        
        // var tcs = new TaskCompletionSource<string>();
        // StartCoroutine(GetAuth(channelName, socketId, tcs));
        // return await MakePostRequestAsync(channelName, socketId);
    }

    public Task<string> MakePostRequestAsync(string channel_name, string socket_id) {
        var tcs = new TaskCompletionSource<string>();
        // WWWForm form = new WWWForm();
        // form.AddField("channel_name", channel_name);
        // form.AddField("socket_id", socket_id);

        UnityWebRequest request = new UnityWebRequest(CommConstants.ServerURL+"broadcasting/auth", "POST");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes("{\"channel_name\":\"channel_name\", \"socket_id\":\"socket_id\"}"));
        request.downloadHandler = new DownloadHandlerBuffer();

        // using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"broadcasting/auth", form))

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

    // IEnumerator GetAuth(string channel_name, string socket_id, TaskCompletionSource<string> tcs){

        
    //     // Debug.Log(PlayerPrefs.GetString("id_animal"));
    //     WWWForm form = new WWWForm();
    //     form.AddField("channel_name", channel_name);
    //     form.AddField("socket_id", socket_id);
    //     // string id_animal = PlayerPrefs.GetString("id_animal");

    //     using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"broadcasting/auth", form)){

    //         yield return www.SendWebRequest();

    //         if (www.result != UnityWebRequest.Result.Success)
    //             {
    //                 Debug.Log(www.error);
    //             }
    //         else
    //             {
    //                 Debug.Log(www.downloadHandler.text);
    //                 tcs.SetResult(www.downloadHandler.text);
    //             }
    //     }
    // }

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
}