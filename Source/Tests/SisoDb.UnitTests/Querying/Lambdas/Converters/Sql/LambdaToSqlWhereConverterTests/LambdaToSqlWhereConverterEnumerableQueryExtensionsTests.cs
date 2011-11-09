﻿using System.Linq;
using NUnit.Framework;
using SisoDb.Dac;
using SisoDb.Querying;
using SisoDb.Querying.Lambdas.Converters.Sql;
using SisoDb.UnitTests.TestFactories;

namespace SisoDb.UnitTests.Querying.Lambdas.Converters.Sql.LambdaToSqlWhereConverterTests
{
    [TestFixture]
    public class LambdaToSqlWhereConverterEnumerableQueryExtensionsTests : LambdaToSqlWhereConverterTestBase
    {
        [Test]
        public void QxAny_OnStrings_GeneratesCorrectSqlQuery()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.Strings.QxAny(s => s == "Alpha" || s == "Bravo"));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            Assert.AreEqual(1, query.MemberPaths.Length);
            Assert.IsNotNull(query.MemberPaths.SingleOrDefault(m => m == "Strings"));
            Assert.AreEqual("((mem0.[StringValue] = @p0) or (mem0.[StringValue] = @p1))", query.CriteriaString);
        }

        [Test]
        public void QxAny_OnStrings_ExtractsCorrectParameters()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.Strings.QxAny(s => s == "Alpha" || s == "Bravo"));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            var expectedParameters = new[] { new DacParameter("@p0", "Alpha"), new DacParameter("@p1", "Bravo") };
            AssertQueryParameters(expectedParameters, query.Parameters);
        }

        [Test]
        public void QxAny_OnDecimals_GeneratesCorrectSqlQuery()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.Decimals.QxAny(e => e == 3.14M || e == -1.89M));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            Assert.AreEqual(1, query.MemberPaths.Length);
            Assert.IsNotNull(query.MemberPaths.SingleOrDefault(m => m == "Decimals"));
            Assert.AreEqual("((mem0.[FractalValue] = @p0) or (mem0.[FractalValue] = @p1))", query.CriteriaString);
        }

        [Test]
        public void QxAny_OnDecimals_ExtractsCorrectParameters()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.Decimals.QxAny(e => e == 3.14M || e == -1.89M));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            var expectedParameters = new[] { new DacParameter("@p0", 3.14m), new DacParameter("@p1", -1.89m) };
            AssertQueryParameters(expectedParameters, query.Parameters);
        }

        [Test]
        public void QxAny_OnListOfIntegers_GeneratesCorrectSqlQuery()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.ListOfIntegers.QxAny(v => v == 42));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            Assert.AreEqual(1, query.MemberPaths.Length);
            Assert.IsNotNull(query.MemberPaths.SingleOrDefault(m => m == "ListOfIntegers"));
            Assert.AreEqual("(mem0.[IntegerValue] = @p0)", query.CriteriaString);
        }

        [Test]
        public void QxAny_OnListOfIntegers_ExtractsCorrectParameters()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.ListOfIntegers.QxAny(v => v == 42));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            var expectedParameters = new[] { new DacParameter("@p0", 42) };
            AssertQueryParameters(expectedParameters, query.Parameters);
        }

        [Test]
        public void QxAny_OnChildItems_GeneratesCorrectSqlQuery()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.ChildItems.QxAny(c => c.Int1 == 42 || c.Int1 == 55));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            Assert.AreEqual(1, query.MemberPaths.Length);
            Assert.IsNotNull(query.MemberPaths.SingleOrDefault(m => m == "ChildItems.Int1"));
            Assert.AreEqual("((mem0.[IntegerValue] = @p0) or (mem0.[IntegerValue] = @p1))", query.CriteriaString);
        }

        [Test]
        public void QxAny_OnChildItems_ExtractsCorrectParameters()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.ChildItems.QxAny(c => c.Int1 == 42 || c.Int1 == 55));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            var expectedParameters = new[] { new DacParameter("@p0", 42), new DacParameter("@p1", 55) };
            AssertQueryParameters(expectedParameters, query.Parameters);
        }

        [Test]
        public void QxAny_WhenNested_GeneratesCorrectSqlQuery()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.ChildItems.QxAny(c => c.GrandChildItems.QxAny(c2 => c2.Int1 == 42)));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            Assert.AreEqual(1, query.MemberPaths.Length);
            Assert.IsNotNull(query.MemberPaths.SingleOrDefault(m => m == "ChildItems.GrandChildItems.Int1"));
            Assert.AreEqual("(mem0.[IntegerValue] = @p0)", query.CriteriaString);
        }

        [Test]
        public void QxAny_WhenNested_ExtractsCorrectParameters()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.ChildItems.QxAny(c => c.GrandChildItems.QxAny(c2 => c2.Int1 == 42)));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            var expectedParameters = new[] { new DacParameter("@p0", 42) };
            AssertQueryParameters(expectedParameters, query.Parameters);
        }

        [Test]
        public void QxAny_WhenValueArrayOnChildItem_GeneratesCorrectSqlQuery()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.ChildItems.QxAny(e => e.Integers.QxAny(e2 => e2 == 42)));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            Assert.AreEqual(1, query.MemberPaths.Length);
            Assert.IsNotNull(query.MemberPaths.SingleOrDefault(m => m == "ChildItems.Integers"));
            Assert.AreEqual("(mem0.[IntegerValue] = @p0)", query.CriteriaString);
        }

        [Test]
        public void QxAny_WhenValueArrayOnChildItem_ExtractsCorrectParameters()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.ChildItems.QxAny(e => e.Integers.QxAny(e2 => e2 == 42)));

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub<MyItem>(), parsedLambda);

            var expectedParameters = new[] { new DacParameter("@p0", 42) };
            AssertQueryParameters(expectedParameters, query.Parameters);
        }
    }
}