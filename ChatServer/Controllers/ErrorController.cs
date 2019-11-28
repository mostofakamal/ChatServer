using System;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        [HttpGet]
        public ActionResult Error([FromServices] IHostingEnvironment webHostEnvironment)
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = feature?.Error;
            if (ex != null)
            {
                var isDev = webHostEnvironment.IsDevelopment();
                var statusCode = (int) HttpStatusCode.InternalServerError;
                if (ex is InvalidOperationException)
                {
                    statusCode = (int)HttpStatusCode.BadRequest;
                }
                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Instance = feature?.Path,
                    Title = isDev ? $"{ex.GetType().Name}: {ex.Message}" : "An error occurred.",
                    Detail = isDev ? ex.StackTrace : null,
                };

                return StatusCode(problemDetails.Status.Value, problemDetails);
            }

            return StatusCode((int) HttpStatusCode.OK);
        }
    }
}