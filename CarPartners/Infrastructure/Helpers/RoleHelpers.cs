using NetCars.Areas.Home.Models.Db.Role;
using NetCars.Areas.Home.Models.View.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Infrastructure.Helpers
{
    public static class RoleHelpers
    {
        public static RoleModel ConvertToModel(RoleView result)
        {
            var roleModel = new RoleModel
            {
                RoleName = result.RoleName
            };

            return roleModel;
        }

        public static RoleView ConvertToView(RoleModel result)
        {
            var roleView = new RoleView
            {
                RoleName = result.RoleName
            };

            return roleView;
        }

        public static RoleModel MergeViewWithModel(RoleModel model, RoleView view)
        {
            model.RoleName = view.RoleName;

            return model;
        }
    }
}