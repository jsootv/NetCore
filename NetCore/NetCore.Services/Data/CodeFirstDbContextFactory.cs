using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NetCore.Services.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Services.Data
{
    public class CodeFirstDbContextFactory
        : IDesignTimeDbContextFactory<CodeFirstDbContext>
    {
        /// <summary>
        /// appsettings.json 경로
        /// </summary>
        private const string _configPath = @"D:\Repository\jsootv\NetCore\NetCore\NetCore.Web.V5\appsettings.json";

        public CodeFirstDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CodeFirstDbContext>();
            // NetCore.Web.V5 프로젝트의 appsettings.json의 DB연결문자열을 가져와야 한다.
            optionsBuilder.UseSqlServer(new DbConnector(_configPath).GetConnectionString());

            return new CodeFirstDbContext(optionsBuilder.Options);
        }
    }
}
