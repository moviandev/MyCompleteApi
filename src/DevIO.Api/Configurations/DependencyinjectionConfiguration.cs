using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using DevIO.Business.Services;
using DevIO.Data.Context;
using DevIO.Data.Repository;

namespace DevIO.Api.Configurations
{
  public static class DependencyinjectionConfiguration
  {
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {
      services.AddScoped<MeuDbContext>();
      services.AddScoped<IFornecedorRepository, FornecedorRepository>();
      services.AddScoped<IEnderecoRepository, EnderecoRepository>();
      services.AddScoped<IProdutoRepository, ProdutoRepository>();

      services.AddScoped<IFornecedorService, FornecedorService>();
      services.AddScoped<INotificador, Notificador>();

      return services;
    }
  }
}