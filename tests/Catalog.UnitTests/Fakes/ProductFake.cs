using Bogus;
using Bogus.DataSets;
using Domain.Entities;
using Domain.Request;

namespace Catalog.UnitTests.Fakes;

public static class ProductFake
{
    public static List<Product> Create(int quantity)
    {
        var faker = new Faker<Product>("pt_BR")
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.Price, f => f.Finance.Amount())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.Category, f => f.PickRandom<Category>())
            .RuleFor(x => x.CreatedAt, f => f.Date.Recent())
            .RuleFor(x => x.ImageURL, f => f.Image.PicsumUrl());

        return faker.Generate(quantity);

    }
}

public static class ProductRequestFake
{
    public static List<ProductRequest> Create(int quantity)
    {
        var faker = new Faker<ProductRequest>("pt_BR")
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.Price, f => f.Finance.Amount())
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.Category, f => f.PickRandom<Category>())
            .RuleFor(x => x.ImageURL, f => f.Image.PicsumUrl());

        return faker.Generate(quantity);
    }
}