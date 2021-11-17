﻿using System.Collections.Generic;
using System.Linq;
using Generator3.Model.Public;

namespace Generator3.Generation.Callback
{
    public class PublicHandlerModel
    {
        private readonly GirModel.Callback _callback;
        private StandardReturnType? _returnType;
        private IEnumerable<StandardParameter>? _parameters;

        public string Name => _callback.Name + "Handler";
        public string DelegateType => _callback.Name;
        public string InternalDelegateType => _callback.Namespace.GetInternalName() + "." + _callback.Name;
        
        public string NamespaceName => _callback.Namespace.Name;

        public StandardReturnType ReturnType => _returnType ??= new StandardReturnType(_callback.ReturnType);
        public IEnumerable<StandardParameter> Parameters => _parameters ??= _callback.Parameters.Select(x => new StandardParameter(x));
        
        public PublicHandlerModel(GirModel.Callback callback)
        {
            _callback = callback;
        }
    }
}
