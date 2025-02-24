using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;  // Espacio de nombres que contiene los DTOs (Data Transfer Objects) de la API
using ApiPeliculas.Repositorio.IRepositorio;  // Espacio de nombres que contiene la interfaz del repositorio de categorías
using AutoMapper;  // Espacio de nombres que proporciona AutoMapper para realizar conversiones de objetos
using Microsoft.AspNetCore.Mvc;  // Espacio de nombres de ASP.NET Core que permite la creación de controladores de API

namespace ApiPeliculas.Controllers
{
    // El decorador [ApiController] indica que esta clase es un controlador de API de ASP.NET Core
    [ApiController]
    // El decorador [Route("api/Categorias")] define la ruta base para este controlador. 
    // Todas las rutas dentro de este controlador estarán bajo 'api/Categorias'.
    [Route("api/Categorias")]
    public class CategoriasController : ControllerBase
    {
        // Declaración de una variable readonly para almacenar la referencia al repositorio de categorías.
        public readonly ICategoriaRepositorio _categoriaRepositorio;

        // Declaración de una variable readonly para almacenar la referencia a AutoMapper, que se usará para mapear entre entidades y DTOs.
        private readonly IMapper _mapper;

        // Constructor que recibe las dependencias necesarias: un repositorio de categorías y AutoMapper.
        public CategoriasController(ICategoriaRepositorio categoriaRepositorio, IMapper mapper)
        {
            // Asigna el repositorio recibido al campo _categoriaRepositorio.
            _categoriaRepositorio = categoriaRepositorio;

            // Asigna AutoMapper recibido al campo _mapper.
            _mapper = mapper;
        }

        #region GetCategorias
        // Método para manejar las solicitudes GET a la ruta 'api/Categorias'.
        // Este método retorna todas las categorías disponibles en la base de datos.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        public IActionResult GetCategorias()
        {
            // Obtiene la lista completa de categorías desde el repositorio.
            var listaCategorias = _categoriaRepositorio.GetCategorias();
            // Se crea una lista vacía de DTOs de categoría para almacenar las categorías mapeadas.
            var listaCategoriasDto = new List<CategoriaDto>();
            // Se recorre cada categoría obtenida del repositorio y se mapea a un DTO utilizando AutoMapper.
            foreach (var lista in listaCategorias)
            {
                // Se mapea cada categoría a un DTO y se agrega a la lista de DTOs.
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(lista));
            }
            // Se devuelve una respuesta HTTP 200 (Ok) con la lista de categorías DTOs.
            return Ok(listaCategoriasDto);
        }
        #endregion

        #region GetCategoria
        [HttpGet("{categoriaId:int}", Name = "GetCategoria" )]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
            // Obtiene la lista completa de categorías desde el repositorio.
            var itemCategoria = _categoriaRepositorio.GetCategoria(categoriaId);
            if (itemCategoria == null) {
                return NotFound();
            }
            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);
            // Se devuelve una respuesta HTTP 200 (Ok) con la lista de categorías DTOs.
            return Ok(itemCategoriaDto);
        }
        #endregion

        #region CrearCategoria
        [HttpPost] // Indica que esta acción manejará las solicitudes HTTP POST
        [ProducesResponseType(201, Type = typeof(CategoriaDto))] // Responde con código 201 y un objeto CategoriaDto si la creación es exitosa
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Responde con código 400 si hay un error de validación
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Responde con código 500 si ocurre un error en el servidor
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            // Verifica si el modelo recibido es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Si el modelo no es válido, retorna un error 400 con detalles de la validación, ModelState contiene los errores
            }

            // Verifica si el objeto recibido es nulo
            if (crearCategoriaDto == null)
            {
                return BadRequest(ModelState); // Si el objeto es nulo, retorna un error 400, ModelState contiene los errores
            }

            // Verifica si ya existe una categoría con el mismo nombre
            if (_categoriaRepositorio.ExisteCategoria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe"); // Agrega un error al modelo si la categoría ya existe
                return StatusCode(404, ModelState); // Retorna un error 404 con el mensaje de error
            }

            // Mapea el DTO recibido a una entidad de tipo Categoria
            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);

            // Intenta guardar la categoría en la base de datos a través del repositorio
            if (!_categoriaRepositorio.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState); // Si no se pudo guardar la categoría, retorna un error 500 con el mensaje de error
            }

            // Si todo es exitoso, responde con código 201 (Creado) y la URL donde se puede acceder al recurso recién creado.
            // 'CreatedAtRoute' genera una respuesta que incluye la URL del recurso creado y el objeto recién creado en el cuerpo de la respuesta
            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria); // Responde con la URL de la nueva categoría y los detalles de la misma
        }
        #endregion
        
        #region ActualizarPatchCategoria
        [HttpPatch("{categoriaId:int}", Name  = "ActualizarPatchCategoria")] // Indica que esta acción manejará las solicitudes HTTP POST
        [ProducesResponseType(201, Type = typeof(CategoriaDto))] // Responde con código 201 y un objeto CategoriaDto si la creación es exitosa
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Responde con código 400 si hay un error de validación
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Responde con código 500 si ocurre un error en el servidor
        public IActionResult ActualizarPatchCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            // Verifica si el modelo recibido es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Si el modelo no es válido, retorna un error 400 con detalles de la validación, ModelState contiene los errores
            }

            if (categoriaDto == null || categoriaId != categoriaDto.Id)
            {
                return BadRequest(ModelState); // Si el objeto es nulo, retorna un error 400, ModelState contiene los errores
            }

            // Mapea el DTO recibido a una entidad de tipo Categoria
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            // Intenta guardar la categoría en la base de datos a través del repositorio
            if (!_categoriaRepositorio.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState); // Si no se pudo guardar la categoría, retorna un error 500 con el mensaje de error
            }

            return NoContent();
        }
        #endregion


        #region BorrarCategoria
        [HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")] // Indica que esta acción manejará las solicitudes HTTP POST
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarCategoria(int categoriaId)
        {
            if (!_categoriaRepositorio.ExisteCategoria(categoriaId))
            {
                return NotFound();
            }

            var categoria = _categoriaRepositorio.GetCategoria(categoriaId);

            // Intenta guardar la categoría en la base de datos a través del repositorio
            if (!_categoriaRepositorio.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState); // Si no se pudo guardar la categoría, retorna un error 500 con el mensaje de error
            }

            return NoContent();
        }

        #endregion
    }
}