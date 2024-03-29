using Domain.Entities;

namespace Application.Interfaces;

public interface IBaseUseCase<TIn, TOut>
{
    Task<List<TOut>> Get();
    Task<TOut> GetById(string id);
    Task<TOut> Add(TIn request);
    Task Update(string id, TIn request);
    Task Delete(string id);
}
