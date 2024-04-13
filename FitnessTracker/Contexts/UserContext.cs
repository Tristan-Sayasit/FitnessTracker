using Dapper;
using FitnessTracker.Models;
using MySqlConnector;

namespace FitnessTracker.Contexts
{
    public class UserContext
    {
        public MySqlConnection _db;

        public UserContext(MySqlConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            return await _db.QueryAsync<UserModel>("select * from users");
        }
                
        public async Task CreateUser(string username, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@user_id", Guid.NewGuid().ToString());
            parameters.Add("@username", username);
            parameters.Add("@password", password);
            await _db.ExecuteAsync("insert into users (user_id, username, password) values (@user_id, @username, @password)", parameters);
        }

        public async Task<UserModel?> LoginUser(string username, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@username", username);
            parameters.Add("@password", password);
            var result = await _db.QueryAsync<UserModel>("select * from users where username = @username and password = @password", parameters);
            return result.FirstOrDefault();
        }

        public async Task<UserModel> GetUserByID(string id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var result = await _db.QueryAsync<UserModel>("select * from users where user_id = @id", parameters);
            return result.First();
        }

        public async Task UpdateUser(UserModel um)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@username", um.username);
            parameters.Add("@password", um.password);
            parameters.Add("@userID", um.user_id);
            parameters.Add("@height", um.height);
            parameters.Add("@weight", um.weight);
            await _db.ExecuteAsync("update users set username = @username, password = @password, weight = @weight, height = @height where user_id = @userID", parameters);
        }

        public async Task<IEnumerable<SplitModel>> GetUserSplits(string userid)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@userID", userid);
            return await _db.QueryAsync<SplitModel>("select * from splits where user_id = @userID", parameters);
        }
    }
}
