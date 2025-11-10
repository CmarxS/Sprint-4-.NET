namespace MottoMap.Middleware
{
    /// <summary>
    /// Middleware para validação de API Key
    /// </summary>
    public class ApiKeyMiddleware
    {
        private const string API_KEY_HEADER = "X-API-Key";
     private readonly RequestDelegate _next;
      private readonly IConfiguration _configuration;

     public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
      {
   _next = next;
       _configuration = configuration;
        }

     public async Task InvokeAsync(HttpContext context)
        {
            // Permitir acesso ao Swagger e Health Checks sem API Key
      if (context.Request.Path.StartsWithSegments("/swagger") ||
    context.Request.Path.StartsWithSegments("/health") ||
          context.Request.Path.Value == "/")
            {
     await _next(context);
   return;
      }

            // Verificar se o header contém a API Key
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey))
         {
         context.Response.StatusCode = 401;
   context.Response.ContentType = "application/json";
   await context.Response.WriteAsJsonAsync(new
     {
        error = "API_KEY_MISSING",
                  message = "API Key não fornecida. Adicione o header 'X-API-Key' na requisição.",
      timestamp = DateTime.UtcNow
    });
       return;
          }

            // Validar a API Key
       var apiKey = _configuration.GetValue<string>("ApiKey");
       if (!apiKey.Equals(extractedApiKey))
        {
     context.Response.StatusCode = 403;
        context.Response.ContentType = "application/json";
  await context.Response.WriteAsJsonAsync(new
     {
    error = "INVALID_API_KEY",
        message = "API Key inválida.",
    timestamp = DateTime.UtcNow
         });
    return;
         }

            await _next(context);
        }
    }
}
