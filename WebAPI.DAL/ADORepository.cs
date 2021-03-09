using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public class ADORepository : IADORepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public ADORepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config["Data:4Sea_Server:ConnectionString"];
        }

        public VesselAisUpdateModel GetVesselData(string searchQuery)
        {
            VesselAisUpdateModel existing = new VesselAisUpdateModel();
            using (SqlConnection connection = GetConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.CommandText = searchQuery;
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        existing.Mmsi = (int)rdr["MMSI"];
                        existing.SpeedMax = null;

                        if (!string.IsNullOrEmpty(rdr["SpeedMax"].ToString()))
                        {
                            if (double.TryParse(rdr["SpeedMax"].ToString(), out double d))
                            {
                                existing.SpeedMax = d;
                            }
                        }

                        if (!string.IsNullOrEmpty(rdr["DraughtMax"].ToString()))
                        {
                            if (double.TryParse(rdr["DraughtMax"].ToString(), out double d))
                            {
                                existing.DraughtMax = d;
                            }
                        }
                    }
                }

                connection.Close();
            }

            return existing;
        }

        private SqlConnection GetConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            return connection;
        }

        public void SetUpdates(string updateQuery)
        {
            using (SqlConnection connection = GetConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandTimeout = 3000;
                command.CommandType = CommandType.Text;
                command.CommandText = updateQuery;
                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
