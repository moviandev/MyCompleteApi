using System.ComponentModel.DataAnnotations;
using DevIO.Business.Models;

namespace DevIO.Api.Dtos
{
  public class FornecedorDto
  {
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatorio")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatorio")]
    [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
    public string Documento { get; set; }

    public TipoFornecedor TipoFornecedor { get; set; }

    public Endereco Endereco { get; set; }

    public bool Ativo { get; set; }
  }
}