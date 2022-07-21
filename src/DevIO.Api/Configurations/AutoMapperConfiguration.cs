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
      CreateMap<Endereco, EnderecoDto>().ReverseMap();
      CreateMap<ProdutoDto, Produto>();
      CreateMap<Produto, ProdutoDto>()
      .ForMember(d => d.NomeFornecedor,
                 o => o.MapFrom(s => s.Fornecedor.Nome));
    }
  }
}