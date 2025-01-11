using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using HttpServerLibrary.Core;
namespace MyORMLibrary;

/// <summary>
/// ������������ �������� ORM ��� ������ � ����� ������.
/// </summary>
/// <typeparam name="T">��� ��������.</typeparam>
public class ORMContext<T> where T : class, new()
{
    private readonly IDbConnection _dbConnection;

    /// <summary>
    /// �������������� ����� ��������� ������ <see cref="ORMContext{T}"/>.
    /// </summary>
    /// <param name="dbConnection">���������� � ����� ������.</param>
    public ORMContext(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    /// <summary>
    /// ������ �������� �� ��������������.
    /// </summary>
    /// <param name="id">������������� ��������.</param>
    /// <param name="tableName">��� �������.</param>
    /// <returns>�������� ���� T.</returns>
    public T ReadById(int id, string tableName)
    {
        string query = $"SELECT * FROM Users WHERE Id=@id";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    return Map(reader);
            }
        }
        return new T();
    }

    /// <summary>
    /// ������ ��� �������� �� �������.
    /// </summary>
    /// <param name="tableName">��� �������.</param>
    /// <returns>������ ��������� ���� T.</returns>
    public List<T> ReadAll(string tableName)
    {
        List<T> results = new List<T>();
        string sql = $"SELECT * FROM {tableName}";
        try
        {
            using (var command = _dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                _dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(Map(reader));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // ����������� ������
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            _dbConnection.Close();
        }
        return results;
    }

    /// <summary>
    /// ������� ����� �������� � �������.
    /// </summary>
    /// <param name="entity">�������� ��� ��������.</param>
    /// <param name="tableName">��� �������.</param>
    /// <returns>��������� ��������.</returns>
    public T Create<T>(T entity, string tableName) where T : class
    {
        using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
        {
            connection.Open();

            var properties = typeof(T).GetProperties();

            // ������ SQL-������
            var columnNames = new List<string>();
            var parameterNames = new List<string>();

            foreach (var property in properties)
            {
                columnNames.Add(property.Name);
                parameterNames.Add("@" + property.Name);
            }

            string sql = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", parameterNames)}); SELECT SCOPE_IDENTITY();";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                foreach (var property in properties)
                {
                    var value = property.GetValue(entity);
                    command.Parameters.AddWithValue("@" + property.Name, value ?? DBNull.Value); // ������������� ��������, ������� null �� DBNull
                }

                var id = command.ExecuteScalar();

                var idProperty = typeof(T).GetProperty("Id");
                if (idProperty != null && idProperty.CanWrite)
                {
                    idProperty.SetValue(entity, Convert.ToInt32(id));
                }
            }
        }

        return entity;
    }

    /// <summary>
    /// ��������� �������� � �������.
    /// </summary>
    /// <param name="id">������������� ��������.</param>
    /// <param name="entity">�������� ��� ����������.</param>
    /// <param name="tableName">��� �������.</param>
    public void Update<T>(int id, T entity, string tableName)
    {
        using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
        {
            connection.Open();
            string sql = $"UPDATE {tableName} SET Column1 = @value1 WHERE Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@value1", "��������");

            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// ��������� �������� �� ���������.
    /// </summary>
    /// <param name="predicate">�������� ��� ����������.</param>
    /// <returns>������ ���������, ��������������� ���������.</returns>
    public List<T> Where(Expression<Func<T, bool>> predicate)
    {
        var sqlQuery = ExpressionParser<T>.BuildSqlQuery(predicate, singleResult: false);
        return ExecuteQueryMultiple(sqlQuery).ToList();
    }

    /// <summary>
    /// ������� �������� �� ��������������.
    /// </summary>
    /// <param name="id">������������� ��������.</param>
    /// <param name="tableName">��� �������.</param>
    /// <returns>���������� ���������� �����.</returns>
    public int Delete(int id, string tableName)
    {
        string query = "DELETE FROM Users WHERE Id=@id";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            return command.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// ��������� ������ � ���������� ���� ��������.
    /// </summary>
    /// <param name="query">SQL-������.</param>
    /// <returns>�������� ���� T ��� null, ���� �������� �� �������.</returns>
    private T ExecuteQuerySingle(string query)
    {
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close();
        }

        return null;
    }

    /// <summary>
    /// ��������� ������ � ���������� ������ ���������.
    /// </summary>
    /// <param name="query">SQL-������.</param>
    /// <returns>������ ��������� ���� T.</returns>
    private IEnumerable<T> ExecuteQueryMultiple(string query)
    {
        var results = new List<T>();
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(Map(reader));
                }
            }
            _dbConnection.Close();
        }
        return results;
    }

    /// <summary>
    /// ������ ������ ������ �� ��������������.
    /// </summary>
    /// <param name="movieId">������������� ������.</param>
    /// <returns>�������� ���� T ��� null, ���� ����� �� ������.</returns>
    public T ReadMovieById(int movieId)
    {
        string query = $"SELECT * FROM MovieDatas WHERE MovieId=@movieId";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@movieId";
            parameter.Value = movieId;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
        }
        return null;
    }

    /// <summary>
    /// ������� ����� ��������.
    /// </summary>
    /// <param name="entity">�������� ��� ��������.</param>
    public void Create(T entity)
    {
        var properties = entity.GetType().GetProperties()
            .Where(p => p.Name != "Id");
        var columns = string.Join(", ", properties.Select(p => p.Name));
        var values = string.Join(", ", properties.Select(p => '@' + p.Name));
        string query = $"INSERT INTO {typeof(T).Name}s ({columns}) VALUES ({values})";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    /// <summary>
    /// ������� ����� �����.
    /// </summary>
    /// <param name="entity">�������� ������ ��� ��������.</param>
    public void CreateMovie(T entity)
    {
        var properties = entity.GetType().GetProperties()
            .Where(p => p.Name != "MovieId" && p.Name != "Id"); // ��������� MovieId � Id
        var columns = string.Join(", ", properties.Select(p => p.Name));
        var values = string.Join(", ", properties.Select(p => '@' + p.Name));
        string query = $"INSERT INTO {typeof(T).Name}s ({columns}) VALUES ({values})";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    /// <summary>
    /// ������� ����� ������ ������.
    /// </summary>
    /// <param name="entity">�������� ������ ������ ��� ��������.</param>
    public void CreateMovieData(T entity)
    {
        var properties = entity.GetType().GetProperties();
        var columns = string.Join(", ", properties.Select(p => p.Name));
        var values = string.Join(", ", properties.Select(p => '@' + p.Name));
        string query = $"INSERT INTO {typeof(T).Name}s ({columns}) VALUES ({values})";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    /// <summary>
    /// �������� �������� �� ��������������.
    /// </summary>
    /// <param name="Id">������������� ��������.</param>
    /// <returns>�������� ���� T ��� null, ���� �������� �� �������.</returns>
    public T GetById(int Id)
    {
        string query = $"SELECT * FROM {typeof(T).Name}s WHERE Id = @Id";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = Id;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }

    /// <summary>
    /// ��������� ��������.
    /// </summary>
    /// <param name="entity">�������� ��� ����������.</param>
    public void Update(T entity)
    {
        var properties = entity.GetType().GetProperties()
            .Where(p => p.Name != "Id");
        var values = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
        string query = $"UPDATE {typeof(T).Name}s SET {values} WHERE Id = @Id";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@Id";
            idParameter.Value = entity.GetType().GetProperty("Id").GetValue(entity);
            command.Parameters.Add(idParameter);
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    /// <summary>
    /// ������� �������� �� ��������������.
    /// </summary>
    /// <param name="id">������������� ��������.</param>
    /// <param name="tableName">��� �������.</param>
    public void Delete(string id, string tableName)
    {
        string query = $"DELETE FROM {tableName} WHERE Id = @Id";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = id;
            command.Parameters.Add(parameter);
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    /// <summary>
    /// ������� ������ ������ �� ��������������.
    /// </summary>
    /// <param name="id">������������� ������ ������.</param>
    /// <param name="tableName">��� �������.</param>
    public void DeleteMovieData(string id, string tableName)
    {
        string query = $"DELETE FROM {tableName} WHERE MovieId = @MovieId";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@MovieId";
            parameter.Value = id;
            command.Parameters.Add(parameter);
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    /// <summary>
    /// ������� ��������.
    /// </summary>
    /// <param name="entity">�������� ��� ��������.</param>
    public void Delete(T entity)
    {
        var properties = entity.GetType().GetProperties();
        var condition = string.Join(" AND ", properties.Select(p => $"{p.Name} = @{p.Name}"));
        string query = $"DELETE FROM {typeof(T).Name}s WHERE {condition}";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity);
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    /// <summary>
    /// ������� ����� �� ��������������.
    /// </summary>
    /// <param name="id">������������� ������.</param>
    /// <param name="tableName">��� �������.</param>
    public void DeleteMovie(string id, string tableName)
    {
        string query = $"DELETE FROM {tableName} WHERE MovieId = @MovieId";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@MovieId";
            parameter.Value = id;
            command.Parameters.Add(parameter);
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    /// <summary>
    /// �������� ��� ��������.
    /// </summary>
    /// <returns>������ ��������� ���� T.</returns>
    public List<T> GetAll()
    {
        string query = $"SELECT * FROM {typeof(T).Name}s";
        using (var command = _dbConnection.CreateCommand())
        {
            var result = new List<T>();
            command.CommandText = query;
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(Map(reader));
                }
                _dbConnection.Close();
                return result;
            }
        }
    }

    /// <summary>
    /// ����� �������� ��� ���������� ���������.
    /// </summary>
    /// <returns>�������� ���� T.</returns>
    public T Where()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// �������� ������ ��������, ��������������� ���������.
    /// </summary>
    /// <param name="predicate">�������� ��� ����������.</param>
    /// <returns>�������� ���� T ��� null, ���� �������� �� �������.</returns>
    public T FirstOrDefault(Predicate<T> predicate)
    {
        var query = $"SELECT * FROM {typeof(T).Name}s";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var entity = Map(reader);
                    if (predicate(entity))
                    {
                        return entity;
                    }
                }
            }
            _dbConnection.Close();
        }
        return null;
    }

    /// <summary>
    /// �������� �������� �� �����.
    /// </summary>
    /// <param name="Name">��� ��������.</param>
    /// <returns>�������� ���� T ��� null, ���� �������� �� �������.</returns>
    public T GetByName(string Name)
    {
        string query = $"SELECT * FROM {typeof(T).Name}s WHERE Name = @Name";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Name";
            parameter.Value = Name;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }

    /// <summary>
    /// �������� �������� �� ��������.
    /// </summary>
    /// <param name="Title">�������� ��������.</param>
    /// <returns>�������� ���� T ��� null, ���� �������� �� �������.</returns>
    public T GetByTitle(string Title)
    {
        string query = $"SELECT * FROM {typeof(T).Name}s WHERE Title = @Title";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Title";
            parameter.Value = Title;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }

    /// <summary>
    /// �������� �������� �� �������� �����.
    /// </summary>
    /// <param name="GenreName">�������� �����.</param>
    /// <returns>�������� ���� T ��� null, ���� �������� �� �������.</returns>
    public T GetByGenreName(string GenreName)
    {
        string query = $"SELECT * FROM {typeof(T).Name}s WHERE GenreName = @GenreName";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@GenreName";
            parameter.Value = GenreName;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }

    /// <summary>
    /// �������� �������� �� �������� ������.
    /// </summary>
    /// <param name="CountryName">�������� ������.</param>
    /// <returns>�������� ���� T ��� null, ���� �������� �� �������.</returns>
    public T GetByCountryName(string CountryName)
    {
        string query = $"SELECT * FROM {typeof(T).Name}s WHERE CountryName = @CountryName";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@CountryName";
            parameter.Value = CountryName;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }

    /// <summary>
    /// �������� ������������ �� ������.
    /// </summary>
    /// <param name="Login">����� ������������.</param>
    /// <returns>�������� ���� T ��� null, ���� ������������ �� ������.</returns>
    public T GetUserByLogin(string Login)
    {
        string query = $"SELECT * FROM {typeof(T).Name}s WHERE Login = @Login";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Login";
            parameter.Value = Login;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }

    /// <summary>
    /// ������� ������ �� IDataReader � �������� ���� T.
    /// </summary>
    /// <param name="reader">IDataReader ��� ������ ������.</param>
    /// <returns>�������� ���� T.</returns>
    private T Map(IDataReader reader)
    {
        var obj = new T();
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (reader[property.Name] != DBNull.Value)
            {
                var value = reader[property.Name];
                var propertyType = property.PropertyType;

                // �������������� �������� � ���������� ���
                if (propertyType == typeof(int))
                {
                    property.SetValue(obj, Convert.ToInt32(value));
                }
                else if (propertyType == typeof(string))
                {
                    property.SetValue(obj, Convert.ToString(value));
                }
                else if (propertyType == typeof(DateTime))
                {
                    property.SetValue(obj, Convert.ToDateTime(value));
                }
                else if (propertyType == typeof(bool))
                {
                    property.SetValue(obj, Convert.ToBoolean(value));
                }
                else if (propertyType == typeof(double))
                {
                    property.SetValue(obj, Convert.ToDouble(value));
                }
                else if (propertyType == typeof(decimal))
                {
                    property.SetValue(obj, Convert.ToDecimal(value));
                }
                else if (propertyType == typeof(float))
                {
                    property.SetValue(obj, Convert.ToSingle(value));
                }
                else if (propertyType == typeof(long))
                {
                    property.SetValue(obj, Convert.ToInt64(value));
                }
                else if (propertyType == typeof(short))
                {
                    property.SetValue(obj, Convert.ToInt16(value));
                }
                else if (propertyType == typeof(byte))
                {
                    property.SetValue(obj, Convert.ToByte(value));
                }
                else if (propertyType == typeof(char))
                {
                    property.SetValue(obj, Convert.ToChar(value));
                }
                else if (propertyType == typeof(Guid))
                {
                    property.SetValue(obj, Guid.Parse(value.ToString()));
                }
                else
                {
                    property.SetValue(obj, value);
                }
            }
        }

        return obj;
    }
    public int? ReadMovieIdByTitle(string title)
    {
        string query = "SELECT MovieId FROM Movies WHERE Title = @Title";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Title";
            parameter.Value = title;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetInt32(reader.GetOrdinal("MovieId"));
                }
            }
        }
        return null;
    }
    public string GetQueryParameter(HttpRequestContext context, string parameterName)
    {
        var queryString = context.Request.QueryString;
        if (queryString == null)
        {
            Console.WriteLine("Query string is null.");
            return null;
        }

        var parameterValue = queryString[parameterName];
        if (string.IsNullOrEmpty(parameterValue))
        {
            Console.WriteLine($"Parameter {parameterName} is missing or empty.");
            return null;
        }

        return parameterValue;
    }

}
