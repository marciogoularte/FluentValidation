#region License
// Copyright 2008-2009 Jeremy Skinner (http://www.jeremyskinner.co.uk)
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

namespace FluentValidation.Tests {
	using System.Linq;
	using Internal;
	using NUnit.Framework;
	using System.Collections.Generic;

	[TestFixture]
	public class ChainingValidatorsTester {
		TestValidator validator;

		[SetUp]
		public void Setup() {
			validator = new TestValidator();
		}

		[Test]
		public void Should_create_multiple_validators() {
			validator.RuleFor(x => x.Surname)
				.NotNull()
				.NotEqual("foo");

			validator.Cast<PropertyRule<Person>>().Single().Validators.Count().ShouldEqual(2);
		}

		[Test]
		public void Should_execute_multiple_validators() {
			validator.RuleFor(x => x.Surname).NotNull()
				.Equal("Foo");

			validator.Validate(new Person()).Errors.Count().ShouldEqual(2);
		}

		[Test]
		public void Options_should_only_apply_to_current_validator() {
			validator.RuleFor(x => x.Surname).NotNull()
				.WithMessage("null")
				.Equal("foo")
				.WithMessage("equal");

			var results = validator.Validate(new Person());
			results.Errors.ElementAt(0).ErrorMessage.ShouldEqual("null");
			results.Errors.ElementAt(1).ErrorMessage.ShouldEqual("equal");
		}
	}
}