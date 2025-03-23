using MassTransit;
using Domain.Entities;
using Domain.Persistencia;
using Application;

public class CrearLibroConsumer : IConsumer<CrearLibroMessage>
{
    private readonly ContextoLibreria _contexto;

    public CrearLibroConsumer(ContextoLibreria contexto)
    {
        _contexto = contexto;
    }

    public async Task Consume(ConsumeContext<CrearLibroMessage> context)
    {
        var message = context.Message;

        var libro = new LibreriaMaterial
        {
            LibreriaMateriaID = Guid.NewGuid(),
            Titulo = message.Titulo,
            FechaPublicacion = message.FechaPublicacion,
            AutorLibro = message.AutorLibro
        };

        await _contexto.LibreriaMaterial.AddAsync(libro);
        await _contexto.SaveChangesAsync();

        Console.WriteLine($"Libro creado: {libro.Titulo}");
    }
}