using BookService;
using Domains;
using dotenv.net;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
DependencyInjection.ConfigureServices(builder.Services);

DependencyInjection.ConfigureServices(builder.Services);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(options =>
{
    var certPath = "./cert/certificate.pfx";
    options.ListenAnyIP(443, listenOptions => { listenOptions.UseHttps(certPath, "pass"); });
});

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options =>
    {
        options.Events = new CertificateAuthenticationEvents
        {
            OnCertificateValidated = _ => { return Task.CompletedTask; }
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

    if (!dbContext.Database.CanConnect()) throw new NotImplementedException("Not able to connect");

    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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