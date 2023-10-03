using System.Security.Cryptography.X509Certificates;
using BlogPessoal.Model;
using BlogPessoal.Service;
using BlogPessoal.Service.Implements;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlogPessoal.Controllers;

[Authorize]
[Route("~/postagens")]
[ApiController]
public class PostagemController : ControllerBase
{
    private readonly IPostagemService _postagemService;
    private readonly IValidator<Postagem> _postagemValidator;
    
    public PostagemController(IPostagemService postagemService, IValidator<Postagem> postagemValidator)
    {
        _postagemService = postagemService;
        _postagemValidator = postagemValidator;
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _postagemService.GetAll());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(long id)
    {
        var postagem = await _postagemService.GetById(id);
            
        if (postagem == null)
            return NotFound();
        
        return Ok(postagem);
    }
    
    [HttpGet("titulo/{titulo}")]
    public async Task<ActionResult> GetByTitulo(string titulo)
    {
        return Ok(await _postagemService.GetByTitulo(titulo));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Postagem postagem)
    {
        var validationResult = await _postagemValidator.ValidateAsync(postagem);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var resposta = await _postagemService.Create(postagem);
        
        if (resposta == null)
            return BadRequest("Tema não encontrado");
        
        return CreatedAtAction(nameof(GetById), new {id = postagem.Id}, postagem);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Postagem postagem)
    {
        if (postagem.Id <= 0)
            return BadRequest("Id da postagem inválido");
        
        var validationResult = await _postagemValidator.ValidateAsync(postagem);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var postagemUpdate = await _postagemService.Update(postagem);
        
        if (postagemUpdate == null)
            return NotFound("Postagem e/ou tema não encontrado(s)");
        
        return Ok(postagemUpdate);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        var postagem = await _postagemService.GetById(id);
        
        if (postagem == null)
            return NotFound("Postagem não encontrada");
        
        await _postagemService.Delete(postagem);
        return NoContent();
    }
}