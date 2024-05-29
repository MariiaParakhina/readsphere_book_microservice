using BookService;
using Domains;
using dotenv.net;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
DependencyInjection.ConfigureServices(builder.Services);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
DependencyInjection.ConfigureServices(builder.Services);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel((context, options) =>
{
    var certPath = "./cert/certificate.pfx";
    options.ListenAnyIP(1026, listenOptions =>
    {
        listenOptions.UseHttps(certPath, "pass");
    });
});

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options =>
    {
        options.Events = new CertificateAuthenticationEvents
        {
            OnCertificateValidated = context =>
            {
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddCors((options) =>
{
    options.AddDefaultPolicy(
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


builder.Services.AddDbContext<IBookDbContext, BookDbContext>(options =>
{
    options.UseNpgsql(
        Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING"),
        b => b.MigrationsAssembly("BookService")
    );
});
    
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IBookDbContext>();

    if (!dbContext.Database.CanConnect())
    {
        throw new NotImplementedException("Not able to connect");
    }
    
    dbContext.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
} 

// Ensure that the middleware knows the HTTPS port
app.UseHttpsRedirection(); 
app.UseAuthorization();
app.UseRouting();
app.UseHsts();
app.UseAuthentication(); 
DotEnv.Load();
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");

    await next();
});
app.MapControllers();
app.Run();