using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ServiceMonitoringToolExample.Api.Extensions
{
    public static class ObjectExtensions
    {
        public static HttpContent SerializeJson(this object value)
        {
            if (value == null)
            {
                throw new Exception("Can't Serialize null");
            }
            var json = JsonConvert.SerializeObject(value);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }
    }
}