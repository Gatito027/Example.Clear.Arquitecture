using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Application;
using MassTransit;

namespace Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class LibroMaterialController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        public LibroMaterialController(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); // Validación de dependencia inyectada
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        // Endpoint para crear un nuevo libro
        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> Crear([FromBody] Nuevo.Ejecuta data)
        {
            if (data == null)
            {
                return BadRequest("Los datos del libro no pueden ser nulos");
            }

            try
            {
                var resultado = await _mediator.Send(data);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el libro: {ex.Message}");
            }
        }

        // Endpoint para obtener la lista de libros
        [HttpGet("lista")]
        public async Task<ActionResult<List<LibroMaterialDto>>> GetLibros()
        {
            try
            {
                var resultado = await _mediator.Send(new Consulta.Ejecuta());
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los libros: {ex.Message}");
            }
        }

        // Endpoint para obtener un libro específico
        [HttpGet("detalle/{id}")]
        public async Task<ActionResult<LibroMaterialDto>> GetLibroUnico(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("El ID del libro no es válido");
            }

            try
            {
                var resultado = await _mediator.Send(new ConsultaFiltro.LibroUnico
                {
                    LibroId = id
                });
                return Ok(resultado);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró un libro con el ID: {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el libro: {ex.Message}");
            }
        }

        // Endpoint para crear un libro con el patrón Saga
        [HttpPost("crear-saga")]
        public async Task<IActionResult> CrearLibroSaga([FromBody] CrearLibroMessage request)
        {
            if (request == null)
            {
                return BadRequest("La solicitud no puede ser nula");
            }

            try
            {
                await _publishEndpoint.Publish(request);
                return Ok("Libro creado correctamente. El procesamiento está en curso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la creación del libro: {ex.Message}");
            }
        }
    }
}