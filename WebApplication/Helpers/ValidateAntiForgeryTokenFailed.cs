using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GalleryWebApplication.Helpers
{
    // Filtr wykonany w przypadku błędnego tokneta zabepiczeającego formularz.
    public class ValidateAntiForgeryTokenFailed : IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context) { }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is IAntiforgeryValidationFailedResult)
            {
                context.Result = new RedirectResult("/error");
            }
        }
    }
}
