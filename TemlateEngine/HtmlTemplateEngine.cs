using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;

namespace TemlateEngine;
/// <summary>
/// Синтаксис шаблонизатора
/// {{Value}} выделение переменных
/// {%if<Условие>%} Действие если условие истинно {%else%} действие если условие ложно {%/if%}
/// {%for% ITEM %in% COLLECTION}Действия с ITEM {%/for%}
/// </summary>
public class HtmlTemplateEngine : IHtmlTemplateEngine
{
    // var template = "Hello, {name}! How are you?"
    // var data = Bob

    public string Render(string template, string str, string data)
    {
        return template.Replace(str, data);
    }
    public string Render(FileInfo fileInfo, object obj)
    {
        var templatePath = fileInfo.FullName;
        if (File.Exists(templatePath))
        {
            return Render(template: File.ReadAllText(templatePath), obj);
        }
        else
        {
            Console.WriteLine($"File {templatePath} not found");
            throw new FileNotFoundException($"File {templatePath} not found"); // я думаю эти ошибки надо отлавливать там где мы будем вызывать Render
        }
    }

    public string Render(Stream stream, object obj)
    {
        if (stream.CanRead)
        {
            var content = new StreamReader(stream).ReadToEnd();
            return Render(content, obj);
        }
        else
        {
            throw new InvalidOperationException("Stream can't be read");
        }
    }

    public string Render(string template, object obj)
    {
        // Идет по темплейту, вытаскивает все требуемые переменные, если видит if разбивает его на части, по телу if так же проходится
        // анализирует его
        // потом берет данные из модельки obj и распихивает их по запрашиваемым переменным
        // возвращает результат собранный из данных модельки
        var properties = obj.GetType().GetProperties();
        var result = template;
        try
        {
            result = HandleLoops(result, properties, obj);
            result = HandleConditionals(result, properties, obj);
            result = HandleVariables(result, obj);
            // result = HandleLoops(result,properties, obj);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine(e.Message);
            return e.Message;
        }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e.Message);
        //     return "There was an unhandled exception while Rendering";
        // }
        //result = HandleVariables(result, obj);
        return result;
    }

    // public string HandleVariables(string template, object obj)
    // {
    //     if (obj is null) throw new ArgumentNullException("obj is null");
    //     var type = obj.GetType();
    //     var result = template;
    //     var fields = type.GetProperties(BindingFlags.Public|BindingFlags.Instance);
    //     foreach (var prop in fields)
    //     {
    //         var repStr = "{{" + prop.Name.ToString() + "}}";
    //         var value = prop.GetValue(obj).ToString();
    //         result = result.Replace(repStr, value);
    //     }
    //
    //     return result;
    // }
    public string HandleVariables(string template, object obj)
    {
        if (obj is null) throw new ArgumentNullException("obj is null");

        var result = template;
        var regex = new Regex(@"{{(.*?)}}");

        result = regex.Replace(result, match =>
        {
            var propertyPath = match.Groups[1].Value; // Например, "Genre1.Name"
            var value = GetNestedPropertyValue(obj, propertyPath);
            return value ?? match.Value; // Если значение не найдено, оставить как есть
        });

        return result;
    }

    private string GetNestedPropertyValue(object obj, string propertyPath)
    {
        if (obj == null || string.IsNullOrWhiteSpace(propertyPath)) return null;

        var currentObject = obj;
        var properties = propertyPath.Split('.'); // Разбиваем "Genre1.Name" на ["Genre1", "Name"]
        foreach (var property in properties)
        {
            if (currentObject == null) return null;

            var type = currentObject.GetType();
            var propertyInfo = type.GetProperty(property, BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null) return null;

            currentObject = propertyInfo.GetValue(currentObject);
        }

        return currentObject?.ToString(); // Возвращаем значение свойства или null
    }

    // <{{Value}}{<<operator>>}{{Value}}>
    private string HandleConditionals(string template, PropertyInfo[] properties, object model)
    {
        if (model is null) throw new ArgumentNullException("model is null");
        var pattern = @"{%if<(.+?)>%}(.*?)({%else%}(.*?))?{%/if%}";
        //var pattern = @"{%if<.*?>%}(?:(?<Open>{%if<.*?>%})|(?<-Open>{%/if%})|.*?)*(?(Open)(?!)){%/if%}";
        var regex = new Regex(pattern, RegexOptions.Singleline);
        return new string(regex.Replace(template, match =>
        {
            var conditional = match.Groups[1].Value.Trim();
            var content = match.Groups[2].Value.Trim(); //трим опционально можно убрать
            var elseContent = match.Groups[3].Success ? match.Groups[4].Value.Trim() : null;
            //var property = properties.FirstOrDefault(p => p.Name == conditional);
            //if (property is not null && property.GetValue(model) is bool value && value)
            if (ProcessCondition(conditional, properties, model))
            {
                return Render(content, model);
            }
            if (elseContent != null)
            {
                return Render(elseContent, model);
            }
            return String.Empty;
        }));
    }
    //dt table optional 
    // прокидывать имя объекта, чтобы от замены избавиться
    // 
    private bool ProcessCondition(string condition, PropertyInfo[] properties, object model)
    {
        if (model is null) throw new ArgumentNullException("model is null");
        if (properties is null || properties.Length == 0) throw new ArgumentNullException("model properties are null");
        foreach (var property in properties)
        {
            var value = property.GetValue(model) is not bool ?
                $"'{property.GetValue(model)}'" : property.GetValue(model).ToString();
            var pattern = $@"{{{{{property.Name}}}}}";
            condition = Regex.Replace(condition, pattern, value, RegexOptions.IgnoreCase);
        }


        condition = condition.Replace("==", "=")
            .Replace("&&", "AND")
            .Replace("||", "OR")
            .Replace("!=", "<>");
        try
        {
            var result = (bool)new System.Data.DataTable().Compute(condition, string.Empty);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

    }
    // private string HandleLoops(string template, PropertyInfo[] properties, object model)
    // {
    //     var pattern = @"{%for%(.+?)%in%(.+?)}(.*?){%/for%}";
    //     var regexp = new Regex(pattern, RegexOptions.Singleline);
    //     return new string(regexp.Replace(template, match =>
    //     {
    //         var item = match.Groups[1].Value.Trim();
    //         var collection = match.Groups[2].Value.Trim();
    //         var content = match.Groups[3].Value;
    //         var values = properties.FirstOrDefault(p => p.Name == collection).GetValue(model);
    //         if (values is IEnumerable)
    //         {
    //             var loopResult = new StringBuilder();
    //           foreach (var value in (IEnumerable)(values))
    //           {
    //               // var subTemplate = item.GetType().GetProperties().Length > 0
    //               //     ? content.Replace("{{" + item + ".", "{{")
    //               //     : content.Replace($"{{{{{item}}}}}", value.ToString());
    //               var subTemplate = content.Replace("{{" + item + ".", "{{");
    //               if (value.GetType().IsPrimitive || value is string)
    //               {
    //                   //var index = content.IndexOf($"{{{{{item}}}}}");
    //                   string subTemplate1 = content;
    //                   subTemplate1 = content.Replace($"{{{{{item}}}}}", value.ToString());
    //                   loopResult.Append(Render(subTemplate1, model));
    //                   continue;
    //               }
    //               loopResult.Append(Render(subTemplate, value));
    //           }
    //
    //           return loopResult.ToString();
    //         }
    //         return string.Empty;
    //     }));
    // }

    // тестю  РАБОТАЕТ МОЖНО В ПРОД
    private string HandleLoops(string template, PropertyInfo[] properties, object model)
    {
        var pattern = @"{%for%(.+?)%in%(.+?)}(.*?){%/for%}";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        return regex.Replace(template, match =>
        {
            var itemName = match.Groups[1].Value.Trim(); // Например, "Book"
            var collectionName = match.Groups[2].Value.Trim(); // Например, "Books"
            var content = match.Groups[3].Value; // Шаблон для каждого элемента

            // Найти коллекцию в свойствах модели
            var collectionProperty = properties.FirstOrDefault(p => p.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase));
            if (collectionProperty == null)
            {
                throw new ArgumentException($"Collection '{collectionName}' not found in model.");
            }

            // Получить значение коллекции
            var collection = collectionProperty.GetValue(model) as IEnumerable;
            if (collection == null)
            {
                throw new ArgumentException($"Property '{collectionName}' is not a valid collection.");
            }

            var loopResult = new StringBuilder();

            foreach (var value in collection)
            {
                if (value == null) continue;

                string renderedItem;

                if (value.GetType().IsPrimitive || value is string)
                {
                    // Для простых типов (например, string) заменяем {{Book}} на значение
                    renderedItem = content.Replace($"{{{{{itemName}}}}}", value.ToString());
                }
                else
                {
                    // Для объектов заменяем {{Book.Property}} на значения их свойств
                    var itemTemplate = content.Replace("{{" + itemName + ".", "{{");
                    renderedItem = Render(itemTemplate, value);
                }

                loopResult.Append(renderedItem);
            }

            return loopResult.ToString();
        });
    }
    private bool CheckStringInLoop(string content, string check)
    {
        if (content.IndexOf("{%for%") == -1) return false;
        return content.IndexOf("{%for%") < content.IndexOf(check) &&
               content.IndexOf("{%/for%}") > content.IndexOf(check);
    }

    private bool CheckStringInCond(string content, string check)
    {
        if (content.IndexOf("{%if") == -1) return false;
        return content.IndexOf("{%if") < content.IndexOf(check) &&
               content.IndexOf("{%/if%}") > content.IndexOf(check);
    }

    // private string HandleLoops(string template, PropertyInfo[] properties, object model)
    // {
    //     var pattern = @"{%for%(.+?)%in%(.+?)}(.*?){%/for%}";
    //     var regex = new Regex(pattern, RegexOptions.Singleline);
    //     return regex.Replace(template, match =>
    //     {
    //         var item = match.Groups[1].Value.Trim(); // Имя переменной (например, Book)
    //         var collectionName = match.Groups[2].Value.Trim(); // Имя свойства коллекции (например, Books)
    //         var content = match.Groups[3].Value; // Шаблон для каждого элемента
    //     
    //         var property = properties.FirstOrDefault(p => p.Name == collectionName);
    //         if (property?.GetValue(model) is IEnumerable collection)
    //         {
    //             var loopResult = new StringBuilder();
    //             foreach (var element in collection)
    //             {
    //                 var elementTemplate = content.Replace($"{{{{{item}}}}}", element.ToString());
    //                 loopResult.Append(elementTemplate);
    //             }
    //             return loopResult.ToString();
    //         }
    //         return string.Empty;
    //     });
    // }

    void exampleException()
    {
        try
        {

            throw new InvalidOperationException("hjhkhjhjkhhk");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {

        }
    }

}
