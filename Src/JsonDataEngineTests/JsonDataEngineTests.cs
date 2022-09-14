using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonDataEngine.Tests
{
	[TestClass()]
	public class JsonDataEngineTests
	{
		private string JsonTemplate = @"
		{
		  data: {
				title: ""$name"",
				projects: [
					{
						title: null,
						totalhours: 100,
						percentage: 66
					},
					{
						title: ""$name"",
						totalhours: 50,
						percentage: 33
					}
				]
			},
		  template: {
			type: ""weather"",
			items: [
			  {
				type: 'pages',
				items: [],
				aproperty: '',
				test: ""$foreach projects use { projectTitle: '$key', projectValues: '$foreach values use { owner: \\'$EMPLOYER\\', employee: \\'$EMPLOYEE\\' }'}""
			  }
			]
		  }
		}";

		private string JsonData = @"
		{ 
			name: ""bob"",
			projects: [ 
				{
					key: ""project A"", 
					values: [
						{ 
							EMPLOYER: ""Manu"", 
							EMPLOYEE: ""Lolo""
						}, 
						{ 
							EMPLOYER: ""Manu"", 
							EMPLOYEE: ""Jen""
						}
					] 
				}, 
				{ 
					key: ""project B"", 
					values: [
						{ 
							EMPLOYER: ""Lolo"", 
							EMPLOYEE: ""Adri""
						}, 
						{ 
							EMPLOYER: ""Lolo"", 
							EMPLOYEE: ""Met""
						}
					] 
				} 
			] 
		}";


		[TestMethod()]
		public void ConvertJsonTest()
		{
			var jsonDataEngine = new JsonDataEngine();
			var jObject = jsonDataEngine.ConvertJson(JsonTemplate, JsonData);

			var title = jObject.SelectToken("data.title");
			Assert.IsNotNull(title);
			Assert.AreEqual("bob", title.ToString());

			var project0 = jObject.SelectToken("template.items[0].test[0]");
			Assert.IsNotNull(project0);

			var project0Title = project0.SelectToken("projectTitle");
			var project0Employer = project0.SelectToken("projectValues[0].owner");
			Assert.IsNotNull(project0Title);
			Assert.IsNotNull(project0Employer);

			Assert.AreEqual("project A", project0Title.ToString());
			Assert.AreEqual("Manuel", project0Employer.ToString());

		}
	}
}