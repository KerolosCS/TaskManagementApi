using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManagementApi.Filters
{
    public class EnvelopResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
           
            if (context.Result is JsonResult jsonResult &&
                jsonResult.Value?.GetType().GetProperty("success") != null)
            {
                await next();
                return;
            }

            switch (context.Result)
            {
                case ObjectResult objectResult:
                    context.Result = new JsonResult(new
                    {
                        success = objectResult.StatusCode < 400,
                        message = GetDefaultMessage(objectResult.StatusCode),
                        data = objectResult.Value
                    })
                    {
                        StatusCode = objectResult.StatusCode
                    };
                    break;

                case EmptyResult:
                    context.Result = new JsonResult(new
                    {
                        success = true,
                        message = "No content",
                        data = (object)null
                    })
                    {
                        StatusCode = 204
                    };
                    break;
            }

            await next();
        }

        private string GetDefaultMessage(int? statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                201 => "Created",
                400 => "Bad request",
                401 => "Unauthorized",
                404 => "Not found",
                500 => "Server error",
                _ => "Request processed"
            };
        }
    }
}