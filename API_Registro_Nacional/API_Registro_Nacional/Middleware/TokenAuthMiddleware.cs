namespace API_Registro_Nacional.Middleware
{
    public class TokenAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private const string TOKEN = "123Blitz";

        public TokenAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var token) || token != TOKEN)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: Token invalid or null.");
                return;
            }

            await _next(context);
        }
    }
}
