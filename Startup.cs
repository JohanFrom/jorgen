using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using jorgen.ApplicationSettings;
using jorgen.Services.Abstract;
using jorgen.Services.Concrete;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Azure.Identity;

namespace jorgen
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        
            services.AddScoped<IJorgenService, JorgenService>();
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddScoped<IKeyVaultService, KeyVaultService>();

            services.AddOptions<ApplicationOptions>().Bind(Configuration.GetSection("ApplicationOptions")).ValidateDataAnnotations();
            services.AddSingleton(provider => provider.GetService<IOptionsMonitor<ApplicationOptions>>().CurrentValue);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Jorgen API",
                    Version = "v1",
                    Description = "More info here",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "More Info Here",
                        Email = "Mail",
                        Url = new Uri("https://www.noroff.no/accelerate"),

                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

            }
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jorgen API v1");
                //c.RoutePrefix = string.Empty;
            });

            SecretClientOptions options = new()
            {
                Retry =
                {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };

            var client = new SecretClient(new Uri("https://JorgenKeyVault.vault.azure.net/"), new DefaultAzureCredential(), options);

            KeyVaultSecret secret = client.GetSecret("WeatherApiKey");

            string secretValue = secret.Value;

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            DefaultFilesOptions DefaultFile = new();
            DefaultFile.DefaultFileNames.Clear();
            DefaultFile.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(DefaultFile);
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }

}
