using FinanceiroPontoNet.Application.Boletos;
using FinanceiroPontoNet.Application.Boletos.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FinanceiroPontoNet.Web.Controllers.Boletos
{
    [Route("boletos")]
    public class BoletoController : ControllerBase
    {
        private readonly IBoletoService _service;
        private readonly ILogger<BoletoController> _logger;

        public BoletoController(IBoletoService service, ILogger<BoletoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo boleto.
        /// </summary>
        /// <param name="dto">Objeto contendo os dados necessários para a criação do boleto.</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBoletoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating a new boleto");
            var boleto = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { id = boleto.Id }, boleto);
        }

        /// <summary>
        /// Busca um boleto específico pelo seu id.
        /// </summary>
        /// <param name="id">O código do boleto a ser buscado.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation("Fetching boleto with ID: {id}", id);
            var boleto = await _service.GetAsync(id);
            return Ok(boleto);
        }

        /// <summary>
        /// Retorna uma lista paginada com todos os boletos cadastrados.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching boletos.");
            var boletos = await _service.GetAllAsync();
            return Ok(boletos);
        }

        /// <summary>
        /// Atualiza as informações de um boleto existente.
        /// </summary>
        /// <param name="id">O ID do boleto a ser atualizado.</param>
        /// <param name="dto">Objeto contendo os dados atualizados do boleto.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BoletoDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("The route ID does not match the request body ID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating boleto with ID: {dto.Id}", id);
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// Exclui um boleto.
        /// </summary>
        /// <param name="id">O ID do boleto a ser excluído.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting boleto with ID: {id}", id);
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
