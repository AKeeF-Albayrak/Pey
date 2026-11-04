using Pey.Shared.Results;

namespace Pey.Shared.Messaging;

// Tüm mesajların ortak atası; TResponse, Send'in dönüş tipini derleme zamanında bilmesini sağlar.
public interface IRequest<TResponse>;

// Command: sistemde bir şeyi değiştirir (CreateAuction, PlaceBid).
public interface ICommand : IRequest<Result>;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;

// Query: sadece okur, hiçbir şeyi değiştirmez (GetAuctionById).
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
