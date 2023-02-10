namespace BlogAPI.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddErrorHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
