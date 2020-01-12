
namespace ImprezGarage.Modules.Firebase
{
    using FireSharp;
    using FireSharp.Config;
    using FireSharp.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Connection
    {
        private readonly IFirebaseClient _client;

        public Connection()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = ConnectionCredentials.AuthSecret,
                BasePath = ConnectionCredentials.BasePath,
            };

            _client = new FirebaseClient(config);
        }

        public List<T> Get<T>()
        {
            var name = typeof(T).Name;
            var response =  _client.Get($"{name}s/");

            return response.Body != null ? response.ResultAs<List<T>>() : new List<T>();
        }

        public async Task<List<T>> GetAsync<T>()
        {
            var name = typeof(T).Name;
            var response = await _client.GetTaskAsync($"{name}s/");
            return response.Body != null ? response.ResultAs<List<T>>() : new List<T>();
        }

        public async void Submit<T>(T obj, int id)
        {
            var name = typeof(T).Name;
            await _client.SetTaskAsync($"{name}s/{id}", obj);
        }

        public void Delete<T>(int id)
        {
            var name = typeof(T).Name;
            _client.Delete($"{name}s/{id}");
        }

        public void Update<T>(T obj, int id)
        {
            var name = typeof(T).Name;
            _client.UpdateTaskAsync($"{name}s/{id}", obj);
        }
    }
}
