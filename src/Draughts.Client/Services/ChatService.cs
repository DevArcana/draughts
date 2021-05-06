using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace Draughts.Client.Services
{
    public record Message(string Author, string Content);
    
    public class ChatService : IAsyncDisposable
    {
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _js;
        private HubConnection _hub;
        private Action _onStateChange;

        public string Username { get; set; }
        public List<Message> Messages { get; } = new();

        public ChatService(NavigationManager navigationManager, IJSRuntime js)
        {
            _navigationManager = navigationManager;
            _js = js;
        }

        public async Task Initialize(Action onStateChange)
        {
            _onStateChange = onStateChange;
            
            if (_hub is null)
            {
                _hub = new HubConnectionBuilder()
                    .WithUrl(_navigationManager.ToAbsoluteUri("/signalr/chat"))
                    .Build();

                _hub.On<string, string>("ReceiveMessage", ReceiveMessage);

                await _hub.StartAsync();
            }
        }

        public async Task SendMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            await _hub.SendAsync("SendMessage", Username, message);
        }

        private void ReceiveMessage(string sender, string content)
        {
            Messages.Add(new Message(sender, content));
            _onStateChange?.Invoke();
            _js.InvokeVoidAsync("scrollToLastMessage");
        }

        public async ValueTask DisposeAsync()
        {
            await _hub.DisposeAsync();
        }
    }
}