using AutoMapper;
using DevIO.Api.Dtos;
using DevIO.Business.Models;

namespace DevIO.Api.Configurations
{
  public class AutoMapperConfiguration : Profile
  {
    public AutoMapperConfiguration()
    {
      CreateMap<Fornecedor, FornecedorDto>().ReverseMap();
      CreateMap<Produto, ProdutoDto>().ReverseMap();
      CreateMap<Endereco, EnderecoDto>().ReverseMap();
    }
  }
}