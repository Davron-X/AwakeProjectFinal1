using Awake_Data.Initializer;
using Microsoft.AspNetCore.Mvc;

namespace AwakeProject.Utility
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        public DbInitializerMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }
        public async Task InvokeAsync(HttpContext httpContext,[FromServices]IDbInitializer dbInitializer)
        {
            dbInitializer.Initialize();
            await requestDelegate.Invoke(httpContext);
        }
    }
}
