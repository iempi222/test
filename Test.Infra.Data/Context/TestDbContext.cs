using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Test.Domain.Models.Account;
using Test.Domain.Models.Events;
using Test.Domain.Models.Location;

namespace Test.Infra.Data.Context
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {

        }

        //my tables set here



        }
    }
}
