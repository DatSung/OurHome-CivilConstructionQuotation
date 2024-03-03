﻿using SWP391.CHCQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.CHCQS.DataAccess.Repository.IRepository
{
    public interface IRejectedCustomQuotationRepository: IRepository<RejectionReport>
    {
        void Update(RejectionReport model);
    }
}