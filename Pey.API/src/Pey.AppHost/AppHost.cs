var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Pey_API>("api");

builder.Build().Run();
