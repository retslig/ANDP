using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http;
using ANDP.Provisioning.API.Rest.Infrastructure;

namespace ANDP.Provisioning.API.Rest
{
    public static class RegistrationFiltersConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration.Filters.Add(
                new UnhandledExceptionFilterAttribute()
                .Register<KeyNotFoundException>(HttpStatusCode.NotFound)

                .Register<SecurityException>(HttpStatusCode.Forbidden)

                .Register<SqlException>(
                    (exception, request) =>
                    {
                        var sqlException = exception as SqlException;

                        if (sqlException.Number > 50000)
                        {
                            var response = request.CreateResponse(HttpStatusCode.BadRequest);
                            response.ReasonPhrase = sqlException.Message.Replace(Environment.NewLine, String.Empty);

                            return response;
                        }
                        else
                        {
                            return request.CreateResponse(HttpStatusCode.InternalServerError);
                        }
                    }
                )
            );
        }
    }
}