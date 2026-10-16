# Pey — Canlı Açık Artırma Platformu

> "Pey sürmek": açık artırmada teklif vermek.

Pey, kullanıcıların ürünlerini açık artırmaya çıkarabildiği, diğer kullanıcıların **gerçek zamanlı** teklif verebildiği ve kazananın platform üzerinden **güvenli ödeme** yapabildiği bir web uygulamasıdır.

Proje, modern .NET ve React ekosistemindeki teknolojileri gerçek ihtiyaçlar üzerinden öğrenmek amacıyla geliştirilmektedir.

---

## Özellikler

- 🔐 Kayıt, giriş, JWT + refresh token, rol bazlı yetki (Alıcı / Satıcı / Admin)
- 📦 Açık artırma oluşturma, fotoğraf yükleme, kategori ve başlangıç fiyatı belirleme
- ⚡ SignalR ile canlı teklif akışı; teklifin geçildiğinde anlık bildirim
- 🔒 Eşzamanlı tekliflerde tutarlılık (Redis dağıtık kilit + iyimser eşzamanlılık)
- ⏱️ Süre dolunca otomatik kapanış ve son saniye uzatması (anti-sniping)
- 💳 iyzico ile ödeme, 3D Secure, webhook doğrulama ve idempotency
- 📨 Ödeme sonrası e-posta, fatura ve satıcı bildirimi (RabbitMQ + Outbox)
- 🔎 Tam metin arama ve filtreleme (Meilisearch / Elasticsearch)
- 📊 OpenTelemetry ile izleme, Aspire Dashboard üzerinden gözlem
