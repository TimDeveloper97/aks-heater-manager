using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace System
{
    public class TokenMap : Dictionary<string, Actor>
    {
        public Actor Find(string token)
        {
            Actor u = null;
            this.TryGetValue(token, out u);

            return u;
        }
        public Actor Add(Account acc)
        {
            var type = Type.GetType($"Actors.{acc.Role}");
            var o = (Actor)Activator.CreateInstance(type);
            o.Copy(acc);

            var token = acc.UserName.JoinMD5(DateTime.Now);
            o.Token = token;
            o.Password = null;

            if (ContainsKey(token) == false)
            {
                Add(token, o);
            }
            return o;
        }
        public bool Remove(Actor user)
        {
            var k = user.Token;
            if (k == null)
                return false;

            return Remove(k);
        }
    }
}

namespace System
{
    using BsonData;
    partial class Account
    {
    }
    partial class Actor
    {
    }
}

namespace Actors
{
    public partial class Technical : TokenUser
    {

    }
    public partial class Customer : Staff
    {

    }

    public partial class Staff : TokenUser
    {

    }
}
