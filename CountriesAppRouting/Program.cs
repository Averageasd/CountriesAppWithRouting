using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();
Dictionary<int, string> idCountryDict = new Dictionary<int, string>
{
    {1,"United States"},
    {2,"Canada"},
    {3,"United Kingdom"},
    {4,"India"},
    {5,"Japan"},

};
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("country", async (HttpContext context) =>
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (KeyValuePair<int, string> idCountryPair in idCountryDict)
        {
            stringBuilder.Append(idCountryPair.Key).Append(", ").Append(idCountryDict[idCountryPair.Key]).Append('\n');
        }
        await context.Response.WriteAsync(stringBuilder.ToString());
    });
    endpoints.MapGet("country/{id:int}", async (HttpContext context) =>
    {
        RouteValueDictionary keyValuePairs = context.Request.RouteValues;
        int countryId = Convert.ToInt32(keyValuePairs["id"]);
        if (countryId <= 100)
        {
            if (countryId < 0 || countryId > 5)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("No country");
            }
            else
            {
                await context.Response.WriteAsync(idCountryDict[countryId]);
            }
            return;
        }
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("The CountryID should be between 1 and 100");
    });
});

app.MapGet("/", async (HttpContext context) =>
{
    await context.Response.WriteAsync("fallback route");
});

app.Run();
