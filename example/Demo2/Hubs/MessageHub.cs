using Demo2.Messages;
using Microsoft.AspNetCore.SignalR;

namespace Demo2.Hubs
{
    /// <summary>
    /// ignalR 허브를 이용한 메세지 처리를 담당하는 클래스
    /// Class for handling messages through a SignalR hub
    /// </summary>
    public class MessageHub : Hub
    {
        /// <summary>
        /// 연결 아이디를 해싱하는 메소드
        /// Method for hashing the connection ID
        /// </summary>
        public async Task<string?> Hasing(string? connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId)) return null;

            return await Task.FromResult(Hashing.Encrypt(connectionId));
        }
    }
}