using AutoMapper;
using DevIO.Api.Dtos;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
  [Route("api/[controller]")]
  public class FornecedoresController : MainController
  {
    private readonly IFornecedorRepository _context;
    private readonly IFornecedorService _service;
    private readonly IMapper _mapper;
    private readonly INotificador _notifier;
    private readonly IEnderecoRepository _enderecoRepository;

    public FornecedoresController(IFornecedorRepository context, IMapper mapper, IFornecedorService service, INotificador notifier, IEnderecoRepository enderecoRepository) : base(notifier)
    {
      _context = context;
      _mapper = mapper;
      _service = service;
      _notifier = notifier;
      _enderecoRepository = enderecoRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FornecedorDto>>> GetAll()
    {
      var fornecedores = _mapper.Map<IEnumerable<FornecedorDto>>(await _context.ObterTodos());
      return Ok(fornecedores);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FornecedorDto>> Get(Guid id)
    {
      var fornecedor = await GetFornecedorProductsAndAddressById(id);

      if (fornecedor == null) return NotFound();

      return fornecedor;
    }

    [HttpPost]
    public async Task<ActionResult<FornecedorDto>> Post(FornecedorDto fornecedorDto)
    {
      if (!ModelState.IsValid) return CustomResponse(ModelState);

      await _service.Adicionar(_mapper.Map<Fornecedor>(fornecedorDto));

      return CustomResponse(fornecedorDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FornecedorDto>> Put(Guid id, [FromBody] FornecedorDto fornecedorDto)
    {
      if (id != fornecedorDto.Id) return BadRequest();

      if (!ModelState.IsValid) return CustomResponse(ModelState);

      await _service.Atualizar(_mapper.Map<Fornecedor>(fornecedorDto));

      return CustomResponse(fornecedorDto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<FornecedorDto>> Delete(Guid id)
    {
      var fornecedor = await GetFornecedorById(id);

      if (fornecedor == null) return NotFound();

      await _service.Remover(id);

      return CustomResponse();
    }

    [HttpGet("get-address/{id:guid}")]
    public async Task<EnderecoDto> GetAddress(Guid id)
    {
      return _mapper.Map<EnderecoDto>(await _enderecoRepository.ObterPorId(id));
    }

    [HttpPut("update-address/{id:guid}")]
    public async Task<IActionResult> UpdateAddress(Guid id, EnderecoDto enderecoDto)
    {
      if (id != enderecoDto.Id)
      {
        NotifyError("O id informado não é o mesmo que foi passado na query");
        return CustomResponse(enderecoDto);
      }

      if (!ModelState.IsValid) return CustomResponse(ModelState);

      await _service.AtualizarEndereco(_mapper.Map<Endereco>(enderecoDto));

      return CustomResponse(enderecoDto);
    }

    private async Task<FornecedorDto> GetFornecedorProductsAndAddressById(Guid id)
    {
      return _mapper.Map<FornecedorDto>(await _context.ObterFornecedorProdutosEndereco(id));
    }

    private async Task<FornecedorDto> GetFornecedorById(Guid id)
    {
      return _mapper.Map<FornecedorDto>(await _context.ObterFornecedorEndereco(id));
    }
  }
}