﻿using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Domain
{
    [CollectionName("users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
    }
}
