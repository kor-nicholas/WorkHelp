using WorkHelpApi.Tests;
using WorkHelpApi.Services.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHelpApi.Tests.Services.Common;

[TestClass]
public class Sha256HashPassTest : BaseServiceTest
{
	[TestMethod]
	[DataRow("test", "test1234")]
	public async Task Sha256HashPassTest_HashPassAndAddSaltInDb_ReturnsHash(string name, string pass)
	{
		// Arrange
		//
		// Act
		//
		// Assert
		Assert.IsNull(null);
	}
}


