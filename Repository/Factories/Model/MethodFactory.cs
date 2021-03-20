﻿using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Model;
using Repository.Xml;

namespace Repository.Factories
{
    internal class MethodFactory
    {
        private readonly ReturnValueFactory _returnValueFactory;
        private readonly ArgumentsFactory _argumentsFactory;
        private readonly ArgumentFactory _argFactory;
        private readonly CaseConverter _caseConverter;

        public MethodFactory(ReturnValueFactory returnValueFactory, ArgumentsFactory argumentsFactory, ArgumentFactory argFactory, CaseConverter caseConverter)
        {
            _returnValueFactory = returnValueFactory;
            _argumentsFactory = argumentsFactory;
            _caseConverter = caseConverter;
            _argFactory = argFactory;
        }

        public Method Create(MethodInfo methodInfo, Namespace @namespace)
        {
            if (methodInfo.Name is null)
                throw new Exception("MethodInfo name is null");

            if (methodInfo.ReturnValue is null)
                throw new Exception($"{nameof(MethodInfo)} {methodInfo.Name} {nameof(methodInfo.ReturnValue)} is null");

            if (methodInfo.Identifier is null)
                throw new Exception($"{nameof(MethodInfo)} {methodInfo.Name} is missing {nameof(methodInfo.Identifier)} value");

            if (methodInfo.Name != string.Empty)
            {
                return new Method(
                    elementName: new ElementName(methodInfo.Identifier),
                    elementManagedName: new ElementManagedName(_caseConverter.ToPascalCase(methodInfo.Name)),
                    returnValue: _returnValueFactory.Create(methodInfo.ReturnValue, @namespace.Name),
                    arguments: _argumentsFactory.Create(methodInfo.Parameters, @namespace.Name, methodInfo.Throws),
                    instanceArg: methodInfo.Parameters?.InstanceParameter != null
                        ? _argFactory.Create(methodInfo.Parameters.InstanceParameter, @namespace.Name)
                        : null
                );
            }

            if (!string.IsNullOrEmpty(methodInfo.MovedTo))
                throw new MethodMovedException(methodInfo, $"Method {methodInfo.Identifier} moved to {methodInfo.MovedTo}.");

            throw new Exception($"{nameof(MethodInfo)} {methodInfo.Identifier} has no {nameof(methodInfo.Name)} and did not move.");

        }

        public Method CreateGetTypeMethod(string getTypeMethodName, Namespace @namespace)
        {
            ReturnValue returnValue = _returnValueFactory.Create(
                type: "gulong",
                transfer: Transfer.None,
                nullable: false,
                namespaceName: @namespace.Name
            );

            return new Method(
                elementName: new ElementName(getTypeMethodName),
                elementManagedName: new ElementManagedName("GetGType"),
                returnValue: returnValue,
                arguments: Enumerable.Empty<Argument>()
            );
        }

        public IEnumerable<Method> Create(IEnumerable<MethodInfo> methods, Namespace @namespace)
        {
            var list = new List<Method>();

            foreach (var method in methods)
            {
                try
                {
                    list.Add(Create(method, @namespace));
                }
                catch (ArgumentFactory.VarArgsNotSupportedException ex)
                {
                    Log.Debug($"Method {method.Name} could not be created: {ex.Message}");
                }
                catch (MethodMovedException ex)
                {
                    Log.Debug($"Method ignored: {ex.Message}");
                }
            }

            return list;
        }

        public class MethodMovedException : Exception
        {
            public MethodInfo MethodInfo { get; }

            public MethodMovedException(MethodInfo methodInfo, string message) : base(message)
            {
                MethodInfo = methodInfo;
            }
        }
    }
}
