using Infrastructure.Data;
using MassTransit;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configurar MassTransit
builder.Services.AddMassTransit(config =>
{
    // Registrar el endpoint de consumidor
    config.AddConsumer<CrearLibroConsumer>();

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Configurar el endpoint de consumidor
        cfg.ReceiveEndpoint("crear_libro_queue", e =>
        {
            e.ConfigureConsumer<CrearLibroConsumer>(context);
        });
    });
});


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
