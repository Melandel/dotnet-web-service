﻿namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.ObjectTextualRendering;

class ObjectRenderingShould
{
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases))]
	public void Foo(object obj, string expected)
	=> Assert.That(obj.Serialize(), Is.EqualTo(expected));
}
