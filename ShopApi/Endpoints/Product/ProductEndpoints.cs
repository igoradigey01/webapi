namespace ShopApi.Endpoints
{
    public static class ProductEndpoints
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/products", async context =>
            {
                // Get all todo items
                await context.Response.WriteAsJsonAsync(new { Message = "All todo items" });
            });
            app.MapGet("/products/{id}", async context =>
            {
                // Get one todo item
                await context.Response.WriteAsJsonAsync(new { Message = "One todo item" });
            });

            app.MapGet("/about", () => "About Page");
        }
    }
}


