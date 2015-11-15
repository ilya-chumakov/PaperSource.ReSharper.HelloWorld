using System;
using System.Diagnostics;
using System.Collections.Generic;
using JetBrains.Application.Progress;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReSharper.PackageV3
{
	[ContextAction(Group = "C#", Name = "Null Object Action", Description = "something new")]
	public class NullObjectContextAction : ContextActionBase
	{
		public ICSharpContextActionDataProvider Provider { get; set; }

		public NullObjectContextAction(ICSharpContextActionDataProvider provider)
		{
			Provider = provider;
		}

		public override string Text { get; } = "Introduce Null Object";

		protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
		{
			ReplaceType();

			return null;
		}

		private void ReplaceType()
		{
			try
			{
				var method = Provider.GetSelectedElement<IMethodDeclaration>();

				var type = method.DeclaredElement.ReturnType;

				string typePresentableName = type.GetPresentableName(CSharpLanguage.Instance);

				var factory = CSharpElementFactory.GetInstance(Provider.PsiModule);

				string code = $"new {typePresentableName}()";

				ICSharpExpression newExpression = factory.CreateExpression(code);

				var returnStatement = Provider.GetSelectedElement<IReturnStatement>(false);

				returnStatement.SetValue(newExpression);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
				throw;
			}
		}

		public override bool IsAvailable(IUserDataHolder cache)
		{
			try
			{
				var method = Provider.GetSelectedElement<IMethodDeclaration>();

				bool insideMethod = method != null;

				if (insideMethod)
				{
					bool isGenericList = CorrectReturnType(method);

					bool returnsNull = ReturnsNull();

					return returnsNull && isGenericList;
				}

				return false;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
				throw;
			}
		}

		private bool ReturnsNull()
		{
			try
			{
				var returnStatement = Provider.GetSelectedElement<IReturnStatement>(false);

				if (returnStatement != null)
				{
					ICSharpExpression value = returnStatement.Value;

					return value == null || value.ConstantValue.IsPureNull(CSharpLanguage.Instance);
				}

				return false;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
				throw;
			}
		}

		private static bool CorrectReturnType(IMethodDeclaration method)
		{
			try
			{
				IDeclaredType declaredType = method.DeclaredElement.ReturnType as IDeclaredType;

				if (declaredType == null || declaredType.IsVoid()) return false;

				ISubstitution sub = declaredType.GetSubstitution();

				if (sub.IsEmpty()) return false;

				IType  parameterType = sub.Apply(sub.Domain[0]);

				IMethod declaredElement = method.DeclaredElement;

				IType realType = declaredElement.Type();

				var predefinedType = declaredElement.Module.GetPredefinedType();

				ITypeElement generic = predefinedType.GenericList.GetTypeElement();

				IType sampleType = EmptySubstitution.INSTANCE
					.Extend(generic.TypeParameters, new IType[] { parameterType })
					.Apply(predefinedType.GenericList);

				bool good = realType.IsImplicitlyConvertibleTo(sampleType, new CSharpTypeConversionRule(declaredElement.Module));

				return good;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
				throw;
			}
		}
	}
}