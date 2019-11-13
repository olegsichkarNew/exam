using IntreviewExam.DataAccessLayer.DbConstants;
using IntreviewExam.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntreviewExam.DataAccessLayer
{
    public class SqlParametersBuilder
    {
        #region Fields

        private readonly List<SqlParameter> _parameters;
        private SqlParameter errorCodeParam;
        private SqlParameter errorMessageParam;
        public SqlParametersBuilder()
        {
            _parameters = new List<SqlParameter>();
        }
        #endregion

        public SqlParametersBuilder WithDecimalNullableParam(string paramName, decimal? value, ParameterDirection direction = ParameterDirection.Input)
        {
            if (!value.HasValue)
            {
                _parameters.Add(new SqlParameter(paramName, value)
                {
                    SqlDbType = SqlDbType.Decimal,
                    DbType = DbType.Decimal,
                    Value = DBNull.Value,
                    Direction = direction
                });
            }
            else
            {
                _parameters.Add(new SqlParameter(paramName, value)
                {
                    SqlDbType = SqlDbType.Decimal,
                    DbType = DbType.Decimal,
                    Direction = direction
                });
            }
            return this;
        }
        public SqlParametersBuilder WithIntParam(string paramName, int value, ParameterDirection direction = ParameterDirection.Input)
        {
            _parameters.Add(new SqlParameter(paramName, value)
            {
                DbType = DbType.Int32,
                Direction = direction
            });

            return this;
        }
        public SqlParametersBuilder WithNullableDateTimeParam(string name, DateTime? value, ParameterDirection direction = ParameterDirection.Input)
        {
            SqlParameter parameter = !value.HasValue ?
            new SqlParameter(name, value)
            {
                DbType = DbType.DateTime,
                Value = DBNull.Value,
                Direction = direction
            }
            :
            new SqlParameter(name, value)
            {
                DbType = DbType.DateTime,
                Direction = direction
            };

            this._parameters.Add(parameter);
            return this;
        }
        public SqlParametersBuilder WithIndividualTableParam(string parameterName, IEnumerable<Individual> values)
        {
            var table = new DataTable();
            table.Columns.Add("CustomerCode", typeof(String));
            table.Columns.Add("DateOfBirth", typeof(DateTime));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("Gender", typeof(Int32));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("NationalID", typeof(string));

            if (values != null && values.Any())
            {
                foreach (var v in values)
                {
                    table.Rows.Add(new object[]
                    {
                        v.CustomerCode,
                        v.DateOfBirth,
                        v.FirstName,
                        v.Gender,
                        v.LastName,
                        v.NationalID,
                    });
                }
            }

            _parameters.Add(new SqlParameter(parameterName, table)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = UserDefinedTableTypes.IndividualTableTypeName,
                Direction = ParameterDirection.Input,
            });
            return this;
        }
        public SqlParametersBuilder WithSubjectRoleTableParam(string parameterName, IEnumerable<SubjectRole> values)
        {
            var table = new DataTable();
            table.Columns.Add("CustomerCode", typeof(String));
            table.Columns.Add("GuaranteeAmount", typeof(decimal));
            table.Columns.Add("GuaranteeAmountCurrency", typeof(Int32));
            table.Columns.Add("RoleOfCustomer", typeof(Int32));
          
            if (values != null && values.Any())
            {
                foreach (var v in values)
                {
                    table.Rows.Add(new object[]
                    {
                        v.CustomerCode,
                        v.GuaranteeAmount,
                        v.GuaranteeAmountCurrency,
                        v.RoleOfCustomer,
                       });
                }
            }

            _parameters.Add(new SqlParameter(parameterName, table)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = UserDefinedTableTypes.SubjectRoleTableTypeName,
                Direction = ParameterDirection.Input,
            });
            return this;
        }
        public SqlParametersBuilder WithStringTableParam(string paramName, string tableTypeName, IEnumerable<String> stringValues, ParameterDirection direction = ParameterDirection.Input)
        {
            var table = new DataTable();
            table.Columns.Add(paramName, typeof(String));
            if (stringValues != null && stringValues.Any())
            {
                foreach (String stringValue in stringValues)
                {
                    table.Rows.Add(stringValue);
                }
            }
            var param = new SqlParameter(paramName, table)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = tableTypeName,
                Direction = direction,
            };
            _parameters.Add(param);
            return this;
        }
        public SqlParametersBuilder WithStringParam(string name, string value, ParameterDirection direction = ParameterDirection.Input)
        {
            SqlParameter parameter = value == null ?
            new SqlParameter(name, DbType.String)
            {
                DbType = DbType.String,
                Direction = direction,
                Value = DBNull.Value
            }
            :
            new SqlParameter(name, value)
            {
                DbType = DbType.String,
                Direction = direction
            };
            _parameters.Add(parameter);

            return this;
        }
        public IEnumerable<SqlParameter> Build()
        {
            return _parameters;
        }
    }
}
