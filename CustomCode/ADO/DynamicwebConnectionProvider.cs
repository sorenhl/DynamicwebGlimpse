using System;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;
using Dynamicweb;
using Glimpse.Ado.AlternateType;

namespace GlimpseDemo.CustomCode.ADO
{


    public class DynamicwebConnectionProvider : IDatabaseConnectionProvider
    {
        #region Private classes for castable
        private class CastableGlimpseDbCommand : GlimpseDbCommand
        {
            public CastableGlimpseDbCommand(DbCommand innerCommand)
                : base(innerCommand)
            {
            }

            public CastableGlimpseDbCommand(DbCommand innerCommand, GlimpseDbConnection connection)
                : base(innerCommand, connection)
            {
            }

            static public explicit operator SqlCommand(CastableGlimpseDbCommand dbCmd)
            {
                return (SqlCommand)dbCmd.InnerCommand;
            }
        }

        private class CastableGlimpseDbConnection : GlimpseDbConnection
        {
            public CastableGlimpseDbConnection(DbConnection connection)
                : base(connection)
            {
            }

            static public explicit operator SqlConnection(CastableGlimpseDbConnection dbConnection)
            {
                return (SqlConnection)dbConnection.InnerConnection;
            }

            protected override DbCommand CreateDbCommand()
            {
                return new CastableGlimpseDbCommand(InnerConnection.CreateCommand(), this);
            }
        }
        #endregion

        /// <summary>
        /// Hold Dynamicweb Database Connection Provider
        /// </summary>
        private readonly DatabaseConnectionProvider _dynamicwebDatabaseConnectionProvider = new DatabaseConnectionProvider();

        #region Implementation of IDatabaseConnectionProvider
        public System.Data.IDbDataAdapter CreateAdapter()
        {
            var adapter = _dynamicwebDatabaseConnectionProvider.CreateAdapter();

            // We do not want to track back-end activitities
            if (IsBackend()) return adapter;

            var dbAdapter = adapter as DbDataAdapter;
            return dbAdapter != null ? new GlimpseDbDataAdapter(dbAdapter) : adapter;
        }

        public System.Data.IDbConnection CreateConnection(string database)
        {
            var conn = _dynamicwebDatabaseConnectionProvider.CreateConnection(database);

            // We do not support OleDbConnections and we do not want back-end activities
            if (conn is OleDbConnection || IsBackend())
                return conn;

            var dbConn = conn as DbConnection;
            return dbConn != null ? new CastableGlimpseDbConnection(dbConn) : conn;
        }

        private bool IsBackend()
        {
            try
            {
                // Try/Catch to avoid Request is not available in this context
                return HttpContext.Current != null && HttpContext.Current.Request.Path.StartsWith("/admin/", StringComparison.InvariantCultureIgnoreCase);

            }
            catch (Exception)
            {
                return true;
            }
        }
        #endregion
    }
}
