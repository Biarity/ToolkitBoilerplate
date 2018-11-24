using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ToolkitBoilerplate.Controllers;

namespace ToolkitBoilerplate.Data
{
    public interface IAdditionalUserInfo
    {
        string UserName { get; set; }
    }

    [DataContract]
    public class ApplicationUser : IdentityUser<int>, IAdditionalUserInfo
    {

    }

    public class AdditionalUserInfoViewModel : IAdditionalUserInfo
    {
        [Required]
        [MinLength(3), MaxLength(10)]
        [RegularExpression("[A-Za-z0-9]+")]
        [PersonalData]
        public string UserName { get; set; }

        public void Map(ref ApplicationUser user)
        {
            user.UserName = UserName;
        }
    }
}
