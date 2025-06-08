using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Models
{
    public abstract class UpdatingModel
    {
        static public MethodInfoMap<UpdatingModel> Mvc { get; private set; } = new MethodInfoMap<UpdatingModel>();
        public Actor Actor { get; set; }
        public Document Context { get; set; }

        public object Exec(string action)
        {
            return Mvc.Find(this.GetType(), action)?.Invoke(this, new object[] { });
        }
        public object Exec(Actor actor, Document context, string action)
        {
            Actor = actor;
            Context = context;

            return Exec(action);
        }
    }

    public abstract class ActorUpdatingModel : UpdatingModel
    {
        public object Exec(Actor actor)
        {
            return Exec(actor, actor.ValueContext, actor.RequestContext.Params[0]);
        }
    }
}