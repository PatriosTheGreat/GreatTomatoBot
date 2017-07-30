using Microsoft.Azure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TomatoBot.Repository
{
	public abstract class DbRepository
	{
		protected DbRepository()
		{
			_connectionString = CloudConfigurationManager.GetSetting("ConnectionString");
		}

		protected IEnumerable<T> ExecuteReader<T>(string queryString, Func<SqlDataReader, T> map, Action<SqlParameterCollection> fillParameters)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var command = new SqlCommand(queryString, connection);
				fillParameters(command.Parameters);
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						yield return map(reader);
					}
				}
			}
		}

		protected int ExecuteNonQuery(string queryString, Action<SqlParameterCollection> fillParameters)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var command = new SqlCommand(queryString, connection);
				fillParameters(command.Parameters);
				return command.ExecuteNonQuery();
			}
		}
		
		protected static void FillStringParameter(SqlParameterCollection parameterCollection, string parameterName, string parameterValue)
		{
			parameterCollection.Add($"@{parameterName}", SqlDbType.NVarChar);
			parameterCollection[$"@{parameterName}"].Value = parameterValue;
		}

		protected static void FillIntParameter(SqlParameterCollection parameterCollection, string parameterName, int parameterValue)
		{
			parameterCollection.Add($"@{parameterName}", SqlDbType.Int);
			parameterCollection[$"@{parameterName}"].Value = parameterValue;
		}

		protected static void FillDateTimeParameter(SqlParameterCollection parameterCollection, string parameterName, DateTime parameterValue)
		{
			parameterCollection.Add($"@{parameterName}", SqlDbType.DateTime);
			parameterCollection[$"@{parameterName}"].Value = parameterValue;
		}

		private readonly string _connectionString;
	}
}