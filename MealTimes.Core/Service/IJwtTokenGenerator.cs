﻿using MealTimes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.Service
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }

}
