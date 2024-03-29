using Application.Interfaces;
using Domain.Base;
using Domain.Entities;
using Domain.Repositories;
using Domain.Request;

namespace Application.UseCases;

public class ProductUseCase : IProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly IMessageQueueService _messageQueueService;

    public ProductUseCase(IProductRepository productRepository, IMessageQueueService messageQueueService)
    {
        _productRepository = productRepository;
        _messageQueueService = messageQueueService;
    }

    public async Task<List<Product>> Get()
    {
        return await _productRepository.Get();
    }

    public async Task<Product> GetById(string productId)
    {
        return await _productRepository.GetEntityByIdAsync(productId);
    }

    public async Task<List<Product>> GetByCategory(Category category)
    {
        var products = await _productRepository.GetByCategory(category);

        return products.ToList();
    }
    public async Task<Product> Add(ProductRequest request)
    {
        var product = Product.FromProductRequest(request);

        await _productRepository.AddAsync(product);

        _messageQueueService.PublishMessage("Totem.Products.Created", product.ToJson());

        return product;
    }

    public async Task Update(string id, ProductRequest productRequest)
    {
        var product = await GetById(id);

        product.Description = productRequest.Description;
        product.ImageURL = productRequest.ImageURL;
        product.Category = productRequest.Category;
        product.Name = productRequest.Name;
        product.Price = productRequest.Price;
        
        await _productRepository.UpdateAsync(id, product);

        _messageQueueService.PublishMessage("Totem.Products.Updated", product.ToJson());

    }

    public async Task Delete(string productId)
    {
        var product = await GetById(productId);

        if (product is null) throw new DomainException("Produto não encontrado");

        await _productRepository.DeleteAsync(productId);

        _messageQueueService.PublishMessage("Totem.Products.Deleted", product.ToJson());

    }
}