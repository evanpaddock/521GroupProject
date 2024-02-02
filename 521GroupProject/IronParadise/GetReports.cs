using IronParadise.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IronParadise
{
    public class GetReports
    {
        private readonly IConfiguration _configuration;

        public GetReports(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MostTypeOfUsersReport GetMostTypeOfUsers()
        {
            var report = new MostTypeOfUsersReport();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string query = @"SELECT TOP 1 FitnessLevel, COUNT(*) as Frequency
                             FROM dbo.FitnessInfo
                             GROUP BY FitnessLevel
                             ORDER BY Frequency DESC;";

            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(query, conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        report.FitnessLevel = reader["FitnessLevel"].ToString();
                        report.Frequency = Convert.ToInt32(reader["Frequency"]);
                    }
                }
            }

            return report;
        }

        public AverageTimePerSessionReport GetAverageTimePerSession()
        {
            var report = new AverageTimePerSessionReport();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string query = "SELECT AVG(SelectedDurationInMinutes) as AverageDuration FROM dbo.FitnessInfo;";

            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(query, conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        report.AverageDuration = Convert.ToDouble(reader["AverageDuration"]);
                    }
                }
            }

            return report;
        }

        public MostCommonGoalReport GetMostCommonGoal()
        {
            var report = new MostCommonGoalReport();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string query = @"SELECT TOP 1 WorkoutGoal, COUNT(*) as Frequency
                             FROM dbo.FitnessInfo
                             GROUP BY WorkoutGoal
                             ORDER BY Frequency DESC;";

            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(query, conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        report.WorkoutGoal = reader["WorkoutGoal"].ToString();
                        report.Frequency = Convert.ToInt32(reader["Frequency"]);
                    }
                }
            }

            return report;
        }

        public NumberOfUsersReport GetNumberOfUsers()
        {
            var report = new NumberOfUsersReport();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string query = @"SELECT COUNT(*) as UserCount
                             FROM [dbo].[AspNetUsers] AS U
                             JOIN [dbo].[AspNetUserRoles] AS UR ON U.Id = UR.UserId
                             JOIN [dbo].[AspNetRoles] AS R ON UR.RoleId = R.Id
                             WHERE R.Name = 'User';";

            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(query, conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        report.UserCount = Convert.ToInt32(reader["UserCount"]);
                    }
                }
            }

            return report;
        }
    }
}
