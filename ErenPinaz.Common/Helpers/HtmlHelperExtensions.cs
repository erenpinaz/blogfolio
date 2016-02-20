using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ErenPinaz.Common.Helpers
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Gets the display description for model
        /// </summary>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self,
            Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var description = metadata.Description;

            return MvcHtmlString.Create(description);
        }
    }
}