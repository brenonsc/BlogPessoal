using BlogPessoal.Model;
using BlogPessoal.Service.Implements;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controllers;

[Route("~/postagens")]
[ApiController]
public class PostagemController : ControllerBase
{
    private readonly PostagemService _postagemService;
    private readonly IValidator<Postagem> _postagemValidator;
    
    public PostagemController(PostagemService postagemService, IValidator<Postagem> postagemValidator)
    {
        _postagemService = postagemService;
        _postagemValidator = postagemValidator;
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _postagemService.GetAll());
    }
}