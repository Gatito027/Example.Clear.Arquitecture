using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Persistencia;

namespace Infrastructure.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuración de controladores y validaciones usando FluentValidation
            services.AddControllers()
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            // Configuración de DbContext con conexión a SQL Server
            services.AddDbContext<ContextoLibreria>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Configuración de servicios de CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins("https://example.com", "https://another-example.com")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });

                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            // Registro de MediatR como servicio
            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

            // Registro de AutoMapper para consultas
            services.AddAutoMapper(typeof(Consulta.Manejador));

            return services;
        }
    }
}
