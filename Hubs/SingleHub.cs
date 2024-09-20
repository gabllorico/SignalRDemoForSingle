using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Models;

namespace SignalRDemo.Hubs
{
    public class SingleHub : Hub
    {
        private static List<DBSample> _listOfConnectedUsers = new List<DBSample>();

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            var user = _listOfConnectedUsers.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (user == null)
            {
                //claims pwede dito
                _listOfConnectedUsers.Add(new DBSample
                {
                    ConnectionId = connectionId
                });
                await base.OnConnectedAsync();
            }
        }

        public async Task AddUserIdOnConnection(int id)
        {
            string connectionId = Context.ConnectionId;
            var count = _listOfConnectedUsers.Count();
            var user = _listOfConnectedUsers.First(x => x.ConnectionId == connectionId);
            user.UserId = count.ToString();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string connectionId = Context.ConnectionId;
            var user = _listOfConnectedUsers.First(x => x.ConnectionId == connectionId);
            _listOfConnectedUsers.Remove(user);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Success(string id)
        {
            var connectionId = _listOfConnectedUsers.First(x => x.UserId == id).ConnectionId;
            //do something with id to update
            await Clients.Client(connectionId).SendAsync("RedirectSuccess", "/home/redirectpage");
        }
    }
}
