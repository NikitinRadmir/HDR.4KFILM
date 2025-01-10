using System.Linq.Expressions;

namespace MyORMLibrary;

/// <summary>
/// Представляет статический класс для парсинга выражений и построения SQL-запросов.
/// </summary>
/// <typeparam name="T">Тип сущности.</typeparam>
public static class ExpressionParser<T>
{
    /// <summary>
    /// Парсит выражение и возвращает его в виде строки.
    /// </summary>
    /// <param name="expression">Выражение для парсинга.</param>
    /// <returns>Строковое представление выражения.</returns>
    internal static string ParseExpression(Expression expression)
    {
        if (expression is BinaryExpression binary)
        {
            // Разбираем выражение на составляющие
            var left = ParseExpression(binary.Left);  // Левая часть выражения
            var right = ParseExpression(binary.Right); // Правая часть выражения
            var op = GetSqlOperator(binary.NodeType);  // Оператор (например, > или =)
            return $"({left} {op} {right})";
        }
        else if (expression is MemberExpression member)
        {
            return member.Member.Name; // Название свойства
        }
        else if (expression is ConstantExpression constant)
        {
            return FormatConstant(constant.Value); // Значение константы
        }

        throw new NotSupportedException($"Unsupported expression type: {expression.GetType().Name}");
    }

    /// <summary>
    /// Возвращает SQL-оператор для заданного типа узла выражения.
    /// </summary>
    /// <param name="nodeType">Тип узла выражения.</param>
    /// <returns>SQL-оператор.</returns>
    private static string GetSqlOperator(ExpressionType nodeType)
    {
        return nodeType switch
        {
            ExpressionType.Equal => "=",
            ExpressionType.GreaterThan => ">",
            ExpressionType.LessThan => "<",
            ExpressionType.AndAlso => "AND",
            ExpressionType.Or => "OR",
            _ => throw new NotSupportedException($"Unsupported operator: {nodeType}")
        };
    }

    /// <summary>
    /// Форматирует значение константы для использования в SQL-запросе.
    /// </summary>
    /// <param name="value">Значение константы.</param>
    /// <returns>Отформатированное значение константы.</returns>
    private static string FormatConstant(object value)
    {
        return value is string ? $"'{value}'" : value.ToString();
    }

    /// <summary>
    /// Строит SQL-запрос на основе предиката.
    /// </summary>
    /// <param name="predicate">Предикат для построения запроса.</param>
    /// <param name="singleResult">Флаг, указывающий, ожидается ли один результат.</param>
    /// <returns>SQL-запрос.</returns>
    internal static string BuildSqlQuery<T>(Expression<Func<T, bool>> predicate, bool singleResult)
    {
        var query = $"SELECT * FROM Users WHERE {ParseExpression(predicate.Body)}";
        return query;
    }
}
