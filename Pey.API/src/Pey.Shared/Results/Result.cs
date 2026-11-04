namespace Pey.Shared.Results;

// Pipeline behavior'larının (bkz. ValidationBehavior) TResponse'un somut tipini bilmeden
// hata sonucu üretebilmesini sağlar. C# 11 "static abstract" arayüz üyesi.
public interface IFailureFactory<TSelf> where TSelf : IFailureFactory<TSelf>
{
    static abstract TSelf Failure(Error error);
}

public class Result : IFailureFactory<Result>
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Başarılı sonuç hata içeremez, başarısız sonuç hatasız olamaz.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    // Handler içinde "return AuctionErrors.NotFound;" yazabilmek için.
    public static implicit operator Result(Error error) => Failure(error);
}

public class Result<TValue> : Result, IFailureFactory<Result<TValue>>
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Başarısız bir sonucun değerine erişilemez.");

    public static new Result<TValue> Failure(Error error) => new(default, false, error);

    // "return auction;" ve "return AuctionErrors.NotFound;" ikisi de Result<T>'ye dönüşür.
    public static implicit operator Result<TValue>(TValue value) => new(value, true, Error.None);
    public static implicit operator Result<TValue>(Error error) => Failure(error);
}
