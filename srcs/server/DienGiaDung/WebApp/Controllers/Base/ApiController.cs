using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web.Http;
using BsonData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApp
{
    public class ApiContext : ExecuteContext
    {
        public System.Web.HttpResponse Response { get; set; }
        public System.Web.HttpRequest Request { get; set; }
        public string Body
        {
            get
            {
                return new StreamReader(Request.InputStream).ReadToEnd();
            }
        }
    }
    public class Api : BaseController
    {
        public virtual object Exec(ApiContext context)
        {
            var body = context.Body;
            var json = string.IsNullOrWhiteSpace(body) ? new Document() : Document.Parse(body);
            var token = json.Token;
            
            Actor actor = null;
            string method_name = context.Params[0];

            if (context.Action == "all")
            {
                var url = json.Url.Split('/');
                context.Action = ExecuteContext.GetObjectName(url[0]);
                method_name = url[1];
            }

            context.Response.ContentType = "application/json";
            MethodCollection mc = Actor.Mvc.FindMethods(context.Action);
            if (mc == null)
            {
                return Actor.ActionNotFound;
            }
            actor = (Actor)Activator.CreateInstance(mc.Type);
            if (actor is TokenUser)
            {
                if (string.IsNullOrEmpty(token))
                    return Actor.TokenError;

                actor = Actor.Logged.Find(token);
                if (actor == null)
                {
                    return Actor.TokenError;
                }
            }
            context.Tag = json.ValueContext;
            actor.RequestContext = context;

            MethodInfo method;
            object res = null;
            if (mc.TryGetValue(ExecuteContext.GetObjectName(method_name), out method))
            {
                res = method.Invoke(actor, new object[] { });
            }
            return res;
        }
    }
}
