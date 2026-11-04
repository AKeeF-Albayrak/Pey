var builder = DistributedApplication.CreateBuilder(args);

// Tüm modüller tek veritabanını kullanır; her modül kendi şemasına sahiptir (identity, auctions, ...).
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var db = postgres.AddDatabase("pey");

var redis = builder.AddRedis("redis")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

// MinIO resmi imajları Docker Hub'dan kaldırıldı; birebir uyumlu topluluk fork'u kullanılıyor.
var minio = builder.AddMinioContainer("minio")
    .WithImage("pgsty/minio")
    .WithImageTag("RELEASE.2026-08-04T00-00-00Z")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.Pey_API>("api")
    .WithReference(db)
    .WithReference(redis)
    .WithReference(rabbitmq)
    .WithReference(minio)
    .WaitFor(db)
    .WaitFor(redis)
    .WaitFor(rabbitmq)
    .WaitFor(minio);

builder.Build().Run();
