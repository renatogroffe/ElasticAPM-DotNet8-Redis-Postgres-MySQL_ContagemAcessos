using APIOrquestracao.Clients;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SerilogEnricher;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog(new LoggerConfiguration()
    .Enrich.WithElasticApmCorrelationInfo()
    .WriteTo.Console(outputTemplate: "[{ElasticApmTraceId} {ElasticApmTransactionId} {Message:lj} {NewLine}{Exception}")
    .CreateLogger());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ContagemClient>();

builder.Services.AddElasticApmForAspNetCore(new HttpDiagnosticsSubscriber());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();