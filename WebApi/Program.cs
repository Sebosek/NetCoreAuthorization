using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

const string AUTH_ID = "Bearer";
const string DESCRIPTION =
    "JWT Authorization header using the Bearer scheme; use only the JWT token (without the keyword Bearer).";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://demo.duendesoftware.com/";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = AUTH_ID,
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
    options.AddSecurityDefinition(AUTH_ID, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://demo.duendesoftware.com/connect/authorize"),
                TokenUrl = new Uri("https://demo.duendesoftware.com/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID scope" },
                    { "profile", "Profile scope" },
                    { "email", "Email claim" },
                },
            },
        },
    });
    // Alternatively
    // options.AddSecurityDefinition(AUTH_ID, new OpenApiSecurityScheme
    // {
    //     Description = DESCRIPTION,
    //     Type = SecuritySchemeType.Http,
    //     Scheme = "bearer"
    // });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("interactive.confidential");
        options.OAuthClientSecret("secret");
        options.OAuthUsePkce();
        options.OAuthScopes("openid", "profile", "email");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();