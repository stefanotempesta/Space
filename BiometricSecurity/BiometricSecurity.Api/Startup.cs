using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiometricSecurity.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BiometricSecurity.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authorizationRequirements = new List<IAuthorizationRequirement>
            {
                new FaceRecognitionRequirement(confidence: 0.9),
                new BodyRecognitionRequirement(confidence: 0.9),
                new FaceRecognitionRequirement(confidence: 0.9)
            };

            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy("AuthorizedUser", policy => policy.Requirements = authorizationRequirements);
                })
                .AddRouting()
                .AddMvc();

            //services.AddSingleton<IAuthorizationHandler, BiometricRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, FaceRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, BodyRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, VoiceRequirementHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
