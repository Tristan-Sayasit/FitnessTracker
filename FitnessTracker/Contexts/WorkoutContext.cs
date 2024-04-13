using Dapper;
using FitnessTracker.Models;
using MySqlConnector;

namespace FitnessTracker.Contexts
{
    public class WorkoutContext
    {
        public MySqlConnection _db;
        public WorkoutContext(MySqlConnection db)
        {
            _db = db;
        }

        public async Task CreateSplit(string userid, string splitName, string splitid)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@user_id", userid);
            parameters.Add("@split_name", splitName);
            parameters.Add("@split_id", splitid);
            await _db.ExecuteAsync("insert into splits (user_id, split_id, split_name, is_selected) values (@user_id, @split_id, @split_name, 'N')", parameters);
        }

        public async Task CreateSplitDay(DateOnly day, string splitid)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@split_id", splitid);
            parameters.Add("@date", day);
            parameters.Add("@split_day_id", Guid.NewGuid().ToString());
            await _db.ExecuteAsync(
                "insert into split_days (split_day_id, day_of_week, split_id) value (@split_day_id, @date, @split_id)",
                parameters);
        }

        public async Task<IEnumerable<SplitDaysModel>> GetSplitDaysFromSplitID(string splitid)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@split_id", splitid);
            return await _db.QueryAsync<SplitDaysModel>("select * from split_days where split_id = @split_id order by day_of_week asc", parameters);
        }
    }
}
