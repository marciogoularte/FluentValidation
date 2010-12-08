#region License
// Copyright (c) Jeremy Skinner (http://www.jeremyskinner.co.uk)
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://www.codeplex.com/FluentValidation
#endregion

namespace FluentValidation {
	using System;
	using System.Collections.Generic;
	using Internal;
	using Validators;
    using System.Linq.Expressions;

	/// <summary>
	/// Rule builder that starts the chain
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TProperty"></typeparam>
	public interface IRuleBuilderInitial<T, TProperty> : IFluentInterface, IRuleBuilder<T, TProperty>, IConfigurable<PropertyRule<T>, IRuleBuilderInitial<T, TProperty>> {
	}

	/// <summary>
	/// Rule builder 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TProperty"></typeparam>
	public interface IRuleBuilder<T, TProperty> : IFluentInterface {
		/// <summary>
		/// Associates a validator with this the property for this rule builder.
		/// </summary>
		/// <param name="validator">The validator to set</param>
		/// <returns></returns>
		IRuleBuilderOptions<T, TProperty> SetValidator(IPropertyValidator validator);

		/// <summary>
		/// Associates an instance of IValidator with the current property rule.
		/// </summary>
		/// <param name="validator">The validator to use</param>
		IRuleBuilderOptions<T, TProperty> SetValidator(IValidator validator);

        /// <summary>
        /// Uses the validator specified to extract the rule
        /// </summary>
        /// <typeparam name="TModel">The model to extract property name</typeparam>
        /// <typeparam name="TValidator">The validator to extract property from</typeparam>
        IRuleBuilderOptions<T, TProperty> UsingRuleFrom<TModel, TValidator>(Expression<Func<TModel, TProperty>> expression) where TValidator : IValidator<TModel>;

        /// <summary>
        /// Uses the validator specified to extract the rule
        /// </summary>
        /// <typeparam name="TValidator">The validator to extract property from using the same name</typeparam>
        IRuleBuilderOptions<T, TProperty> UsingRuleFrom<TValidator>() where TValidator : IValidator;

        /// <summary>
        /// Associates an instance of IValidator with the current property rule.
        /// </summary>
        /// <typeparam name="TValidator">The validator to use</typeparam>
        /// <returns></returns>
	    IRuleBuilderOptions<T, TProperty> Using<TValidator>() where TValidator : IValidator;
	}


	/// <summary>
	/// Rule builder
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TProperty"></typeparam>
	public interface IRuleBuilderOptions<T, TProperty> : IRuleBuilder<T, TProperty>, IConfigurable<PropertyRule<T>, IRuleBuilderOptions<T, TProperty>> {

	}
}