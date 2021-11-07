using GameApis.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace GameApis.OperationFilters;

internal class AddDomainResponseOperationFilter : IOperationFilter
{
    private static OpenApiReference? _exceptionReference;
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        EnsureExceptionReferenceCreated(context);
        operation.Responses.Add("400", new OpenApiResponse
        {
            Description = "When a domain exception occurs",
            Content = new Dictionary<string, OpenApiMediaType>()
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = _exceptionReference
                    }
                }
            }
        });
    }

    [MemberNotNull(nameof(_exceptionReference))]
    private static void EnsureExceptionReferenceCreated(OperationFilterContext context)
    {
        if (_exceptionReference is not null)
        {
            return;
        }

        var schema = context.SchemaGenerator.GenerateSchema(typeof(DomainExceptionResponse), context.SchemaRepository);
        _exceptionReference = schema.Reference;
    }
}
