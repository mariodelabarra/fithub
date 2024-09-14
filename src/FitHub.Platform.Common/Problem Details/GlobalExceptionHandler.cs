using FitHub.Platform.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitHub.Platform.Common
{
    public class GlobalExceptionHandler
    {
        public static void Configure(IApplicationBuilder builder)
        {
            builder.Run(async context =>
            {
                var exceptionHandlerPathFeature = 
                    context.Features.Get<IExceptionHandlerPathFeature>();

                // Declare the problem results
                IResult problemResult;

                // Switch statement to match the custom exceptions
                switch (exceptionHandlerPathFeature?.Error)
                {
                    // This custom exception here contains validation errors from
                    // Fluent Validation
                    case ValidationRuleException:
                        var exp = (ValidationRuleException)exceptionHandlerPathFeature!.Error;

                        problemResult = Results.ValidationProblem
                        (
                            exp.Errors,
                            type: "https://httpstatuses.com/422",
                            statusCode: StatusCodes.Status422UnprocessableEntity
                        );

                        break;

                    // If no custom exception is matched, return generic 500 Internal Server
                    // error response
                    default:
                        {
                            var details = new ProblemDetails
                            {
                                Type = "https://httpstatuses.com/500",
                                Title = "An error occurred while processing your request.",
                                Status = StatusCodes.Status500InternalServerError
                            };

                        problemResult = Results.Problem(details);
                        break;
                        }
                }

                await problemResult.ExecuteAsync(context);
            });
        }
    }
}
