using Microsoft.Data.SqlClient;
using System.Data;

namespace ExpanseTrackerDataLayer
{
    public class ExpanseTrackerData
    {
        public static async Task<List<ExpanseTrackerDto?>> GetExpansesList()
        {
            List<ExpanseTrackerDto?> expanseList = new List<ExpanseTrackerDto?>();

            try
            {
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
                                            reader.GetDateTime(reader.GetOrdinal("Date")),
                                            reader.GetString(reader.GetOrdinal("Description")),
                                            reader.GetDecimal(reader.GetOrdinal("Amount"))
                                        ));
                            }
                        }

                        connection.Close();
                    }
                }
            }
            catch (Exception error)
            {
                System.Console.WriteLine(error.Message);
                throw;
            }

            return expanseList; 
        }
        public static async Task<ExpanseTrackerDto?> GetExpanseById(int ExpanseId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DataSettings.connectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_GetExpanseById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", ExpanseId);

                        connection.Open();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                return new ExpanseTrackerDto(
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetDateTime(reader.GetOrdinal("Date")),
                                    reader.GetString(reader.GetOrdinal("Description")),
                                    reader.GetDecimal(reader.GetOrdinal("Amount"))
                                );
                            }
                        }

                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return null;
        }
        public static async Task<int?> AddNewExpanse(ExpanseTrackerDto? expanseDto)
        {
            int expanseId = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(DataSettings.connectionString))
                using (SqlCommand command = new SqlCommand("Sp_AddNewExpanse", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;  

                    command.Parameters.AddWithValue("@Date", expanseDto?.Date);
                    command.Parameters.AddWithValue("@Description", expanseDto?.Description);
                    command.Parameters.AddWithValue("@Amount", expanseDto?.Amount);

                    var OutputParameter = new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(OutputParameter);

                    connection.Open();

                    await command.ExecuteNonQueryAsync();

                    expanseId = (int)OutputParameter.Value;

                    connection.Close();

                }
            }
            catch (Exception)
            {

                throw;
            }
            
            return expanseId;
        }
    }
    
}
