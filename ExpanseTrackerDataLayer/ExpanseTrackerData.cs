﻿using Microsoft.Data.SqlClient;
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
                using (SqlConnection connection = new SqlConnection(DataSettings.ConnectionString))
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
                                            reader.GetDecimal(reader.GetOrdinal("Amount")),
                                            (reader.IsDBNull(reader.GetOrdinal("CategoryId"))) ? null : reader.GetInt32(reader.GetOrdinal("CategoryId"))
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
        public static async Task<ExpanseTrackerDto?> GetExpanseById(int expanseId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DataSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Sp_GetExpanseById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Id", expanseId);

                        connection.Open();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                return new ExpanseTrackerDto(
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetDateTime(reader.GetOrdinal("Date")),
                                    reader.GetString(reader.GetOrdinal("Description")),
                                    reader.GetDecimal(reader.GetOrdinal("Amount")),
                                    reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? null : reader.GetInt32(reader.GetOrdinal("CategoryId"))
                                );
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

            return null;
        }
        public static async Task<int> GetAllExpansesSummary()
        {
            int summary = 0;
            try
            {
                using (var connection = new SqlConnection(DataSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("Sp_ExpansesSummary", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        object? result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int outSum))
                        {
                            summary = Convert.ToInt32(outSum);
                        }

                        connection.Close();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                throw;
            }

            return summary;
        }
        public static async Task<int> GetExpansesSummaryByMonth(int month)
        {
            int summary = 0;

            try
            {
                using (var connection = new SqlConnection(DataSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("Sp_ExpansesSummaryByMonth", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Month", month);

                        connection.Open();

                        object? result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int outSum))
                        {
                            summary = Convert.ToInt32(outSum);
                        }

                        connection.Close();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                throw;
            }

            return summary;
        }
        public static async Task<List<ExpanseTrackerDto>?> GetExpansesByCategory(int CategoryId)
        {
            List<ExpanseTrackerDto>? expansesList = new List<ExpanseTrackerDto>();

            try
            {
                using (var connection = new SqlConnection(DataSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("Sp_GetExpansesByCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CategoryId", CategoryId);

                        connection.Open();

                        var reader = await command.ExecuteReaderAsync();

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                expansesList.Add(
                                    new ExpanseTrackerDto(
                                        reader.GetInt32(reader.GetOrdinal("Id")),
                                        reader.GetDateTime(reader.GetOrdinal("Date")),
                                        reader.GetString(reader.GetOrdinal("Description")),
                                        reader.GetDecimal(reader.GetOrdinal("Amount")),
                                        reader.GetInt32(reader.GetOrdinal("CategoryId"))
                                    )
                                );
                            }
                        }
                        else
                        {
                            return null;
                        }

                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return expansesList;
        }
        public static async Task<int?> AddNewExpanse(ExpanseTrackerDto? expanseDto)
        {
            int expanseId = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(DataSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand("Sp_AddNewExpanse", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;  

                    command.Parameters.AddWithValue("@Date", expanseDto?.Date);
                    command.Parameters.AddWithValue("@Description", expanseDto?.Description);
                    command.Parameters.AddWithValue("@Amount", expanseDto?.Amount);
                    command.Parameters.AddWithValue("@CategoryId", expanseDto?.CategoryId);

                    connection.Open();

                    object? result = await command.ExecuteScalarAsync();

                    if (result != null && int.TryParse(result.ToString(), out int insertedId))
                    {
                        expanseId = Convert.ToInt32(insertedId);
                    }

                    connection.Close();

                }
            }
            catch (Exception error)
            {
                System.Console.WriteLine(error.Message);
                throw;
            }
            
            return expanseId;
        }
        public static async Task<bool> UpdateExpanse(int? id, ExpanseTrackerDto dto)
        {
            int rowsAffected = 0;

            try
            {
                using var connection = new SqlConnection(DataSettings.ConnectionString);
                using (var command = new SqlCommand("Sp_UpdateExpanse", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Date", dto.Date);
                    command.Parameters.AddWithValue("@Description", dto.Description);
                    command.Parameters.AddWithValue("@Amount", dto.Amount);
                    command.Parameters.AddWithValue("CategoryId", dto.CategoryId);

                    connection.Open();

                    object? result = await command.ExecuteScalarAsync();

                    if (result != null && int.TryParse(result.ToString(), out int outId))
                    {
                        rowsAffected = Convert.ToInt32(outId);
                    }

                    connection.Close();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                throw;
            }

            return rowsAffected > 0;
        }
        public static async Task<bool> DeleteExpanse(int Id)
        {
            int rowsAffected = 0;

            using var connection = new SqlConnection(DataSettings.ConnectionString);
            using (var command = new SqlCommand("Sp_DeleteExpanse", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", Id);     
            
                connection.Open();
            
                object? result = await command.ExecuteScalarAsync();

                if (result != null && int.TryParse(result.ToString(), out int outId))
                {
                    rowsAffected = Convert.ToInt32(outId);
                }
                
                connection.Close();
            }

            return rowsAffected > 0;
        }
        public static async Task<bool> AddMonthBudget(int MonthId, MonthBudgetDto dto)
        {
            int rowsAffected = 0;

            try
            {
                using (var connection = new SqlConnection(DataSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("Sp_AddMonthBudget", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@MonthId", MonthId);
                        command.Parameters.AddWithValue("@Budget", dto.Budget);

                        connection.Open();  

                        rowsAffected = await command.ExecuteNonQueryAsync();

                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return rowsAffected > 0;
        }
    }
}
