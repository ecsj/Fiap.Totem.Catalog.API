using Application.Interfaces;
using Domain.Entities;
using Domain.Request;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductUseCase _productUseCase;

    public ProductController(IProductUseCase productUseCase)
    {
        _productUseCase = productUseCase;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var products = await _productUseCase.Get();

        return Ok(products);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var product = await _productUseCase.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    [HttpGet("GetCategory/{category}")]
    public async Task<IActionResult> GetCategory(Category category)
    {
        var products = await _productUseCase.GetByCategory(category);

        return Ok(products);
    }

    [HttpGet("GetProductCategories/")]
    public IActionResult GetProductCategories()
    {
        var categories = new Dictionary<int, string>();

        Enum.GetValues(typeof(Category))
                        .Cast<Category>()
                        .ToList().ForEach(v => categories.Add(Convert.ToInt32(v), v.ToString()));

        return Ok(categories);
    }

    /// <summary>
    /// Adiciona um novo Produto
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     POST /api/Product
    ///     {
    ///         "name": "Coca cola",
    ///         "price": 9.99,
    ///         "description": "",
    ///         "category": 2,
    ///         "imageURL": "https://picsum.photos/200/300"
    ///     }
    ///
    ///  Categorias:
    ///
    ///     0 = Lanche,
    ///     1 = Acompanhamento,
    ///     2 = Bebida,
    ///     3 = Sobremesa
    /// </remarks>
    /// <param name="value">Valor do novo item.</param>
    /// <returns>O novo item adicionado.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductRequest request)
    {
        var product = await _productUseCase.Add(request);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductRequest updatedProduct)
    {
        await _productUseCase.Update(id, updatedProduct);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _productUseCase.Delete(id);

        return NoContent();
    }
}