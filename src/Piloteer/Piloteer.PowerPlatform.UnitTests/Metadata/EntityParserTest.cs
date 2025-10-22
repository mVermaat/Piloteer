using Piloteer.PowerPlatform.Metadata;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Piloteer.PowerPlatform.UnitTests.Metadata
{
    public class EntityParserTest
    {
        private readonly IAppSettingsProvider _appSettingsProvider;
        private readonly IAliasedRecordCache _recordCache;

        public EntityParserTest()
        {
            _appSettingsProvider = Substitute.For<IAppSettingsProvider>();
            _appSettingsProvider.GetRequiredAppSettingsValue("Formatting:DateFormat").Returns("yyyy-MM-dd");
            _appSettingsProvider.GetRequiredAppSettingsValue("Formatting:TimeFormat").Returns("HH:mm:ss");
            _appSettingsProvider.GetRequiredAppSettingsValue("Formatting:LanguageCode").Returns("1033"); // English (United States)

            _recordCache = Substitute.For<IAliasedRecordCache>();

            PowerPlatformErrors.AddErrors();
        }

        [Fact]
        public void TestParsingString()
        {
            var parser = new EntityParser(_appSettingsProvider);
            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                new StringAttributeMetadata("name")
                {
                    LogicalName = "name",
                });
            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("name", "Test Account"),
            };

            var entity = parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache);

            Assert.Equal("Test Account", entity.GetAttributeValue<string>("name"));
        }

        [Fact]
        public void TestParsingMemo()
        {
            var parser = new EntityParser(_appSettingsProvider);
            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                new MemoAttributeMetadata("name")
                {
                    LogicalName = "name",
                });
            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("name", "Test Account"),
            };

            var entity = parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache);

            Assert.Equal("Test Account", entity.GetAttributeValue<string>("name"));
        }

        [Fact]
        public void TestParsingEntityName()
        {
            var parser = new EntityParser(_appSettingsProvider);
            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                new EntityNameAttributeMetadata("name")
                {
                    LogicalName = "name",
                });
            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("name", "Test Account"),
            };

            var entity = parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache);

            Assert.Equal("Test Account", entity.GetAttributeValue<string>("name"));
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("TRUE", true)]
        [InlineData("FALSE", false)]
        [InlineData("Yes", true)]
        [InlineData("yes", true)]
        [InlineData("No", false)]
        [InlineData("no", false)]
        public void TestCorrectParsingBoolean(string inputValue, bool expectedValue)
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateBooleanAttributeMetadata("someboolvalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("someboolvalue", inputValue),
            };

            var entity = parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache);

            Assert.Equal(expectedValue, entity.GetAttributeValue<bool>("someboolvalue"));
        }

        [Fact]
        public void TestIncorrectParsingBoolean()
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateBooleanAttributeMetadata("someboolvalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("someboolvalue", "somethingwrong"),
            };

            AssertTestExecutionExceptionIsThrown(10001, () => parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache));
        }

        [Theory]
        [InlineData("10.54", 10.54d)]
        [InlineData("43,686.55", 43686.55d)]
        [InlineData("1234", 1234d)]
        [InlineData("0", 0d)]
        [InlineData("323,531", 323531d)]
        public void TestCorrectParsingDouble(string inputValue, double expectedValue)
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateDoubleAttributeMetadata("somedoublevalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("somedoublevalue", inputValue),
            };

            var entity = parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache);

            Assert.Equal(expectedValue, entity.GetAttributeValue<double>("somedoublevalue"));
        }

        [Fact]
        public void TestIncorrectParsingDouble()
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateDoubleAttributeMetadata("somedoublevalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("somedoublevalue", "NotANumber"),
            };

            AssertTestExecutionExceptionIsThrown(10006, () => parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache));
        }


        [Theory]
        [InlineData("10.54", 10.54)]
        [InlineData("43,686.55", 43686.55)]
        [InlineData("1234", 1234)]
        [InlineData("0", 0)]
        [InlineData("323,531", 323531)]
        public void TestCorrectParsingDecimal(string inputValue, decimal expectedValue)
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateDecimalAttributeMetadata("somedecimalvalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("somedecimalvalue", inputValue),
            };

            var entity = parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache);

            Assert.Equal(expectedValue, entity.GetAttributeValue<decimal>("somedecimalvalue"));
        }

        [Fact]
        public void TestIncorrectParsingDecimal()
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateDecimalAttributeMetadata("somedecimalvalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("somedecimalvalue", "NotANumber"),
            };

            AssertTestExecutionExceptionIsThrown(10006, () => parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache));
        }

        [Theory]
        [InlineData("1234", 1234)]
        [InlineData("0", 0)]
        public void TestCorrectParsingInteger(string inputValue, decimal expectedValue)
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateIntegerAttributeMetadata("someintegervalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("someintegervalue", inputValue),
            };

            var entity = parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache);

            Assert.Equal(expectedValue, entity.GetAttributeValue<int>("someintegervalue"));
        }

        [Theory]
        [InlineData("10.54")]
        [InlineData("43,686.55")]
        [InlineData("323,531")]
        [InlineData("NoNumber")]
        public void TestIncorrectParsingInteger(string inputValue)
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateIntegerAttributeMetadata("someintegervalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("someintegervalue", inputValue),
            };

            AssertTestExecutionExceptionIsThrown(10007, () => parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache));
        }

        [Theory]
        [InlineData("10.54", 10.54)]
        [InlineData("43,686.55", 43686.55)]
        [InlineData("1234", 1234)]
        [InlineData("0", 0)]
        [InlineData("323,531", 323531)]
        public void TestCorrectParsingMoney(string inputValue, decimal expectedValue)
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateMoneyAttributeMetadata("somemoneyvalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("somemoneyvalue", inputValue),
            };

            var entity = parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache);

            Assert.Equal(expectedValue, entity.GetAttributeValue<Money>("somemoneyvalue").Value);
        }

        [Fact]
        public void TestIncorrectParsingMoney()
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateMoneyAttributeMetadata("somemoneyvalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("somemoneyvalue", "NotANumber"),
            };

            AssertTestExecutionExceptionIsThrown(10006, () => parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache));
        }

        [Theory]
        [InlineData("{AACAD868-8B20-4B52-A081-4577388D3444}")]
        [InlineData("AACAD868-8B20-4B52-A081-4577388D3444")]
        [InlineData("aacad868-8b20-4b52-a081-4577388d3444")]
        [InlineData("{aacad868-8b20-4b52-a081-4577388d3444}")]
        public void TestCorrectParsingGuid(string inputValue)
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateUniqueIdentifierAttributeMetadata("someguidvalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("someguidvalue", inputValue),
            };

            var entity = parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache);

            Assert.Equal(Guid.Parse("{AACAD868-8B20-4B52-A081-4577388D3444}"), entity.GetAttributeValue<Guid>("someguidvalue"));
        }

        [Fact]
        public void TestIncorrectParsingGuid()
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateUniqueIdentifierAttributeMetadata("someguidvalue"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("someguidvalue", "NoGuid"),
            };

            AssertTestExecutionExceptionIsThrown(10008, () => parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache));
        }

        [Theory]
        [InlineData("2025-12-31 09:50:45", DateTimeFormat.DateAndTime, 2025, 12, 31, 8, 50, 45)]
        [InlineData("2025-12-31", DateTimeFormat.DateOnly, 2025, 12, 30, 23, 0, 0)]
        public void TestCorrectParsingDateTimeUserLocal(string inputValue, DateTimeFormat format, int year, int month, int day, int hour, int minute, int second)
        {
            TestCorrectParsingDateTime(inputValue, format, DateTimeBehavior.UserLocal, new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc));
        }

        [Theory]
        [InlineData("2025-12-31 09:50:45", DateTimeFormat.DateAndTime, 2025, 12, 31, 9, 50, 45)]
        [InlineData("2025-12-31", DateTimeFormat.DateOnly, 2025, 12, 31, 0, 0, 0)]
        public void TestCorrectParsingDateTimeTimeZoneIndependent(string inputValue, DateTimeFormat format, int year, int month, int day, int hour, int minute, int second)
        {
            TestCorrectParsingDateTime(inputValue, format, DateTimeBehavior.TimeZoneIndependent, new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc));
        }

        [Theory]
        [InlineData("2025-12-31 09:50:45", DateTimeFormat.DateAndTime, 2025, 12, 31, 9, 50, 45)]
        [InlineData("2025-12-31", DateTimeFormat.DateOnly, 2025, 12, 31, 0, 0, 0)]
        public void TestCorrectParsingDateDateOnly(string inputValue, DateTimeFormat format, int year, int month, int day, int hour, int minute, int second)
        {
            TestCorrectParsingDateTime(inputValue, format, DateTimeBehavior.DateOnly, new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc));
        }

        [Theory]
        [InlineData("2025-12-31", DateTimeFormat.DateAndTime)]
        [InlineData("2025/12/31 09:50:45", DateTimeFormat.DateAndTime)]
        [InlineData("2025-12-31 9:50:45", DateTimeFormat.DateAndTime)]
        [InlineData("NotADate", DateTimeFormat.DateAndTime)]
        [InlineData("2025-12-31 09:50:45", DateTimeFormat.DateOnly)]
        [InlineData("2025/12/31", DateTimeFormat.DateOnly)]
        [InlineData("NotADate", DateTimeFormat.DateOnly)]
        
        public void TestIncorrectParsingDateTimeUserLocal(string inputValue, DateTimeFormat format)
        {
            TestIncorrectParsingDateTime(inputValue, format, DateTimeBehavior.UserLocal);
        }

        [Theory]
        [InlineData("2025-12-31", DateTimeFormat.DateAndTime)]
        [InlineData("2025/12/31 09:50:45", DateTimeFormat.DateAndTime)]
        [InlineData("2025-12-31 9:50:45", DateTimeFormat.DateAndTime)]
        [InlineData("NotADate", DateTimeFormat.DateAndTime)]
        [InlineData("2025-12-31 09:50:45", DateTimeFormat.DateOnly)]
        [InlineData("2025/12/31", DateTimeFormat.DateOnly)]
        [InlineData("NotADate", DateTimeFormat.DateOnly)]

        public void TestIncorrectParsingDateTimeTimeZoneIndependent(string inputValue, DateTimeFormat format)
        {
            TestIncorrectParsingDateTime(inputValue, format, DateTimeBehavior.TimeZoneIndependent);
        }

        [Theory]
        [InlineData("2025-12-31", DateTimeFormat.DateAndTime)]
        [InlineData("2025/12/31 09:50:45", DateTimeFormat.DateAndTime)]
        [InlineData("2025-12-31 9:50:45", DateTimeFormat.DateAndTime)]
        [InlineData("NotADate", DateTimeFormat.DateAndTime)]
        [InlineData("2025-12-31 09:50:45", DateTimeFormat.DateOnly)]
        [InlineData("2025/12/31", DateTimeFormat.DateOnly)]
        [InlineData("NotADate", DateTimeFormat.DateOnly)]

        public void TestIncorrectParsingDateTimeTimeZoneDateOnly(string inputValue, DateTimeFormat format)
        {
            TestIncorrectParsingDateTime(inputValue, format, DateTimeBehavior.DateOnly);
        }

        [Theory]
        [InlineData("TestValue1", 1001)]
        [InlineData("TestValue2", 1002)]
        [InlineData("1001", 1001)]
        [InlineData("1002", 1002)]
        public void TestCorrectParsingPicklist(string inputValue, int expectedValue)
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreatePicklistAttributeMetadata("someoptionset", 
                    (1001, "TestValue1"), (1002, "TestValue2"), (1003, "TestValue3")));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("someoptionset", inputValue),
            };

            var entity = parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache);

            Assert.Equal(expectedValue, entity.GetAttributeValue<OptionSetValue>("someoptionset").Value);
        }

        [Theory]
        [InlineData("TestValue4")]
        [InlineData("1004")]
        public void TestIncorrectParsingPicklist(string inputValue)
        {
            var parser = new EntityParser(_appSettingsProvider);

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreatePicklistAttributeMetadata("someoptionset",
                    (1001, "TestValue1"), (1002, "TestValue2"), (1003, "TestValue3")));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("someoptionset", inputValue),
            };

            AssertTestExecutionExceptionIsThrown(10001, () => parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache));
        }

        [Fact]
        public async Task TestCorrectParsingLookup()
        {
            var parser = new EntityParser(_appSettingsProvider);
            var record = new Entity("account", Guid.NewGuid());
            await _recordCache.AddAsync("MyAlias", record);
            _recordCache.GetRequired("MyAlias").Returns(record.ToEntityReference());

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateLookupAttributeMetadata("somelookup"));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("somelookup", "MyAlias"),
            };

            var entity = parser.ParseEntity(md, inputAttributes, TimeZoneInfo.Utc, _recordCache);

            Assert.Equal(record.Id, entity.GetAttributeValue<EntityReference>("somelookup").Id);
        }

        private void TestIncorrectParsingDateTime(string inputValue, DateTimeFormat format, DateTimeBehavior behavior)
        {
            var parser = new EntityParser(_appSettingsProvider);
            var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); // Ensure the time zone is available

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateDateTimeAttributeMetadata("somedatevalue", format, behavior));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("somedatevalue", inputValue),
            };

            AssertTestExecutionExceptionIsThrown(10009, () => parser.ParseEntity(md, inputAttributes, cetTimeZone, _recordCache));
        }

        private void TestCorrectParsingDateTime(string inputValue, DateTimeFormat format, DateTimeBehavior behavior, DateTime expectedDateTime)
        {
            var parser = new EntityParser(_appSettingsProvider);
            var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); // Ensure the time zone is available

            var md = MetadataHelper.CreateEntityMetadata("fakeaccount",
                MetadataHelper.CreateDateTimeAttributeMetadata("somedatevalue", format, behavior));

            var inputAttributes = new UnparsedAttribute[]
            {
                new UnparsedAttribute("somedatevalue", inputValue),
            };

            var entity = parser.ParseEntity(md, inputAttributes, cetTimeZone, _recordCache);

            Assert.Equal(expectedDateTime, entity.GetAttributeValue<DateTime>("somedatevalue"));
        }

        private void AssertTestExecutionExceptionIsThrown(int expectedErrorCode, Action actionCode)
        {
            try
            {
                actionCode();
                Assert.Fail("Expected TestExecutionException was not thrown.");
            }
            catch (TestExecutionException ex)
            {
                Assert.Equal(expectedErrorCode, ex.ErrorCode);
                return;
            }
        }
    }
}
