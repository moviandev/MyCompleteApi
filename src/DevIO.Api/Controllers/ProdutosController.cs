using AutoMapper;
using DevIO.Api.Dtos;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
  [Route("api/[controller]")]
  public class ProdutosController : MainController
  {
    private readonly IProdutoRepository _repository;
    private readonly IProdutoService _service;
    private readonly IMapper _mapper;
    private readonly INotificador _notifier;

    public ProdutosController(IProdutoRepository repository, IProdutoService service, IMapper mapper, INotificador notifier) : base(notifier)
    {
      _repository = repository;
      _service = service;
      _mapper = mapper;
      _notifier = notifier;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetAll()
    {
      return CustomResponse(_mapper.Map<IEnumerable<ProdutoDto>>(await _repository.ObterProdutosFornecedores()));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProdutoDto>> Get(Guid id)
    {
      var produtoDto = await GetProduct(id);

      if (produtoDto == null) return NotFound();

      return produtoDto;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      var produtoDto = await GetProduct(id);

      if (produtoDto == null) return NotFound();

      await _service.Remover(id);

      return CustomResponse();
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoDto>> Post(ProdutoDto produtoDto)
    {
      if (!ModelState.IsValid) return CustomResponse(ModelState);

      var imageName = $"{Guid.NewGuid()}_{produtoDto.Imagem}";

      if (!UploadFile(produtoDto.ImagemUpload, imageName)) return CustomResponse(produtoDto);

      produtoDto.Imagem = imageName;
      await _service.Adicionar(_mapper.Map<Produto>(produtoDto));

      return CustomResponse(produtoDto);
    }

    private bool UploadFile(string file, string fileName)
    {
      var imgDataByByteArr = Convert.FromBase64String(file);

      if (string.IsNullOrWhiteSpace(file))
      {
        NotifyError("Forneca uma imagem para esse produto");
        return false;
      }

      var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

      if (System.IO.File.Exists(filePath))
      {
        NotifyError("Já existe um arquivo com esse nome");
        return false;
      }

      System.IO.File.WriteAllBytes(filePath, imgDataByByteArr);

      return true;
    }

    private async Task<ProdutoDto> GetProduct(Guid id)
    {
      return _mapper.Map<ProdutoDto>(await _repository.ObterProdutoFornecedor(id));
    }
  }
}