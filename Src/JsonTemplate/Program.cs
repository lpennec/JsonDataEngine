using JsonTemplate;

var jsonTemplate = @"
	{
	  data: {
			title: ""$name"",
			projects: [
				{
					title: ""METRO"",
					totalhours: 100,
					percentage: 66
				},
				{
					title: ""ZF"",
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
	}
";

var jsonData = @"
	{ 
		name: ""bob"",
		projects: [ 
			{
				key: ""project A"", 
				values: [
					{ 
						EMPLOYER: ""Manuel"", 
						EMPLOYEE: ""LOIC""
					}, 
					{ 
						EMPLOYER: ""Manuel"", 
						EMPLOYEE: ""JENNY""
					}
				] 
			}, 
			{ 
				key: ""project B"", 
				values: [
					{ 
						EMPLOYER: ""Loic"", 
						EMPLOYEE: ""Adri""
					}, 
					{ 
						EMPLOYER: ""Loic"", 
						EMPLOYEE: ""Metin""
					}
				] 
			} 
		] 
	}";

JsonService.ConvertJson(jsonTemplate, jsonData);