﻿using Module11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module11.Models;

namespace Module11.Services
{
        public interface IStorage
        {
            Session GetSession(long chatId);
        }
}
