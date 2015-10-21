﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Fonlow.TypeScriptCodeDom
{
    public class TsCodeGenerator : ICodeGenerator
    {
        public string CreateEscapedIdentifier(string value)
        {
            return KeywordHandler.CreateEscapedIdentifier(value);
        }

        public string CreateValidIdentifier(string value)
        {
            return KeywordHandler.CreateValidIdentifier(value);
        }

        public void GenerateCodeFromCompileUnit(CodeCompileUnit e, TextWriter w, CodeGeneratorOptions o)
        {
            e.Namespaces.OfType<CodeNamespace>().ToList().ForEach(n =>
            {
                GenerateCodeFromNamespace(n, w, o);
                w.WriteLine();
            });
        }

        public void GenerateCodeFromExpression(CodeExpression e, TextWriter w, CodeGeneratorOptions o)
        {
            CodeObjectHelper.GenerateCodeFromExpression(e, w, o);
        }

        public void GenerateCodeFromNamespace(CodeNamespace e, TextWriter w, CodeGeneratorOptions o)
        {
            w.WriteLine($"module {e.Name}{{");

            e.Types.OfType<CodeTypeDeclaration>().ToList().ForEach(t =>
            {
                GenerateCodeFromType(t, w, o);
                w.WriteLine();
            });
            //todo: later e.Imports.

            w.WriteLine($"}}");
        }

        public void GenerateCodeFromStatement(CodeStatement e, TextWriter w, CodeGeneratorOptions o)
        {
            CodeObjectHelper.GenerateCodeFromStatement(e, w, o);
        }

        public void GenerateCodeFromType(CodeTypeDeclaration e, TextWriter w, CodeGeneratorOptions o)
        {
            var currentIndent = o.IndentString;
            var accessModifier = ((e.TypeAttributes & System.Reflection.TypeAttributes.Public) == System.Reflection.TypeAttributes.Public) ? "export" : String.Empty;
            var typeOfType = CodeObjectHelper.GetTypeOfType(e);
            var name = e.Name;
            var typeParametersExpression = CodeObjectHelper.GetTypeParametersExpression(e);
            var baseTypesExpression = CodeObjectHelper.GetBaseTypeExpression(e);
            w.Write($"{o.IndentString}{accessModifier} {typeOfType} {name}{typeParametersExpression}{baseTypesExpression}{{");
            o.IndentString += Constants.BasicIndent;
            CodeObjectHelper.GenerateTypeMembers(e, w, o);
            w.WriteLine(currentIndent+"}");
            o.IndentString = currentIndent;
        }

        public string GetTypeOutput(CodeTypeReference type)
        {
            return TypeMapper.GetTypeOutput(type);
        }

        public bool IsValidIdentifier(string value)
        {
            return KeywordHandler.IsValidIdentifier(value);
        }

        public bool Supports(GeneratorSupport supports)
        {
            return (supports & supported) != 0;
        }

        const GeneratorSupport supported = GeneratorSupport.ArraysOfArrays
            | GeneratorSupport.MultidimensionalArrays
            | GeneratorSupport.TryCatchStatements
            | GeneratorSupport.DeclareValueTypes
            | GeneratorSupport.DeclareEnums
            | GeneratorSupport.GotoStatements
            | GeneratorSupport.StaticConstructors
            | GeneratorSupport.DeclareInterfaces
            | GeneratorSupport.DeclareDelegates
            | GeneratorSupport.DeclareEvents
            | GeneratorSupport.NestedTypes
            | GeneratorSupport.MultipleInterfaceMembers
            | GeneratorSupport.ComplexExpressions
            | GeneratorSupport.PartialTypes
            | GeneratorSupport.GenericTypeReference
            | GeneratorSupport.GenericTypeDeclaration
            | GeneratorSupport.DeclareIndexerProperties;

        public void ValidateIdentifier(string value)
        {
            KeywordHandler.ValidateIdentifier(value);
        }

    }




}