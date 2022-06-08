using AutoMapper;
using DevIO.Api.Dtos;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
  [Route("api/[controller]")]
  public class FornecedoresController : MainController
  {
    private readonly IFornecedorRepository _context;
    private readonly IMapper _mapper;

    public FornecedoresController(IFornecedorRepository context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ActionResult<IEnumerable<FornecedorDto>>> GetAll()
    {
      var fornecedores = _mapper.Map<IEnumerable<FornecedorDto>>(await _context.ObterTodos());
      return Ok(fornecedores);
    }
  }
}