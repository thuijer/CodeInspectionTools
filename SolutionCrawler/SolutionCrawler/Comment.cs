﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionCrawler
{
    public class Comment
    {
        public Comment(string content, int lineNumber, MemberDeclarationSyntax methodOrPropertyIfAny, TypeDeclarationSyntax typeIfAny, NamespaceDeclarationSyntax namespaceIfAny)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Null/blank content specified");
            if (lineNumber < 0)
                throw new ArgumentOutOfRangeException("lineNumber");

            Content = content;
            LineNumber = lineNumber;
            MethodOrPropertyIfAny = methodOrPropertyIfAny;
            TypeIfAny = typeIfAny;
            NamespaceIfAny = namespaceIfAny;
        }

        /// <summary>
        /// This will never be null or blank
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// This will always be a positive integer
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// This may be null since the comment may not exist within a method or property
        /// </summary>
        public MemberDeclarationSyntax MethodOrPropertyIfAny { get; private set; }

        /// <summary>
        /// This may be null since the comment may not exist within an class, interface or struct
        /// </summary>
        public TypeDeclarationSyntax TypeIfAny { get; private set; }

        /// <summary>
        /// This may be null since the comment may not exist within a method or property
        /// </summary>
        public NamespaceDeclarationSyntax NamespaceIfAny { get; private set; }
    }

}
