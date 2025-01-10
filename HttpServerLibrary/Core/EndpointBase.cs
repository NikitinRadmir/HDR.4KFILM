using HttpServerLibrary.Core;
using HttpServerLibrary.Core.HttpResponse;

namespace HttpServerLibrary
{

    /// <summary>
    /// Абстрактный класс от которого наследуются все Enpoints
    /// Определяет набор методов которыми обладает каждый Enpoint
    /// Реализует методы для работы с классами реализующими <see cref="IHttpResponseResult"/>
    /// </summary>
    public abstract class EndpointBase 
    {
        /// <summary>
        /// Контекст текущего запроса
        /// Иеет кастомный тип <see cref="HttpRequestContext"/>
        /// </summary>
        protected HttpRequestContext Context { get; private set; }

        /// <summary>
        /// Метод для инициализации контекста
        /// </summary>
        /// <param name="context">Контекст текущего запроса</param>
        internal void SetContext(HttpRequestContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Метод для инициализации класса <see cref="HtmlResult"/> с параметром
        /// </summary>
        /// <param name="html">Html текст для разворачивания в <see cref="HtmlResult"/></param>
        /// <returns>возвращает объект реализующий <see cref="IHttpResponseResult"/></returns>
        protected IHttpResponseResult Html(string html) => new HtmlResult(html);

        protected IHttpResponseResult Json(object data) => new JsonResult(data);
        

        protected IHttpResponseResult Redirect(string location) => new RedirectResult(location);
    }
}