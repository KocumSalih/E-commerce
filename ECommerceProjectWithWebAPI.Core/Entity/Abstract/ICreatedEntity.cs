﻿using System;

namespace ECommerceProjectWithWebAPI.Core.Entity.Abstract
{
    public interface ICreatedEntity
    {
        int CreatedUserId { get; set; }
        DateTime CreatedDate { get; set; }

    }
}
