using System.Dynamic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using BlogPessoal.Model;
using BlogPessoalTest.Factory;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit.Extensions.Ordering;

namespace BlogPessoalTest.Controller;

public class UserControllerTest : IClassFixture<WebAppFactory>
{
    protected readonly WebAppFactory _factory;
    protected HttpClient _client;
    
    private readonly dynamic token;
    private string Id { get; set; } = string.Empty;
    
    public UserControllerTest(WebAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        token = GetToken();
    }

    private static dynamic GetToken()
    {
        dynamic data = new ExpandoObject();
        data.sub = "root@root.com";
        return data;
    }

    [Fact, Order(1)]
    public async Task DeveCriarUmUsuario()
    {
        var novoUsuario = new Dictionary<string, string>()
        {
            { "nome", "Ingrid" },
            { "usuario", "ingrid@email.com" },
            { "senha", "12345678" },
            {"foto", "" }
        };
        
        var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
        var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");
        
        var resposta = await _client.PostAsync("/usuarios/cadastrar", corpoRequisicao);
        resposta.EnsureSuccessStatusCode();
        resposta.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Fact, Order(2)]
    public async Task DeveDarErroEmail()
    {
        var novoUsuario = new Dictionary<string, string>()
        {
            { "nome", "Ingrid" },
            { "usuario", "ingridemail.com" },
            { "senha", "12345678" },
            {"foto", "" }
        };
        
        var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
        var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");
        
        var resposta = await _client.PostAsync("/usuarios/cadastrar", corpoRequisicao);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact, Order(3)]
    public async Task NaoDeveCriarUsuarioDuplicado()
    {
        var novoUsuario = new Dictionary<string, string>()
        {
            { "nome", "Karina" },
            { "usuario", "karina@email.com" },
            { "senha", "12345678" },
            {"foto", "" }
        };
        
        var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
        var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");
        
        await _client.PostAsync("/usuarios/cadastrar", corpoRequisicao);
        
        var resposta = await _client.PostAsync("/usuarios/cadastrar", corpoRequisicao);
        
        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact, Order(4)]
    public async Task DeveListarTodosUsuarios()
    {
        _client.SetFakeBearerToken((object) token);
        
        var resposta = await _client.GetAsync("/usuarios/all");

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact, Order(5)]
    public async Task DeveAtualizarUmUsuario()
    {
        var novoUsuario = new Dictionary<string, string>()
        {
            { "nome", "João" },
            { "usuario", "joao@email.com" },
            { "senha", "12345678" },
            {"foto", "" }
        };
        
        var usuarioJson = JsonConvert.SerializeObject(novoUsuario);
        var corpoRequisicao = new StringContent(usuarioJson, Encoding.UTF8, "application/json");
        
        var resposta = await _client.PostAsync("/usuarios/cadastrar", corpoRequisicao);
        
        var corpoRespostaPost = await resposta.Content.ReadFromJsonAsync<User>();
        
        if(corpoRespostaPost != null)
            Id = corpoRespostaPost.Id.ToString();

        var usuarioAtualizado = new Dictionary<string, string>()
        {
            { "id", Id },
            { "nome", "João Atualizado" },
            { "usuario", "joao@email.com" },
            { "senha", "12345678" },
            { "foto", "" }
        };
        
        var usuarioJsonAtualizado = JsonConvert.SerializeObject(usuarioAtualizado);
        var corpoRequisicaoAtualizado = new StringContent(usuarioJsonAtualizado, Encoding.UTF8, "application/json");
        
        _client.SetFakeBearerToken((object) token);
        
        var respostaPut = await _client.PutAsync("/usuarios/atualizar", corpoRequisicaoAtualizado);
        
        respostaPut.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}