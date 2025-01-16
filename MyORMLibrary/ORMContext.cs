using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using HttpServerLibrary.Core;

namespace MyORMLibrary
{
    /// <summary>
    /// Представляет контекст ORM для работы с базой данных.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    public class ORMContext<T> where T : class, new()
    {
        private readonly IDbConnection _dbConnection;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ORMContext{T}"/>.
        /// </summary>
        /// <param name="dbConnection">Соединение с базой данных.</param>
        public ORMContext(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Читает сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Сущность типа T или null, если сущность не найдена.</returns>
        public T ReadById(int id)
        {
            var properties = typeof(T).GetProperties();
            string idColumn = properties.Any(p => p.Name == "MovieId") ? "MovieId" : "Id";
            string idParameterName = properties.Any(p => p.Name == "MovieId") ? "@MovieId" : "@Id";
            string query = $"SELECT * FROM {typeof(T).Name}s WHERE {idColumn} = {idParameterName}";

            try
            {
                using (var command = _dbConnection.CreateCommand())
                {
                    command.CommandText = query;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = idParameterName;
                    parameter.Value = id;
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
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                _dbConnection.Close();
            }

            return null;
        }

        /// <summary>
        /// Читает все сущности из таблицы.
        /// </summary>
        /// <returns>Список сущностей типа T.</returns>
        public List<T> ReadAll()
        {
            List<T> results = new List<T>();
            string sql = $"SELECT * FROM {typeof(T).Name}s";

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
                // Логирование ошибки
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                _dbConnection.Close();
            }

            return results;
        }

        /// <summary>
        /// Создает новую сущность в таблице.
        /// </summary>
        /// <param name="entity">Сущность для создания.</param>
        /// <returns>Созданная сущность.</returns>
        public T Create(T entity)
        {
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id" && p.Name != "MovieId").ToList();
            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var parameterNames = string.Join(", ", properties.Select(p => "@" + p.Name));

            string sql = $"INSERT INTO {typeof(T).Name}s ({columnNames}) VALUES ({parameterNames}); SELECT SCOPE_IDENTITY();";

            try
            {
                using (var command = _dbConnection.CreateCommand())
                {
                    command.CommandText = sql;
                    foreach (var property in properties)
                    {
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = $"@{property.Name}";
                        parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                        command.Parameters.Add(parameter);
                    }

                    _dbConnection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        var idProperty = typeof(T).GetProperty("Id");
                        var movieIdProperty = typeof(T).GetProperty("MovieId");

                        if (idProperty != null)
                        {
                            idProperty.SetValue(entity, newId);
                        }
                        else if (movieIdProperty != null)
                        {
                            movieIdProperty.SetValue(entity, newId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                _dbConnection.Close();
            }

            return entity;
        }

        public T CreateMovieData(T entity)
        {
            var properties = typeof(T).GetProperties().ToList();

            // Исключаем свойства Id и MovieId из списка колонок и параметров для вставки, если они не имеют значений
            var columnNames = string.Join(", ", properties.Where(p => (p.Name != "Id" && p.Name != "MovieId") || (p.GetValue(entity) != null)).Select(p => p.Name));
            var parameterNames = string.Join(", ", properties.Where(p => (p.Name != "Id" && p.Name != "MovieId") || (p.GetValue(entity) != null)).Select(p => "@" + p.Name));

            string sql = $"INSERT INTO {typeof(T).Name}s ({columnNames}) VALUES ({parameterNames}); SELECT SCOPE_IDENTITY();";

            try
            {
                using (var command = _dbConnection.CreateCommand())
                {
                    command.CommandText = sql;
                    foreach (var property in properties)
                    {
                        if (property.Name == "Id" || property.Name == "MovieId")
                        {
                            // Пропускаем свойства Id и MovieId, если они не имеют значений
                            if (property.GetValue(entity) == null)
                            {
                                continue;
                            }
                        }

                        var parameter = command.CreateParameter();
                        parameter.ParameterName = $"@{property.Name}";
                        parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                        command.Parameters.Add(parameter);
                    }

                    _dbConnection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        var idProperty = typeof(T).GetProperty("Id");
                        var movieIdProperty = typeof(T).GetProperty("MovieId");

                        if (idProperty != null)
                        {
                            idProperty.SetValue(entity, newId);
                        }
                        else if (movieIdProperty != null)
                        {
                            movieIdProperty.SetValue(entity, newId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                _dbConnection.Close();
            }

            return entity;
        }



        /// <summary>
        /// Обновляет сущность в таблице.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <param name="entity">Сущность для обновления.</param>
        //public void Update(int id, T entity)
        //{
        //    var properties = typeof(T).GetProperties();
        //    var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

        //    string idColumn = properties.Any(p => p.Name == "MovieId") ? "MovieId" : "Id";
        //    string idParameterName = properties.Any(p => p.Name == "MovieId") ? "@MovieId" : "@Id";
        //    string sql = $"UPDATE {typeof(T).Name}s SET {setClause} WHERE {idColumn} = {idParameterName}";

        //    try
        //    {
        //        using (var command = _dbConnection.CreateCommand())
        //        {
        //            command.CommandText = sql;
        //            foreach (var property in properties)
        //            {
        //                var parameter = command.CreateParameter();
        //                parameter.ParameterName = $"@{property.Name}";
        //                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
        //                command.Parameters.Add(parameter);
        //            }
        //            var idParameter = command.CreateParameter();
        //            idParameter.ParameterName = idParameterName;
        //            idParameter.Value = id;
        //            command.Parameters.Add(idParameter);

        //            _dbConnection.Open();
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Логирование ошибки
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }
        //    finally
        //    {
        //        _dbConnection.Close();
        //    }
        //}

        //public void Update(int id, T entity)
        //{
        //    var properties = typeof(T).GetProperties();
        //    var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

        //    string idColumn = properties.Any(p => p.Name == "MovieId") ? "MovieId" : "Id";
        //    string idParameterName = properties.Any(p => p.Name == "MovieId") ? "@MovieId" : "@Id";
        //    string sql = $"UPDATE {typeof(T).Name}s SET {setClause} WHERE {idColumn} = {idParameterName}";

        //    try
        //    {
        //        using (var command = _dbConnection.CreateCommand())
        //        {
        //            command.CommandText = sql;
        //            foreach (var property in properties)
        //            {
        //                var parameter = command.CreateParameter();
        //                parameter.ParameterName = $"@{property.Name}";
        //                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
        //                command.Parameters.Add(parameter);
        //            }
        //            var idParameter = command.CreateParameter();
        //            idParameter.ParameterName = idParameterName;
        //            idParameter.Value = id;
        //            command.Parameters.Add(idParameter);

        //            _dbConnection.Open();
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Логирование ошибки
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }
        //    finally
        //    {
        //        _dbConnection.Close();
        //    }
        //}

        public void Update(T entity)
        {
            var properties = entity.GetType().GetProperties()
                .Where(p => p.Name != "Id" && p.Name != "MovieId");
            var values = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
            string query = $"UPDATE {typeof(T).Name}s SET {values} WHERE ";

            if (entity.GetType().GetProperty("Id") != null)
            {
                query += "Id = @Id";
            }
            else if (entity.GetType().GetProperty("MovieId") != null)
            {
                query += "MovieId = @MovieId";
            }

            using (var command = _dbConnection.CreateCommand())
            {
                command.CommandText = query;

                if (entity.GetType().GetProperty("Id") != null)
                {
                    var idParameter = command.CreateParameter();
                    idParameter.ParameterName = "@Id";
                    idParameter.Value = entity.GetType().GetProperty("Id").GetValue(entity);
                    command.Parameters.Add(idParameter);
                }
                else if (entity.GetType().GetProperty("MovieId") != null)
                {
                    var movieIdParameter = command.CreateParameter();
                    movieIdParameter.ParameterName = "@MovieId";
                    movieIdParameter.Value = entity.GetType().GetProperty("MovieId").GetValue(entity);
                    command.Parameters.Add(movieIdParameter);
                }

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
        /// Удаляет сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        public void Delete(int id)
        {
            var properties = typeof(T).GetProperties();
            string idColumn = properties.Any(p => p.Name == "MovieId") ? "MovieId" : "Id";
            string idParameterName = properties.Any(p => p.Name == "MovieId") ? "@MovieId" : "@Id";
            string sql = $"DELETE FROM {typeof(T).Name}s WHERE {idColumn} = {idParameterName}";

            try
            {
                using (var command = _dbConnection.CreateCommand())
                {
                    command.CommandText = sql;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = idParameterName;
                    parameter.Value = id;
                    command.Parameters.Add(parameter);

                    _dbConnection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                _dbConnection.Close();
            }
        }

        /// <summary>
        /// Маппинг данных из IDataReader в сущность типа T.
        /// </summary>
        /// <param name="reader">IDataReader для чтения данных.</param>
        /// <returns>Сущность типа T.</returns>
        private T Map(IDataReader reader)
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (reader[property.Name] != DBNull.Value)
                {
                    property.SetValue(obj, Convert.ChangeType(reader[property.Name], property.PropertyType));
                }
            }

            return obj;
        }

        /// <summary>
        /// Фильтрует сущности по предикату.
        /// </summary>
        /// <param name="predicate">Предикат для фильтрации.</param>
        /// <returns>Список сущностей, удовлетворяющих предикату.</returns>
        public List<T> Where(Expression<Func<T, bool>> predicate)
        {
            var sqlQuery = ExpressionParser<T>.BuildSqlQuery(predicate, singleResult: false);
            return ExecuteQueryMultiple(sqlQuery).ToList();
        }

        /// <summary>
        /// Выполняет запрос и возвращает одну сущность.
        /// </summary>
        /// <param name="query">SQL-запрос.</param>
        /// <returns>Сущность типа T или null, если сущность не найдена.</returns>
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
        /// Выполняет запрос и возвращает список сущностей.
        /// </summary>
        /// <param name="query">SQL-запрос.</param>
        /// <returns>Список сущностей типа T.</returns>
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
        /// Метод заглушка для фильтрации сущностей.
        /// </summary>
        /// <returns>Сущность типа T.</returns>
        public T Where()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получает первую сущность, удовлетворяющую предикату.
        /// </summary>
        /// <param name="predicate">Предикат для фильтрации.</param>
        /// <returns>Сущность типа T или null, если сущность не найдена.</returns>
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
    }
}
