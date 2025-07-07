namespace Backpack.Domain.Contract.Mediator;

public interface IRequest { }

public interface IRequest<TResult> : IRequest { }
