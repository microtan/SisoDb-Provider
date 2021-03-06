using NUnit.Framework;
using SisoDb.Querying.Sql;
using SisoDb.Resources;
using SisoDb.Sql2008;
using SisoDb.Querying;

namespace SisoDb.UnitTests.Querying.QueryGeneration
{
    [TestFixture]
    public class Sql2008QueryGeneratorTests : QueryGeneratorTests
    {
        protected override IDbQueryGenerator GetQueryGenerator()
        {
            return new Sql2008QueryGenerator(new Sql2008Statements(), new SqlExpressionBuilder(() => new SqlWhereCriteriaBuilder()));
        }

        protected override IDbQuery On_GenerateQuery_WithTake2AndPage2WithSize10_GeneratesCorrectQuery()
        {
            var queryCommand = BuildQuery<MyClass>(q => q.Take(2).Page(2, 10).OrderBy(i => i.Int1));
            var generator = GetQueryGenerator();

            return generator.GenerateQuery(queryCommand);
        }

        [Test]
        public override void GenerateQuery_WithSkip13AndSort_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithSkip13AndSort_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual(1, sqlQuery.Parameters.Length);
            Assert.AreEqual("@skipRows", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(13, sqlQuery.Parameters[0].Value);
        }

        [Test]
        public override void GenerateQuery_WithTake2AndPage2WithSize10_GeneratesCorrectQuery()
        {
            var ex = Assert.Throws<SisoDbException>(() => base.On_GenerateQuery_WithTake2AndPage2WithSize10_GeneratesCorrectQuery());
            Assert.AreEqual(ExceptionMessages.PagingMissesOrderBy, ex.Message);
        }

        [Test]
        public void GenerateQuery_WithTake2AndPage2WithSize10AndSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithTake2AndPage2WithSize10_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual("@skipRows", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(20, sqlQuery.Parameters[0].Value);
            Assert.AreEqual("@takeRows", sqlQuery.Parameters[1].Name);
            Assert.AreEqual(30, sqlQuery.Parameters[1].Value);
        }

        [Test]
        public override void GenerateQuery_WithFirst_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithFirst_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual(0, sqlQuery.Parameters.Length);
        }

        [Test]
        public override void GenerateQuery_WithSingle_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithSingle_GeneratesCorrectQuery();
            
            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual(0, sqlQuery.Parameters.Length);
        }

        [Test]
        public override void GenerateQuery_WithWhere_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhere_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[0].Value);
        }

        [Test]
        public override void GenerateQuery_WithWhereHavingImplicitBool_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhereHavingImplicitBool_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(true, sqlQuery.Parameters[0].Value);
        }

        [Test]
        public override void GenerateQuery_WithWhereHavingExplicitBool_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhereHavingExplicitBool_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(true, sqlQuery.Parameters[0].Value);
        }

        [Test]
        public override void GenerateQuery_WithWhereUsingNullableIntIsNull_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhereUsingNullableIntIsNull_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual(0, sqlQuery.Parameters.Length);
        }

        [Test]
        public override void GenerateQuery_WithWhereUsingNullableIntIsNotNull_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhereUsingNullableIntIsNotNull_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual(0, sqlQuery.Parameters.Length);
        }

        [Test]
        public override void GenerateQuery_WithWhereUsingNullableIntHasValue_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhereUsingNullableIntHasValue_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual(0, sqlQuery.Parameters.Length);
        }

        [Test]
        public override void GenerateQuery_WithWhereUsingNullableIntHasValueFalse_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhereUsingNullableIntHasValueFalse_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual(0, sqlQuery.Parameters.Length);
        }

        [Test]
        public override void GenerateQuery_WithWhereUsingNegationOfNullableIntHasValue_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhereUsingNegationOfNullableIntHasValue_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual(0, sqlQuery.Parameters.Length);
        }

        [Test]
        public override void GenerateQuery_WithWhereContainingNullableIntComparedAgainstValue_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhereContainingNullableIntComparedAgainstValue_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[0].Value);
        }

        [Test]
        public override void GenerateQuery_WithWhereContainingNullableIntValueComparedAgainstValue_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhereContainingNullableIntValueComparedAgainstValue_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[0].Value);
        }

        [Test]
        public override void GenerateQuery_WithChainedWheres_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithChainedWheres_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);

            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(40, sqlQuery.Parameters[0].Value);

            Assert.AreEqual("@p1", sqlQuery.Parameters[1].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[1].Value);
        }

        [Test]
        public override void GenerateQuery_WithSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithSorting_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select s.[Json] from (select s.[StructureId], min(mem0.[Value]) mem0 from [MyClassStructure] s left join [MyClassIntegers] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' group by s.[StructureId]) rs inner join [MyClassStructure] s on s.[StructureId] = rs.[StructureId] order by mem0 Asc;",
                sqlQuery.Sql);
        }

        [Test]
        public override void GenerateQuery_WithWhereAndSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhereAndSorting_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select s.[Json] from (select s.[StructureId], min(mem0.[Value]) mem0 from [MyClassStructure] s left join [MyClassIntegers] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' where (mem0.[Value] = @p0) group by s.[StructureId]) rs inner join [MyClassStructure] s on s.[StructureId] = rs.[StructureId] order by mem0 Asc;",
                sqlQuery.Sql);

            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[0].Value);
        }

        [Test]
        public override void GenerateQuery_WithTake_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithTake_GeneratesCorrectQuery();
            SqlApprovals.Verify(sqlQuery.Sql);
        }

        [Test]
        public override void GenerateQuery_WithTakeAndSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithTakeAndSorting_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select top (11) s.[Json] from (select s.[StructureId], min(mem0.[Value]) mem0 from [MyClassStructure] s left join [MyClassIntegers] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' group by s.[StructureId]) rs inner join [MyClassStructure] s on s.[StructureId] = rs.[StructureId] order by mem0 Asc;",
                sqlQuery.Sql);
        }

        [Test]
        public override void GenerateQuery_WithTakeAndWhereAndSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithTakeAndWhereAndSorting_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select top (11) s.[Json] from (select s.[StructureId], min(mem0.[Value]) mem0 from [MyClassStructure] s left join [MyClassIntegers] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' where (mem0.[Value] = @p0) group by s.[StructureId]) rs inner join [MyClassStructure] s on s.[StructureId] = rs.[StructureId] order by mem0 Asc;",
                sqlQuery.Sql);

            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[0].Value);
        }

        [Test]
        public override void GenerateQuery_WithPagingAndWhereAndSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithPagingAndWhereAndSorting_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[0].Value);

            Assert.AreEqual("@skipRows", sqlQuery.Parameters[1].Name);
            Assert.AreEqual(0, sqlQuery.Parameters[1].Value);

            Assert.AreEqual("@takeRows", sqlQuery.Parameters[2].Name);
            Assert.AreEqual(10, sqlQuery.Parameters[2].Value);
        }

        [Test]
        public override void GenerateQuery_WithExplicitSortingOnTwoDifferentMemberTypesAndSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithExplicitSortingOnTwoDifferentMemberTypesAndSorting_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select s.[Json] from (select s.[StructureId], min(mem0.[Value]) mem0, min(mem1.[Value]) mem1 from [MyClassStructure] s left join [MyClassIntegers] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' left join [MyClassStrings] mem1 on mem1.[StructureId] = s.[StructureId] and mem1.[MemberPath] = 'String1' group by s.[StructureId]) rs inner join [MyClassStructure] s on s.[StructureId] = rs.[StructureId] order by mem0 Asc, mem1 Desc;",
                sqlQuery.Sql);
        }

        [Test]
        public override void GenerateQuery_WithSortingOnTwoDifferentMemberOfSameType_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithSortingOnTwoDifferentMemberOfSameType_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select s.[Json] from (select s.[StructureId], min(mem0.[Value]) mem0, min(mem1.[Value]) mem1 from [MyClassStructure] s left join [MyClassIntegers] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' left join [MyClassIntegers] mem1 on mem1.[StructureId] = s.[StructureId] and mem1.[MemberPath] = 'Int2' group by s.[StructureId]) rs inner join [MyClassStructure] s on s.[StructureId] = rs.[StructureId] order by mem0 Asc, mem1 Asc;",
                sqlQuery.Sql);
        }

        [Test]
        public override void GenerateQuery_WithEnum_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithEnum_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual("Value1", sqlQuery.Parameters[0].Value);
            Assert.AreEqual("@p1", sqlQuery.Parameters[1].Name);
            Assert.AreEqual("Value2", sqlQuery.Parameters[1].Value);
        }

        [Test]
        public override void GenerateQuery_With_StringQxStartsWith_or_IntEquals_or_ListOfStringsQxAny_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_With_StringQxStartsWith_or_IntEquals_or_ListOfStringsQxAny_GeneratesCorrectQuery();

            SqlApprovals.Verify(sqlQuery.Sql);
            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual("Foo%", sqlQuery.Parameters[0].Value);
            Assert.AreEqual("@p1", sqlQuery.Parameters[1].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[1].Value);
            Assert.AreEqual("@p2", sqlQuery.Parameters[2].Name);
            Assert.AreEqual("Bar", sqlQuery.Parameters[2].Value);
        }
    }
}