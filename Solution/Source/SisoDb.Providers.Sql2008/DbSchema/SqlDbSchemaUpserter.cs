﻿using System.Data;
using System.Text;
using SisoDb.Core;
using SisoDb.Dac;
using SisoDb.DbSchema;
using SisoDb.Providers;
using SisoDb.Sql2008.Dac;
using SisoDb.Structures.Schemas;

namespace SisoDb.Sql2008.DbSchema
{
    public class SqlDbSchemaUpserter : IDbSchemaUpserter
    {
        private readonly SqlDbStructuresSchemaBuilder _structuresDbSchemaBuilder;
        private readonly SqlDbIndexesSchemaBuilder _indexesDbSchemaBuilder;
        private readonly SqlDbUniquesSchemaBuilder _uniquesDbSchemaBuilder;

        private readonly SqlDbIndexesSchemaSynchronizer _indexesDbSchemaSynchronizer;
        private readonly SqlDbUniquesSchemaSynchronizer _uniquesDbSchemaSynchronizer;
        private readonly SqlDbClient _dbClient;

        public SqlDbSchemaUpserter(SqlDbClient dbClient)
        {
            _dbClient = dbClient.AssertNotNull("dbClient");

            var columnGenerator =
                SisoEnvironment.ProviderFactories.Get(dbClient.ProviderType).GetDbColumnGenerator();

            _structuresDbSchemaBuilder = new SqlDbStructuresSchemaBuilder(_dbClient.SqlStatements);
            _indexesDbSchemaBuilder = new SqlDbIndexesSchemaBuilder(_dbClient.SqlStatements, columnGenerator);
            _uniquesDbSchemaBuilder = new SqlDbUniquesSchemaBuilder(_dbClient.SqlStatements);

            _indexesDbSchemaSynchronizer = new SqlDbIndexesSchemaSynchronizer(_dbClient, columnGenerator);
            _uniquesDbSchemaSynchronizer = new SqlDbUniquesSchemaSynchronizer(_dbClient);
        }

        public void Upsert(IStructureSchema structureSchema)
        {
            var structuresTableName = structureSchema.GetStructureTableName();
            var indexesTableName = structureSchema.GetIndexesTableName();
            var uniquesTableName = structureSchema.GetUniquesTableName();

            var structuresTableExists = _dbClient.TableExists(structuresTableName);
            var indexesTableExists = _dbClient.TableExists(indexesTableName);
            var uniquesTableExists = _dbClient.TableExists(uniquesTableName);

            if (structuresTableExists && indexesTableExists && uniquesTableExists)
            {
                _indexesDbSchemaSynchronizer.Synchronize(structureSchema);
                _uniquesDbSchemaSynchronizer.Synchronize(structureSchema);
                return;
            }

            var structuresSql = structuresTableExists ? "" : _structuresDbSchemaBuilder.GenerateSql(structureSchema);
            var indexesSql = indexesTableExists ? "" : _indexesDbSchemaBuilder.GenerateSql(structureSchema);
            var uniquesSql = uniquesTableExists ? "" : _uniquesDbSchemaBuilder.GenerateSql(structureSchema);

            using (var cmd = _dbClient.CreateCommand(CommandType.Text, null,
                new DacParameter("entityHash", structureSchema.Hash),
                new DacParameter("entityName", structureSchema.Name)))
            {
                var sql = new StringBuilder();

                if (!structuresTableExists)
                {
                    sql.AppendLine(structuresSql);
                    //cmd.CommandText = structuresSql;
                    //cmd.ExecuteNonQuery();
                }

                if (!indexesTableExists)
                {
                    sql.AppendLine(indexesSql);
                    //cmd.CommandText = indexesSql;
                    //cmd.ExecuteNonQuery();
                }
                else
                    _indexesDbSchemaSynchronizer.Synchronize(structureSchema);

                if (!uniquesTableExists)
                {
                    sql.AppendLine(uniquesSql);
                    //cmd.CommandText = uniquesSql;
                    //cmd.ExecuteNonQuery();
                }
                else
                    _uniquesDbSchemaSynchronizer.Synchronize(structureSchema);

                cmd.CommandText = sql.ToString();
                cmd.ExecuteNonQuery();
            }
        }
    }
}