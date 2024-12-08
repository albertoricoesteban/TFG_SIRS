using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using System;
using System.Collections.Generic;

namespace SIRS.Application.Interfaces
{
    public interface IRolAppService
    {
        RoleViewModel GetById(int id);
        IEnumerable<RoleViewModel> GetAll();


    }
}
