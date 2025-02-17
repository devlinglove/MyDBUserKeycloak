using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using WebApplicationTokenSub.Handlers;
using WebApplicationTokenSub.Options;
using WebApplicationTokenSub.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<KeyCloakAuthorizationDelegatingHandler>();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<CreateUserService>();

builder.Services.Configure<KeycloakOptions>(builder.Configuration.GetSection("Keycloak"));

builder.Services.AddHttpClient<CreateUserService>(httpClient =>
{
    httpClient.BaseAddress = new Uri("http://localhost:8080/realms/my-realm/protocol/openid-connect/");
})
.AddHttpMessageHandler<KeyCloakAuthorizationDelegatingHandler>();

// KeyCloak
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.Authority = builder.Configuration["Jwt:Authority"];
    o.Audience = builder.Configuration["Jwt:Audience"];
    o.RequireHttpsMetadata = false;
    o.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();

            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";

            // Debug only for security reasons
            // return c.Response.WriteAsync(c.Exception.ToString());

            return c.Response.WriteAsync("An error occured processing your authentication.");
        }
    };
});

// Cross-Origin 
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowOrigin",
            builder => builder.WithOrigins("http://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Added
app.UseCors("AllowOrigin");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
