using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DevIO.Api.Controllers
{
  [ApiController]
  public abstract class MainController : ControllerBase
  {
    private readonly INotificador _notifier;

    protected MainController(INotificador notifier)
    {
      _notifier = notifier;
    }

    protected bool ValidExecution()
    {
      return !_notifier.TemNotificacao();
    }

    protected ActionResult CustomResponse(object result = null)
    {
      if (ValidExecution())
        return Ok(new
        {
          success = true,
          data = result
        });

      return BadRequest(new
      {
        success = false,
        errors = _notifier.ObterNotificacoes().Select(n => n.Mensagem)
      });
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
      if (!modelState.IsValid) NotifyErrorInvalidModel(modelState);
      return CustomResponse();
    }

    protected void NotifyErrorInvalidModel(ModelStateDictionary modelState)
    {
      var errors = modelState.Values.SelectMany(e => e.Errors);

      foreach (var error in errors)
      {
        var errorMessage = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
        NotifyError(errorMessage);
      }
    }

    protected void NotifyError(string errorMessage)
    {
      _notifier.Handle(new Notificacao(errorMessage));
    }
  }
}