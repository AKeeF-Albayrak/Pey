namespace Pey.Shared.Results;

// Birden fazla alan hatasını tek bir hata olarak taşır (örn. hem "Title" hem "StartingPrice" geçersiz).
public sealed record ValidationError(Error[] Errors)
    : Error("General.Validation", "Bir veya daha fazla doğrulama hatası oluştu.", ErrorType.Validation);
