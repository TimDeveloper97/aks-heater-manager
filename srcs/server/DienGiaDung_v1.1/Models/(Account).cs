using BsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public partial class Account : Document
    {
        #region Attributes
        #endregion

        public Account() { }
        public Account(string userName, string password)
        {
            var u = userName.ToLower();
            if (password != null)
            {
                password = u.JoinMD5(password);
            }
            ObjectId = u;
            Password = password;
        }
        public Account(string userName, string password, string role)
            : this(userName, password)
        {
            Role = role;
        }
        public virtual bool IsPasswordValid(string original, string encriped)
        {
            var epw = this.Password;
            if ((epw == null && UserName != original)
                || (epw != null && epw != encriped))
            {
                return false;
            }
            return true;
        }
    }
}

#region API
namespace System
{
    partial class Account
    {
    }

    public abstract partial class Actor : Document
    {
        static public TokenMap Logged { get; private set; } = new TokenMap();
        static public MethodInfoMap<Actor> Mvc { get; private set; } = new MethodInfoMap<Actor>();

        static public Document ActionNotFound => Error(404, "Action not found");
        static public Document TokenError => Error(100, "Token Error");
        static public Document Error(int code, string message) => new Document { Code = code, Message = message };
        static public Document Error(int code) => new Document { Code = code };
        static public Document Success() => new Document();
        static public Document Success(object model) => new Document { Value = model };
        static public Document Success(object model, string message) => new Document { Value = model, Message = message };

        protected ExecuteContext _execContext;
        public ExecuteContext RequestContext
        {
            get
            {
                if (_execContext == null)
                    _execContext = new ExecuteContext();
                return _execContext;
            }
            set
            {
                _execContext = value;
            }
        }

        new public Document ValueContext
        {
            get => (Document)RequestContext.Tag;
            set => RequestContext.Tag = value;
        }

        public object Execute(ExecuteContext context)
        {
            _execContext = context;
            var method = Mvc.Find(this.GetType(), context.Action);
            var res = method?.Invoke(this, new object[] { });

            return res;
        }
    }
    public partial class Guest : Actor
    {
    }
    public abstract partial class TokenUser : Actor
    {
        protected virtual Document CheckToken()
        {
            return Success();
        }
    }
}
#endregion

namespace System
{
    public class AccountDB : Collection
    {
        public AccountDB() : base(nameof(DB.Accounts), DB.Main)
        {
        }

    }

    partial class DB
    {
        static AccountDB _accounts;
        public static AccountDB Accounts
        {
            get
            {
                if (_accounts == null) _accounts = new AccountDB();
                return _accounts;
            }
        }
    }
}
