using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// .NET Authorize middleware replaces standard claims with Microsoft proprietary claim types,
// to disable claim type mapping, simple clear the map.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("cookie")
    .AddGitHub("github", options =>
    {
        options.ClientId = "e86***d8e";
        options.ClientSecret = "71e***f30";
        options.UserEmailsEndpoint = "https://api.github.com/user/emails";
        options.Scope.Add("user:email");
        options.SaveTokens = true;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://demo.duendesoftware.com/";
        options.ClientId = "interactive.confidential";
        options.ClientSecret = "secret";
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("offline_access");
        options.UsePkce = true;
        options.AutomaticRefreshInterval = TimeSpan.FromMinutes(60);
        options.ResponseType = "code";
        options.SaveTokens = true;
        // Client doesn't support Implicit flow; claim must be loaded from User info
        options.GetClaimsFromUserInfoEndpoint = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();