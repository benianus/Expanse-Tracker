using Microsoft.Data.SqlClient;
using System.Data;

namespace ExpanseTrackerDataLayer
{
    public class ExpanseTrackerData
    {
        public static async Task<List<ExpanseTrackerDto>> GetExpansesList()
        {
            List<ExpanseTrackerDto> expanseList = new List<ExpanseTrackerDto>();

            using (SqlConnection connection = new SqlConnection(DataSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand("Sp_GetExpanseList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            expanseList.Add(
                                    new ExpanseTrackerDto(
                                        reader.GetInt32(reader.GetOrdinal("Id")),
                                        reader.GetDateTime(reader.GetOrdinal("Description")),
                                        reader.GetString(reader.GetOrdinal("Description")),
                                        reader.GetDecimal(reader.GetOrdinal("Amount"))
                                    ));
                        }
                    }
                }
            }

            return expanseList; 
        }
    }
}
