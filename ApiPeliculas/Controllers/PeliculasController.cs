using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/Peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _peliculaRepositorio;
        private readonly IMapper _mapper;

        public PeliculasController(IPeliculaRepositorio peliculaRepositorio, IMapper mapper)
        {
            _peliculaRepositorio = peliculaRepositorio;
            _mapper = mapper;
        }

        #region Consultas
        #region GetPeliculas
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _peliculaRepositorio.GetPeliculas();
            var listaPeliculasDto = new List<PeliculaDto>();
            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(lista));
            }
            return Ok(listaPeliculasDto);
        }
        #endregion

        #region GetPelicula
        [AllowAnonymous]
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int id)
        {
            var pelicula = _peliculaRepositorio.GetPelicula(id);
            if (pelicula == null)
                return NotFound();

            var peliculaDto = _mapper.Map<PeliculaDto>(pelicula);
            return Ok(peliculaDto);
        }
        #endregion

        #region BuscarPelicula
        [AllowAnonymous]
        [HttpGet("Buscar")]
        public IActionResult BuscarPelicula(string nombre)
        {
            try
            {
                var pelicula = _peliculaRepositorio.BuscarPelicula(nombre.Trim());
                if (pelicula.Any())
                {
                    return Ok(pelicula);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando la informacion.");
            }

        }
        #endregion

        #region GetPeliculasEnCategoria
        [AllowAnonymous]
        [HttpGet("Categoria/{categoriaId}")]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var peliculas = _peliculaRepositorio.GetPeliculasEnCategoria(categoriaId);
            if (peliculas == null || peliculas.Count == 0)
                return NotFound();

            var peliculasDto = _mapper.Map<ICollection<PeliculaDto>>(peliculas);
            return Ok(peliculasDto);
        }
        #endregion
        #endregion

        #region Operaciones CRUD
        #region CrearPelicula
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPelicula([FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (peliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_peliculaRepositorio.ExistePelicula(peliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La pelicula ya existe");
                return StatusCode(404, ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);

            if (!_peliculaRepositorio.CrearPelicula(pelicula))
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear la película.");

            return CreatedAtRoute("GetPelicula", new { id = pelicula.Id }, pelicula);
        }
        #endregion

        #region ActualizarPatchPelicula
        [Authorize(Roles = "admin")]
        [HttpPut("{peliculaId:int}")]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (peliculaDto == null)
                return BadRequest(ModelState);

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);

            if (!_peliculaRepositorio.ActualizarPelicula(pelicula))
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar la película.");

            return NoContent();
        }
        #endregion

        #region EliminarPelicula
        [Authorize(Roles = "admin")]
        [HttpDelete("{peliculaId:int}", Name = "EliminarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarCategoria(int peliculaId)
        {
            if (!_peliculaRepositorio.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            var pelicula = _peliculaRepositorio.GetPelicula(peliculaId);

            if (!_peliculaRepositorio.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando la película {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        #endregion
        #endregion
    }
}
