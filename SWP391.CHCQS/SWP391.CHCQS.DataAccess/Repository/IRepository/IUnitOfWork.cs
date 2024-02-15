﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.CHCQS.DataAccess.Repository.IRepository
{    
    public interface IUnitOfWork
    {
        IStandardQuotationRepository StandardQuotation { get; }
        IProjectRepository Project { get; }
        void Save();
    }
}
